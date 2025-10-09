using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ProjectConfigSettingsProvider
{
    private const string k_Path = "Project/Config";
    public const string k_AssetPath = "Assets/Config/ConfigsAsset.asset";

    [SettingsProvider]
    public static SettingsProvider CreateProvider() =>
        new(k_Path, SettingsScope.Project)
        {
            label = "Config",
            guiHandler = (search) => { OnGUI(); },
            keywords = new HashSet<string> { "config", "settings", "json" }
        };

    private static void OnGUI()
    {
        ConfigsAsset asset = GetOrCreateAsset();
        if (asset == null)
        {
            EditorGUILayout.HelpBox("Could not create or load ConfigsAsset.", MessageType.Error);
            return;
        }

        EditorGUILayout.LabelField("Project Settings JSON", EditorStyles.boldLabel);
        asset.localEnvironment = EditorGUILayout.TextArea(asset.localEnvironment, GUILayout.Height(150));
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("External JSON (for testing)", EditorStyles.boldLabel);
        asset.externalEnvironment = EditorGUILayout.TextArea(asset.externalEnvironment, GUILayout.Height(150));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }
    }

    private static ConfigsAsset GetOrCreateAsset()
    {
        ConfigsAsset asset = AssetDatabase.LoadAssetAtPath<ConfigsAsset>(k_AssetPath);
        if (asset == null)
        {
            string dir = System.IO.Path.GetDirectoryName(k_AssetPath);
            if (!AssetDatabase.IsValidFolder(dir))
            {
                AssetDatabase.CreateFolder("Assets", "Config");
            }

            asset = ScriptableObject.CreateInstance<ConfigsAsset>();
            AssetDatabase.CreateAsset(asset, k_AssetPath);
            AssetDatabase.SaveAssets();
        }
        return asset;
    }
}