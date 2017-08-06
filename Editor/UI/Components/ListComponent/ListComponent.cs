using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// A component for list data.
    /// </summary>
    public class ListComponent : IEditorView
    {
        /// <summary>
        /// The current scroll position.
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        /// Items to render.
        /// </summary>
        protected readonly List<ListItem> _items = new List<ListItem>();

        /// <summary>
        /// Tracks which list item is currently selected.
        /// </summary>
        protected ListItem _selected;

        /// <summary>
        /// Filtering criteria. The list will only render list items for which
        /// ListItem::Matches(filter) evaluates to true.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets/sets the currently selected list item + dispatches an event.
        /// </summary>
        public ListItem Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selected == value)
                {
                    return;
                }

                if (null != _selected)
                {
                    _selected.Selected = false;
                }

                _selected = value;

                if (null != _selected)
                {
                    _selected.Selected = true;
                }

                if (null != OnSelected)
                {
                    OnSelected(Selected);
                }

                Repaint();
            }
        }

        /// <summary>
        /// Retrieves the current set of items.
        /// </summary>
        public ListItem[] Items
        {
            get
            {
                return _items.ToArray();
            }
        }

        /// <summary>
        /// Retrieves the current set of items.
        /// </summary>
        public ListItem[] Visible
        {
            get
            {
                return _items
                    .Where(item => string.IsNullOrEmpty(Filter) || item.Matches(Filter))
                    .ToArray();
            }
        }
        
        /// <summary>
        /// Dispatched when a different list item has been selected.
        /// </summary>
        public event Action<ListItem> OnSelected;

        /// <summary>
        /// Called when a repaint has been requested.
        /// </summary>
        public event Action OnRepaintRequested;

        /// <summary>
        /// Draws the list.
        /// </summary>
        public virtual void Draw()
        {
            if (_items.Count == 0)
            {
                return;
            }

            _scrollPosition = GUILayout.BeginScrollView(
                _scrollPosition,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            ListItem selected = null;

            foreach (var item in Visible)
            {
                if (item.Draw())
                {
                    selected = item;
                }
            }

            GUILayout.EndScrollView();

            // set it here to prevent draw issues
            if (null != selected)
            {
                Selected = selected;
            }
        }

        /// <summary>
        /// Populates the list with an IEnumerable of items.
        /// </summary>
        /// <param name="items"></param>
        public virtual void Populate(IEnumerable<ListItem> items)
        {
            _items.Clear();

            if (null != items)
            {
                _items.AddRange(items);
            }

            if (_items.Count > 0)
            {
                Selected = _items[0];
            }
            else
            {
                Selected = null;
            }
        }

        /// <summary>
        /// Appends the list of items to the end of the current items.
        /// </summary>
        /// <param name="items"></param>
        public virtual void Append(IEnumerable<ListItem> items)
        {
            if (null != items)
            {
                _items.AddRange(items);

                Repaint();
            }
        }

        /// <summary>
        /// Clears list.
        /// </summary>
        public void Clear()
        {
            Populate(new ListItem[0]);
        }

        /// <summary>
        /// Requests a repaint.
        /// </summary>
        protected void Repaint()
        {
            if (null != OnRepaintRequested)
            {
                OnRepaintRequested();
            }
        }
    }
}