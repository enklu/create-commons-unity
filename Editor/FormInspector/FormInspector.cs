using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Generic inspector for objects. Automatically shows controls for all
    /// fields.
    /// </summary>
    public class FormInspector : IEditorView
    {
        /// <summary>
        /// Options for rendering controls.
        /// </summary>
        [Flags]
        public enum RenderOptions
        {
            /// <summary>
            /// Default options.
            /// </summary>
            None,

            /// <summary>
            /// Sets the inspector to alphabetize controls.
            /// </summary>
            Alphabetize
        }

        /// <summary>
        /// Options for rendering forms.
        /// </summary>
        private readonly RenderOptions _options;

        /// <summary>
        /// Backing variable for Value property.
        /// </summary>
        private object _value;

        /// <summary>
        /// Lookup from type to the renderer that creates controls for it.
        /// </summary>
        private readonly Dictionary<Type, ControlRenderer> _controls = new Dictionary<Type, ControlRenderer>();

        /// <summary>
        /// Value to draw controls for.
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;

                // repaint immediately
                OnRepaintRequested?.Invoke();
            }
        }

        /// <summary>
        /// Event to call when this inspector needs a repaint.
        /// </summary>
        public event Action OnRepaintRequested;

        /// <summary>
        /// Creates a new FormInspector.
        /// </summary>
        /// <param name="options">Addional options for rendering.</param>
        public FormInspector(RenderOptions options = RenderOptions.None)
        {
            _options = options;

            // gather the custom controls
            ForAllTypes(type =>
            {
                if (!type.IsSubclassOf(typeof(ControlRenderer)))
                {
                    return;
                }

                var attributes = type.GetCustomAttributes(
                    typeof(ControlTypeAttribute),
                    true);
                if (0 == attributes.Length)
                {
                    return;
                }

                var controlTypeAttribute = (ControlTypeAttribute) attributes[0];
                _controls[controlTypeAttribute.Type] = (ControlRenderer) Activator.CreateInstance(type);
            });
        }

        /// <summary>
        /// Draws the inspector.
        /// </summary>
        public void Draw()
        {
            if (null == _value)
            {
                return;
            }

            DrawObjectFields(_value);
        }

        /// <summary>
        /// Draws controls for each field.
        /// </summary>
        /// <param name="value"></param>
        private void DrawObjectFields(object value)
        {
            var shouldRepaint = false;
            var parameters = new ControlRendererParameter[0];
            var type = value.GetType();

            var fields = GetFields(type);
            for (int i = 0, ilen = fields.Length; i < ilen; i++)
            {
                var field = fields[i];
                var fieldType = field.FieldType;

                ControlRenderer controlRenderer;
                if (!_controls.TryGetValue(fieldType, out controlRenderer))
                {
                    if (fieldType.IsEnum)
                    {
                        controlRenderer = _controls[typeof(Enum)];
                    }
                    else
                    {
                        continue;
                    }
                }

                // TODO: attributes > parameters
                //var attributes = field.GetCustomAttributes(true);

                var fieldValue = field.GetValue(value);
                if (controlRenderer.Draw(field.Name, ref fieldValue, ref parameters))
                {
                    field.SetValue(value, fieldValue);

                    shouldRepaint = true;
                }
            }

            if (shouldRepaint)
            {
                OnRepaintRequested?.Invoke();
            }
        }

        /// <summary>
        /// Retrieves fields for a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private FieldInfo[] GetFields(IReflect type)
        {
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

            // alphabetize the fields
            if (0 != (_options & RenderOptions.Alphabetize))
            {
                var sortedFields = fields.ToList();
                sortedFields.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));

                fields = sortedFields.ToArray();
            }
            return fields;
        }

        /// <summary>
        /// Allows iteration over all types.
        /// </summary>
        /// <param name="action"></param>
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
                Type[] types;

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