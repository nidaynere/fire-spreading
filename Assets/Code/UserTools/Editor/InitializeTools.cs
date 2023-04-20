
using System;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Linq;
using UnityEditor;
using NUnit.Framework;

namespace FireSpreading.UserTools {
    public class InitializeTools {
        private static bool AssetActuallyExists(string assetPath) {
            return !(AssetDatabase.AssetPathToGUID(assetPath) == string.Empty ||
                AssetDatabase.GetMainAssetTypeAtPath(assetPath) != null);
        }

        [DidReloadScripts]
        public static void OnCompile() {
            var currentToolTypes = AppDomain.CurrentDomain.GetAssemblies() 
                                .SelectMany(assembly => assembly.GetTypes())
                                .Where(type => 
                                type.IsSubclassOf(typeof(ScriptableTool)) && !type.IsAbstract).
                                ToArray ();  

            var currentTools = currentToolTypes.Select(x => ScriptableObject.CreateInstance(x));

            if (!AssetDatabase.IsValidFolder("Assets/UserTools")) {
                AssetDatabase.CreateFolder("Assets", "UserTools"); 
            }

            foreach (var tool in currentTools) {
                Assert.NotNull(tool); 

                var path = $"Assets/UserTools/{tool.GetType().Name}.asset";

                if (AssetActuallyExists(path)){
                    continue;
                }

                AssetDatabase.CreateAsset(tool, path);
            }
        }
    }

}
