using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    public class PaginatedListComponent : ListComponent
    {
        private int _elementsPerPage = 10;
        private int _page = 0;
        private int _maxPage = 0;

        private Vector2 _scrollPosition;

        public int ElementsPerPage
        {
            get { return _elementsPerPage; }
            set
            {
                _elementsPerPage = value;
                RecalculateMaxPages();

                Repaint();
            }
        }

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

                    GUILayout.Label(string.Format(
                        "Page {0}/{1}",
                        _page + 1,
                        _maxPage + 1));
                    
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