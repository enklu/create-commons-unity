using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Subclass of ListComponent that renders a static number of ListItems and
    /// a selection of pages. This is helpful for extremely long lists.
    /// </summary>
    public class PaginatedListComponent : ListComponent
    {
        /// <summary>
        /// The number of elements per page.
        /// </summary>
        private int _elementsPerPage = 10;

        /// <summary>
        /// The current page the component is rendering.
        /// </summary>
        private int _page = 0;

        /// <summary>
        /// The last page.
        /// </summary>
        private int _maxPage = 0;

        /// <summary>
        /// Position for scroll controls.
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        /// Controls the number of elements the component renders per page.
        /// </summary>
        public int ElementsPerPage
        {
            get => _elementsPerPage;
            set
            {
                _elementsPerPage = value;
                RecalculateMaxPages();

                Repaint();
            }
        }

        /// <summary>
        /// Draws the ListItems and page selection.
        /// </summary>
        public override void Draw()
        {
            if (0 == _items.Count)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            {
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                {
                    var start = _page * _elementsPerPage;
                    var end = Mathf.Min(_items.Count, (_page + 1) * _elementsPerPage);
                    for (var i = start; i < end; i++)
                    {
                        _items[i].Draw();
                    }
                }
                EditorGUILayout.EndScrollView();

                GUILayout.FlexibleSpace();

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();

                    EditorUtils.PushEnabled(_page > 0);
                    if (GUILayout.Button("<<", GUILayout.ExpandWidth(false)))
                    {
                        _page = Mathf.Clamp(_page - 1, 0, _maxPage);
                        Repaint();
                    }
                    EditorUtils.PopEnabled();

                    GUILayout.Label($"Page {_page + 1}/{_maxPage + 1}");
                    
                    EditorUtils.PushEnabled(_page < _maxPage);
                    if (GUILayout.Button(">>", GUILayout.ExpandWidth(false)))
                    {
                        _page = Mathf.Clamp(_page + 1, 0, _maxPage);
                        Repaint();
                    }

                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
            EditorGUILayout.EndVertical();
        }

        public override void Populate(IEnumerable<ListItem> items)
        {
            base.Populate(items);
            
            RecalculateMaxPages();
            Repaint();
        }

        public override void Append(IEnumerable<ListItem> items)
        {
            base.Append(items);
            
            RecalculateMaxPages();
            Repaint();
        }

        private void RecalculateMaxPages()
        {
            _maxPage = _items.Count / _elementsPerPage;
        }
    }
}