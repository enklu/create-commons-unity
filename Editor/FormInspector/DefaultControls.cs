using System;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Attribute that binds a ControlRenderer to a Type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControlTypeAttribute : CallbackOrderAttribute
    {
        /// <summary>
        /// The Type to render controls for.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Creates a new attribute.
        /// </summary>
        /// <param name="type">The Type to render controls for.</param>
        public ControlTypeAttribute(Type type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// Base class for control renderer implementations.
    /// </summary>
    public abstract class ControlRenderer
    {
        /// <summary>
        /// Renders controls for a type.
        /// </summary>
        /// <param name="label">The name of the object the renderer is drawing controls for.</param>
        /// <param name="object">The current value.</param>
        /// <param name="parameters">Other parameters that may be passed in.</param>
        /// <returns></returns>
        public abstract bool Draw(
            string label,
            ref object @object,
            ref ControlRendererParameter[] parameters);
    }

    /// <summary>
    /// Renders controls for an int.
    /// </summary>
    [ControlType(typeof(int))]
    public class IntControlRenderer : ControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            var value = EditorGUILayout.IntField(label, (int) @object);
            var changed = value != (int) @object;

            @object = value;

            return changed;
        }
    }

    /// <summary>
    /// Renders controls for a short.
    /// </summary>
    [ControlType(typeof(short))]
    public class ShortControlRenderer : IntControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
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

    /// <summary>
    /// Renders controls for a byte.
    /// </summary>
    [ControlType(typeof(byte))]
    public class ByteControlRenderer : IntControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
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

    /// <summary>
    /// Renders controls for a long.
    /// </summary>
    [ControlType(typeof(long))]
    public class LongControlRenderer : IntControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            // TODO: Write custom long renderer.
            var value = (long) EditorGUILayout.IntField(label, Convert.ToInt32((long) @object));
            var changed = value != (long) @object;

            @object = value;

            return changed;
        }
    }

    /// <summary>
    /// Renders controls for a float.
    /// </summary>
    [ControlType(typeof(float))]
    public class FloatControlRenderer : ControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            var value = EditorGUILayout.FloatField(label, (float)@object);
            var changed = !Mathf.Approximately(value, (float)@object);

            @object = value;

            return changed;
        }
    }

    /// <summary>
    /// Renders controls for a double.
    /// </summary>
    [ControlType(typeof(double))]
    public class DoubleControlRenderer : FloatControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            // TODO: Write custom double renderer.
            var floatValue = Convert.ToSingle((double) @object);
            var value = (double) EditorGUILayout.FloatField(label, floatValue);
            var changed = !Mathf.Approximately((float) value, floatValue);

            @object = value;

            return changed;
        }
    }

    /// <summary>
    /// Renders controls for a string.
    /// </summary>
    [ControlType(typeof(string))]
    public class StringControlRenderer : ControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            var value = EditorGUILayout.TextField(label, (string) @object);
            var changed = value != (string) @object;

            @object = value;

            return changed;
        }
    }

    /// <summary>
    /// Renders controls for a bool.
    /// </summary>
    [ControlType(typeof(bool))]
    public class BoolControlRenderer : ControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            var value = EditorGUILayout.Toggle(label, (bool) @object);
            var changed = value != (bool) @object;

            @object = value;

            return changed;
        }
    }

    /// <summary>
    /// Renders controls for an Enum.
    /// </summary>
    [ControlType(typeof(Enum))]
    public class EnumControlRenderer : ControlRenderer
    {
        /// <inheritdoc />
        public override bool Draw(string label, ref object @object, ref ControlRendererParameter[] parameters)
        {
            var value = EditorGUILayout.EnumPopup(label, (Enum) @object);
            var changed = value.Equals((Enum) @object);

            @object = value;

            return changed;
        }
    }
}
