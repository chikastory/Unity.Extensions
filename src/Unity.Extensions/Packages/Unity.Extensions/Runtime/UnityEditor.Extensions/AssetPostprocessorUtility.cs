using System;

namespace UnityEditor {
    public static class AssetPostprocessorUtility {
        public static event Action<string[], string[], string[], string[]> OnPostprocessAllAssetsRaised;

        internal static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths) =>
            OnPostprocessAllAssetsRaised?.Invoke(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
    }
}
