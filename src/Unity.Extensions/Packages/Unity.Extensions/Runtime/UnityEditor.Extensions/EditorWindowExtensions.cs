using System.Reflection;

namespace UnityEditor {
    public static class EditorWindowExtensions {
        static readonly PropertyInfo TrackerProperty = typeof(Editor).Assembly
            .GetType("UnityEditor.IPropertyView").GetProperty("tracker");

        public static ActiveEditorTracker GetTracker(this EditorWindow source) =>
            (ActiveEditorTracker)TrackerProperty.GetValue(source);
    }
}
