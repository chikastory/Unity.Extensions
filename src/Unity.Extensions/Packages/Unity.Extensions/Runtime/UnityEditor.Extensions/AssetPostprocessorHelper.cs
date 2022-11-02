namespace UnityEditor {
    sealed class AssetPostprocessorHelper : AssetPostprocessor {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths) =>
            AssetPostprocessorUtility.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets,
                movedFromAssetPaths);
    }
}
