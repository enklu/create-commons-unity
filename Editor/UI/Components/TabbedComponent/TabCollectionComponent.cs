using System;
using UnityEditor;
using UnityEngine;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Tabbed component.
    /// </summary>
    public class TabCollectionComponent
    {
        /// <summary>
        /// A button at the end of the tab list.
        /// </summary>
        public class ActionButton
        {
            /// <summary>
            /// Label.
            /// </summary>
            public string Label;

            /// <summary>
            /// Called when the action button has been pressed.
            /// </summary>
            public Action<ActionButton> OnAction;
        }

        /// <summary>
        /// Tabs to draw.
        /// </summary>
        public TabComponent[] Tabs
        {
            get
            {
                return _tabs;
            }
            set
            {
                _tabs = value;
                _tab = 0;

                for (int i = 0, len = _tabs.Length; i < len; i++)
                {
                    var tab = _tabs[i];

                    tab.OnRepaintRequested -= Repaint;
                    tab.OnRepaintRequested += Repaint;
                }
            }
        }

        /// <summary>
        /// Action buttons to draw.
        /// </summary>
        public ActionButton[] Actions { get; set; }

        /// <summary>
        /// Called when the component requests a repaint.
        /// </summary>
        public event Action OnRepaintRequested;

        /// <summary>
        /// Tabs!
        /// </summary>
        private TabComponent[] _tabs;

        /// <summary>
        /// Index of current tab we are drawing/
        /// </summary>
        private int _tab;

        /// <summary>
        /// Draws controls.
        /// </summary>
        public void Draw()
        {
            DrawLabels();
            DrawTab();
        }

        /// <summary>
        /// Draws tab labels.
        /// </summary>
        private void DrawLabels()
        {
            EditorGUILayout.BeginHorizontal();

            // tabs
            if (null != Tabs)
            {
                for (int i = 0, len = Tabs.Length; i < len; i++)
                {
                    var tab = Tabs[i];
                    if (null == tab)
                    {
                        continue;
                    }

                    var label = string.IsNullOrEmpty(tab.Label)
                        ? "(Unnamed)"
                        : tab.Label;

                    if (_tab == i)
                    {
                        EditorUtils.PushEnabled(false);
                        GUILayout.Button(label, "TabSelected", GUILayout.ExpandHeight(false));
                        EditorUtils.PopEnabled();
                    }
                    else if (GUILayout.Button(label, "TabUnselected", GUILayout.ExpandHeight(false)))
                    {
                        _tab = i;

                        Repaint();
                    }
                }
            }

            // action buttons
            if (null != Actions)
            {
                for (int i = 0, len = Actions.Length; i < len; i++)
                {
                    var action = Actions[i];
                    var label = string.IsNullOrEmpty(action.Label)
                        ? "(Unnamed)"
                        : action.Label;

                    if (GUILayout.Button(
                        label,
                        "TabUnselected",
                        GUILayout.ExpandHeight(false),
                        GUILayout.ExpandWidth(false)))
                    {
                        if (null != action.OnAction)
                        {
                            action.OnAction.Invoke(action);
                        }

                        Repaint();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Renders the current tab.
        /// </summary>
        private void DrawTab()
        {
            if (_tab < 0 || _tab >= Tabs.Length)
            {
                return;
            }

            var tab = Tabs[_tab];
            if (null == tab)
            {
                return;
            }

            tab.Draw();
        }

        /// <summary>
        /// Calls the OnRepaintRequested method.
        /// </summary>
        private void Repaint()
        {
            if (null != OnRepaintRequested)
            {
                OnRepaintRequested();
            }
        }
    }
}