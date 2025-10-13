using System;
using UnityEngine;

public class ConfigsRuntimeInitializer : MonoBehaviour
{
    public ConfigsAsset configsAsset;
    public string resourceName = "ConfigAsset";

    private void Awake()
    {
        ConfigsAsset asset = configsAsset != null ? configsAsset : Resources.Load<ConfigsAsset>(resourceName);
        if (asset == null)
        {
            Debug.LogError("ConfigsRuntimeInitializer: no ConfigsAsset found. This should not happen.");
            return;
        }

        ConfigsLoader.LoadAndApply(asset, Environment.GetCommandLineArgs());
    }
}
