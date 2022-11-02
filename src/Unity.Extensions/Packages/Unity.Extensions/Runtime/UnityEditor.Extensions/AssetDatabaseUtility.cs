using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.AssetDatabase;
using Object = UnityEngine.Object;

namespace UnityEditor {
    public static partial class AssetDatabaseUtility {
        [Pure]
        public static bool IsNotMainAsset([NotNull] Object source) => !IsMainAsset(source);

        [Pure]
        public static TObject FindAndLoadAsset<TObject>([NotNull] string keyword = "")
            where TObject : Object =>
            LoadAssetAtPath<TObject>(GUIDToAssetPath(FindAssets($"t:{typeof(TObject).Name} {keyword}").First()));

        [Pure]
        public static IEnumerable<TObject> FindAndLoadAssets<TObject>([NotNull] string keyword = "")
            where TObject : Object =>
            FindAssets($"t:{typeof(TObject).Name} {keyword}").Select(GUIDToAssetPath).Select(LoadAssetAtPath<TObject>);

        [Pure]
        public static IEnumerable<TObject> FindAndLoadMainAssets<TObject>([NotNull] string keyword = "")
            where TObject : Object =>
            FindAssets($"t:{typeof(TObject).Name} {keyword}").Select(GUIDToAssetPath).Select(LoadMainAssetAtPath)
                .OfType<TObject>();

        [Pure]
        public static IEnumerable<TObject> LoadSubAssets<TObject>([NotNull] Object source) where TObject : Object {
            Assert.IsTrue(IsMainAsset(source));
            return LoadAllAssetsAtPath(GetAssetPath(source)).Except(new[] { source }).OfType<TObject>();
        }

        public static TScriptableObject AddSubAsset<TScriptableObject>([NotNull] Object source)
            where TScriptableObject : ScriptableObject {
            Assert.IsTrue(IsMainAsset(source));
            using var scope = new UndoUtility.CollapseGroupScope($"Add {ObjectNames.NicifyVariableName(typeof(TScriptableObject).Name)}");
            var instance = ScriptableObject.CreateInstance<TScriptableObject>();
            Undo.RegisterCreatedObjectUndo(instance, "Create Instance");
            Undo.RecordObjects(new [] { source, instance }, "Add Object To Asset");
            AddObjectToAsset(instance, source);
            return instance;
        }

        public static void AddSubAsset<TObject>([NotNull] Object source, [NotNull] TObject target)
            where TObject : Object {
            Assert.IsTrue(IsMainAsset(source));
            Assert.IsFalse(LoadSubAssets<TObject>(source).Contains(target));
            using var scope = new UndoUtility.CollapseGroupScope($"Add {target.name}");
            Undo.RecordObjects(new [] { source, target }, "Add Object To Asset");
            AddObjectToAsset(target, source);
        }

        public static void RemoveSubAsset<TObject>([NotNull] Object source, [NotNull] TObject target)
            where TObject : Object {
            Assert.IsTrue(IsMainAsset(source));
            Assert.IsTrue(IsNotMainAsset(target));
            Assert.IsTrue(LoadSubAssets<TObject>(source).Contains(target));
            using var scope = new UndoUtility.CollapseGroupScope($"Remove {target.name}");
            Undo.RecordObjects(new [] { source, target }, $"Remove Sub Asset");
            Undo.DestroyObjectImmediate(target);
        }

        public static void RemoveSubAssets<TObject>([NotNull] Object source, [NotNull] IEnumerable<TObject> targets)
            where TObject : Object {
            Assert.IsTrue(IsMainAsset(source));
            var targetArray = targets as TObject[] ?? targets.ToArray();
            Assert.IsTrue(targetArray.All(IsNotMainAsset));
            Assert.IsFalse(targetArray.Except(LoadSubAssets<TObject>(source)).Any());
            using var scope = new UndoUtility.CollapseGroupScope($"Remove {targetArray.Length} Sub Assets");
            Undo.RecordObjects(targetArray.Concat(new [] { source }).ToArray(), $"Remove Sub Assets");
            foreach (var target in targetArray) Undo.DestroyObjectImmediate(target);
        }
    }
}
