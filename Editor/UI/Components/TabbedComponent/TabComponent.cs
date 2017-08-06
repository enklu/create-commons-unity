using System;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Component for rendering tabs.
    /// </summary>
    public class TabComponent : IEditorView
    {
        /// <summary>
        /// Label of tab.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// For repainting!
        /// </summary>
        public event Action OnRepaintRequested;

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