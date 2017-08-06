using System;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// A list item for an ListComponent.
    /// </summary>
    public class ListItem
    {
        /// <summary>
        /// The label for this list item.
        /// </summary>
        public string Label;

        /// <summary>
        /// The value of this list item.
        /// </summary>
        public object Value;

        /// <summary>
        /// The tooltip of this list item.
        /// </summary>
        public string Tooltip;

        /// <summary>
        /// True if this ListItem is part of the current selection. Should not
        /// be set directly, but through ListComponent.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Called when clicked.
        /// </summary>
        public event Action<ListItem> OnClicked;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListItem(string label, object value)
        {
            Label = label;
            Value = value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListItem(string label)
            : this(label, null)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListItem()
            : this(string.Empty, null)
        {

        }

        /// <summary>
        /// Returns the Value cast to a specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ValueAs<T>()
        {
            return (T) Value;
        }

        /// <summary>
        /// Draws the list item. Returns true if the item was clicked on.
        /// </summary>
        /// <returns></returns>
        public virtual bool Draw()
        {
            if (!string.IsNullOrEmpty(Label))
            {
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
                GUILayout.Label(Label);
                EditorGUILayout.EndVertical();
                
                var clicked = EditorUtils.WasLastRectClicked();
                if (clicked)
                {
                    Clicked();
                }

                return clicked;
            }

            return false;
        }

        /// <summary>
        /// Returns true if this list item matches the current filter.
        /// </summary>
        public virtual bool Matches(string filter)
        {
            return Label.ToLowerInvariant().Contains(filter);
        }

        /// <summary>
        /// Called when clicked.
        /// </summary>
        protected virtual void Clicked()
        {
            if (null != OnClicked)
            {
                OnClicked(this);
            }
        }
    }
}