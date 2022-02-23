using Bolt;
using Bolt.Extend;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AutoBinary
{
    public static class ReplaceBinaryFileHelper
    {
        //[MenuItem("Tools/BinaryFlowMacro")]
        public static void BinaryFlowMacro()
        {
            var assetPath = "Assets";
            string[] macroGuids = AssetDatabase.FindAssets($"t:FlowMacro", new string[] { assetPath });
            foreach(var guid in macroGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var macro = AssetDatabase.LoadAssetAtPath<FlowMacro>(path);
                macro.OnAfterDeserialize();
                var directory = Path.GetDirectoryName(path);
                directory = directory.Replace(@"Assets",@"Assets\BinaryFile");
                var name = Path.GetFileNameWithoutExtension(path);
                var binaryPath = Path.Combine(directory, $"{name}.bytes");
                LevelGraphBinaryManager.Instance.SerializeGraph(macro.graph, binaryPath);
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/ReplaceFlowMachine")]
        public static void ReplaceSelectFlowMachine()
        {
            BinaryFlowMacro();

            if (Selection.gameObjects.Length > 0)
            {
                foreach (var go in Selection.gameObjects)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(go))
                    {
                        ReplaceFlowMachine(go);
                    }
                }
            }

            if(Selection.assetGUIDs.Length > 0)
            {
                List<string> guidList = new List<string>(Selection.assetGUIDs);
                for (int i = guidList.Count - 1; i > -1; i--)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guidList[i]);
                    if(AssetDatabase.IsValidFolder(path))
                    {
                        guidList.RemoveAt(i);
                        string[] prefabGuids = AssetDatabase.FindAssets($"t:Prefab", new string[] { path });
                        foreach (var prefabGuid in prefabGuids)
                        {
                            guidList.Add(prefabGuid);
                        }
                    }
                }

                foreach(var guid in guidList)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if(prefab == null)
                    {
                        continue;
                    }

                    var flowMachines = prefab.GetComponentsInChildren<FlowMachine>();
                    if(flowMachines.Length < 1)
                    {
                        continue;
                    }

                    ReplaceFlowMachine(prefab,true);
                }
            }
        }

        private static void ReplaceFlowMachine(GameObject go,bool isPrefab = false)
        {
            var flowMachines = go.transform.GetComponentsInChildren<FlowMachine>();
            foreach(var flowMachine in flowMachines)
            {
                if (flowMachine.m_Macro != null)
                {
                    var path = AssetDatabase.GetAssetPath(flowMachine.m_Macro);
                    var directory = Path.GetDirectoryName(path);
                    directory = directory.Replace(@"Assets", @"Assets\BinaryFile");
                    var name = Path.GetFileNameWithoutExtension(path);
                    var binaryPath = Path.Combine(directory, $"{name}.bytes");
                    if (!File.Exists(binaryPath))
                    {
                        Debug.LogError($"FlowMacro bytes isn't exist.path : {binaryPath}");
                        continue;
                    }

                    var binaryFile = AssetDatabase.LoadAssetAtPath<TextAsset>(binaryPath);
                    var custom = flowMachine.transform.GetOrAddComponent<CustomFlowMachine>();
                    custom.m_MacroBytes = binaryFile;
                    if (!isPrefab)
                    {
                        custom.LoadGraph();
                    }
                    GameObject.DestroyImmediate(flowMachine, true);
                }
            }
        }
    }
}
