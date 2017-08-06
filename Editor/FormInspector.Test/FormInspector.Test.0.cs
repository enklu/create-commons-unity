using UnityEditor;

namespace CreateAR.Commons.Unity.Editor.Test
{
    public enum TestEnum
    {
        Value0,
        Value1,
        Value2,
        Value3,
        Value4
    }

    public class BagOfData
    {
        public byte Byte;
        public short Short;
        public int Int;
        public long Long;

        public float Float;
        public double Double;

        public bool Bool;
        public string String;
        public TestEnum Enum;

        // TODO: Composite

        // TODO: Cycles
    }

    public class FormInspector_Test_0 : EditorWindow
    {
        private readonly FormInspector _form = new FormInspector(FormInspector.RenderOptions.Alphabetize);

        [MenuItem("Test/FormInspector/Test.0")]
        private static void Open()
        {
            GetWindow<FormInspector_Test_0>();
        }

        private void OnEnable()
        {
            _form.Value = new BagOfData();
        }

        private void OnDisable()
        {
            _form.Value = null;
        }

        private void OnGUI()
        {
            _form.Draw();
        }
    }
}
