using System;

namespace CreateAR.Commons.Unity.Editor
{
    public interface IEditorView
    {
        event Action OnRepaintRequested;

        void Draw();
    }
}