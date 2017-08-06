using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    public class FormInspector : IEditorView
    {
        [Flags]
        public enum RenderOptions
        {
            None,
            Alphabetize
        }

        private RenderOptions _options;

        private object _value;

        private readonly Dictionary<Type, Control> _controls = new Dictionary<Type, Control>();

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                // repaint immediately
                if (null != OnRepaintRequested)
                {
                    OnRepaintRequested();
                }
            }
        }

        public event Action OnRepaintRequested;

        public FormInspector(RenderOptions options = RenderOptions.None)
        {
            // gather the custom controls
            ForAllTypes(type =>
            {
                if (type.IsSubclassOf(typeof(Control)))
                {
                    var attributes = type.GetCustomAttributes(
                        typeof(ControlTypeAttribute),
                        true);
                    if (0 == attributes.Length)
                    {
                        return;
                    }

                    var controlTypeAttribute = (ControlTypeAttribute) attributes[0];
                    _controls[controlTypeAttribute.Type] = (Control) Activator.CreateInstance(type);
                }
            });
        }

        public void Draw()
        {
            if (null == _value)
            {
                return;
            }

            DrawObjectFields(_value);
        }

        private void DrawObjectFields(object value)
        {
            var shouldRepaint = false;
            var parameters = new ControlParameter[0];
            var type = value.GetType();

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            if (0 != (_options & RenderOptions.Alphabetize))
            {
                var sortedFields = fields.ToList();
                sortedFields.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));

                fields = sortedFields.ToArray();
            }

            for (int i = 0, ilen = fields.Length; i < ilen; i++)
            {
                var field = fields[i];
                var fieldType = field.FieldType;

                Control control;
                if (!_controls.TryGetValue(fieldType, out control))
                {
                    if (fieldType.IsEnum)
                    {
                        control = _controls[typeof(Enum)];
                    }
                    else
                    {
                        continue;
                    }
                }

                var attributes = field.GetCustomAttributes(true);

                // TODO: attributes > parameters

                var fieldValue = field.GetValue(value);
                if (control.Draw(field.Name, ref fieldValue, ref parameters))
                {
                    field.SetValue(value, fieldValue);

                    shouldRepaint = true;
                }
            }

            if (shouldRepaint)
            {
                if (null != OnRepaintRequested)
                {
                    OnRepaintRequested();
                }
            }
        }

        private static void ForAllTypes(Action<Type> action)
        {
            // catch exceptions
            Assembly[] assemblies;
            try
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            catch (AppDomainUnloadedException appDomainException)
            {
                Debug.LogError("Could not load assemblies : " + appDomainException);
                return;
            }

            for (int i = 0, ilen = assemblies.Length; i < ilen; i++)
            {
                var assembly = assemblies[i];
                Type[] types = null;

                try
                {
                    // only exported types
                    types = assembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException typeLoadException)
                {
                    Debug.LogError(typeLoadException);

                    // pull types off of _exception_
                    types = typeLoadException.Types;
                }

                if (null == types)
                {
                    continue;
                }

                // execute action on all types
                for (int j = 0, jlen = types.Length; j < jlen; j++)
                {
                    var type = types[j];

                    action(type);
                }
            }
        }
    }
}