
using System;
using UnityEditor.Callbacks;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.Assertions;

namespace FireSpreading.UserTools {
    public class InitializeTools {
        private static bool AssetActuallyExists(string assetPath) {
            var actualPath = $"{Application.dataPath}/{assetPath}";
            return System.IO.File.Exists(actualPath);
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
                Assert.IsNotNull(tool); 

                var path = $"UserTools/{tool.GetType().Name}.asset";

                if (AssetActuallyExists(path)){  
                    continue;
                }
                 
                Debug.Log($"[InitializeTools] Found a new tool, saving at {path}");

                AssetDatabase.CreateAsset(tool, $"Assets/{path}");
            }
        }
    }

}
