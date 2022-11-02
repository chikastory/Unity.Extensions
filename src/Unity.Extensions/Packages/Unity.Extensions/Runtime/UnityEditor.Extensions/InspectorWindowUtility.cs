using System.Reflection;

namespace UnityEditor {
    public static class InspectorWindowUtility {
        static readonly MethodInfo GetAllInspectorWindowsMethod = typeof(Editor).Assembly
            .GetType("UnityEditor.InspectorWindow").GetMethod("GetAllInspectorWindows",
                BindingFlags.Static | BindingFlags.NonPublic);

        public static EditorWindow[] GetAllInspectorWindows() =>
            (EditorWindow[])GetAllInspectorWindowsMethod.Invoke(null, null);
    }
}
