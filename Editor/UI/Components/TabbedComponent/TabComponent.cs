using System;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Component for rendering tabs.
    /// </summary>
    public class TabComponent : IEditorView
    {
        /// <summary>
        /// Backing variable for Label property.
        /// </summary>
        private string _label;

        /// <summary>
        /// Backing property for Enabled property.
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// Label of tab.
        /// </summary>
        public string Label
        {
            get => _label;
            set
            {
                if (value == _label)
                {
                    return;
                }

                _label = value;

                Repaint();
            }
        }

        /// <summary>
        /// Enables or disables the tab.
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled)
                {
                    return;
                }

                _enabled = value;

                Repaint();
            }
        }

        /// <summary>
        /// For repainting!
        /// </summary>
        public event Action OnRepaintRequested;

        /// <summary>
        /// Creates a new tab.
        /// </summary>
        public TabComponent()
        {
            Enabled = true;
        }

        /// <summary>
        /// Draws controls.
        /// </summary>
        public virtual void Draw()
        {
            //
        }

        /// <summary>
        /// Repaints controls.
        /// </summary>
        protected void Repaint()
        {
            OnRepaintRequested?.Invoke();
        }
    }
}