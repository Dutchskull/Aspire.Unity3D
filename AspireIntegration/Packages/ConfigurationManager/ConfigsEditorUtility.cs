using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;

public static class ConfigsEditorUtility
{
    public static void ApplyMergedConfig(ConfigsAsset asset = null)
    {
        if (asset == null)
        {
            asset = AssetDatabase.LoadAssetAtPath<ConfigsAsset>(ProjectConfigSettingsProvider.k_AssetPath);
        }

        if (asset == null)
        {
            return;
        }

        string merged = MergeJson(asset.projectSettingsJson, asset.externalJson);
        Microsoft.Extensions.Configuration.IConfigurationRoot cfg = ConfigProvider.BuildFromJson(merged);
        ConfigProvider.ReplaceConfiguration(cfg);
    }

    private static string MergeJson(string baseJson, string overrideJson)
    {
        if (string.IsNullOrWhiteSpace(baseJson))
        {
            baseJson = "{}";
        }

        if (string.IsNullOrWhiteSpace(overrideJson))
        {
            return baseJson;
        }

        JObject baseJ = JObject.Parse(baseJson);
        JObject overJ = JObject.Parse(overrideJson);
        baseJ.Merge(overJ, new JsonMergeSettings
        {
            MergeArrayHandling = MergeArrayHandling.Replace,
            MergeNullValueHandling = MergeNullValueHandling.Merge
        });
        return baseJ.ToString();
    }
}