using System;

namespace UnityEditor {
    public static partial class UndoUtility {
        public sealed class CollapseGroupScope : IDisposable {
            readonly int _group;
            readonly string _name;

            public CollapseGroupScope(string name) {
                _group = Undo.GetCurrentGroup();
                _name = name;
            }

            public void Dispose() {
                Undo.SetCurrentGroupName(_name);
                Undo.CollapseUndoOperations(_group);
            }
        }
    }
}
