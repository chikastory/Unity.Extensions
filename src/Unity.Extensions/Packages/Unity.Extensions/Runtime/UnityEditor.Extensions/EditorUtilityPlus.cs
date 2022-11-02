using System.Linq;

namespace UnityEditor {
    public static class EditorUtilityPlus {
        public static void ForceRebuild(Editor editor) {
            foreach (var tracker in InspectorWindowUtility.GetAllInspectorWindows().Select(x => x.GetTracker()))
                if (tracker.activeEditors.Any(x => x == editor)) {
                    tracker.ForceRebuild();
                    return;
                }
        }
    }
}
