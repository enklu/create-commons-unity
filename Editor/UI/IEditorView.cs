using System;

namespace CreateAR.Commons.Unity.Editor
{
    /// <summary>
    /// Interface for any editor component.
    /// </summary>
    public interface IEditorView
    {
        /// <summary>
        /// Called when this view wishes to be repainted.
        /// </summary>
        event Action OnRepaintRequested;

        /// <summary>
        /// Draws the view.
        /// </summary>
        void Draw();
    }
}