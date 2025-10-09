using UnityEngine;

[CreateAssetMenu(fileName = "ConfigsAsset", menuName = "Configs/ConfigsAsset")]
public class ConfigsAsset : ScriptableObject
{
    [TextArea(4, 20)]
    public string projectSettingsJson;

    [TextArea(4, 20)]
    public string externalJson;
}