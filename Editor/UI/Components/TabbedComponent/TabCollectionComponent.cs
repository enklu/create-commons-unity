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
            get => _tabs;
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
        /// Gets/sets the zero indexed current tab.
        /// </summary>
        public int CurrentTab
        {
            get => _tab;
            set
            {
                value = Mathf.Clamp(value, 0, _tabs.Length - 1);

                if (_tab == value)
                {
                    return;
                }

                _tab = value;

                Repaint();
            }
        }
        
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
            EditorGUILayout.BeginHorizontal();
            {
                DrawLabels();
                DrawTab();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws tab labels.
        /// </summary>
        private void DrawLabels()
        {
            EditorGUILayout.BeginVertical(
                GUILayout.Width(EditorGUIUtility.fieldWidth));

            // draw each tab
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
                        GUILayout.Button(
                            label,
                            "TabSelected",
                            GUILayout.ExpandHeight(false),
                            GUILayout.ExpandWidth(true));
                        EditorUtils.PopEnabled();
                    }
                    else
                    {
                        EditorUtils.PushEnabled(GUI.enabled && tab.Enabled);
                        if (GUILayout.Button(
                            label,
                            "TabUnselected",
                            GUILayout.ExpandHeight(false),
                            GUILayout.ExpandWidth(true)))
                        {
                            _tab = i;

                            Repaint();
                        }
                        EditorUtils.PopEnabled();
                    }
                }
            }

            EditorGUILayout.EndVertical();
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

            Tabs[_tab]?.Draw();
        }

        /// <summary>
        /// Calls the OnRepaintRequested method.
        /// </summary>
        private void Repaint()
        {
            OnRepaintRequested?.Invoke();
        }
    }
}