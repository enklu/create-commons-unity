using System;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlTypeAttribute : CallbackOrderAttribute
    {
        public readonly Type Type;

        public ControlTypeAttribute(Type type)
        {
            Type = type;
        }
    }

    public abstract class Control
    {
        public abstract bool Draw(string label, ref object @object, ref ControlParameter[] parameters);
    }

    [ControlType(typeof(int))]
    public class IntControl : Control
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = EditorGUILayout.IntField(label, (int) @object);
            var changed = value != (int) @object;

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(short))]
    public class ShortControl : IntControl
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = (short) Mathf.Clamp(
                EditorGUILayout.IntField(label, (short) @object),
                short.MinValue,
                short.MaxValue);
            var changed = value != (short) @object;

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(byte))]
    public class ByteControl : IntControl
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = (byte) Mathf.Clamp(
                EditorGUILayout.IntField(label, (byte) @object),
                byte.MinValue,
                byte.MaxValue);
            var changed = value != (byte) @object;

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(long))]
    public class LongControl : IntControl
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            // TODO: Write custom long renderer.
            var value = (long) EditorGUILayout.IntField(label, Convert.ToInt32((long) @object));
            var changed = value != (long) @object;

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(float))]
    public class FloatControl : Control
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = EditorGUILayout.FloatField(label, (float)@object);
            var changed = !Mathf.Approximately(value, (float)@object);

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(double))]
    public class DoubleControl : FloatControl
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            // TODO: Write custom double renderer.
            var floatValue = Convert.ToSingle((double) @object);
            var value = (double) EditorGUILayout.FloatField(label, floatValue);
            var changed = !Mathf.Approximately((float) value, floatValue);

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(string))]
    public class StringControl : Control
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = EditorGUILayout.TextField(label, (string) @object);
            var changed = value != (string) @object;

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(bool))]
    public class BoolControl : Control
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = EditorGUILayout.Toggle(label, (bool) @object);
            var changed = value != (bool) @object;

            @object = value;

            return changed;
        }
    }

    [ControlType(typeof(Enum))]
    public class EnumControl : Control
    {
        public override bool Draw(string label, ref object @object, ref ControlParameter[] parameters)
        {
            var value = EditorGUILayout.EnumPopup(label, (Enum) @object);
            var changed = value.Equals((Enum) @object);

            @object = value;

            return changed;
        }
    }
}
