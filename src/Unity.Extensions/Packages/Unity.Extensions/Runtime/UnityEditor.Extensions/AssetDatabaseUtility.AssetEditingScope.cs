using System;

namespace UnityEditor {
    public static partial class AssetDatabaseUtility {
        public sealed class AssetEditingScope : IDisposable {
            bool _disposed;

            public AssetEditingScope() => AssetDatabase.StartAssetEditing();

            public void Dispose() {
                if (_disposed) return;
                AssetDatabase.StopAssetEditing();
                _disposed = true;
            }
        }
    }
}
