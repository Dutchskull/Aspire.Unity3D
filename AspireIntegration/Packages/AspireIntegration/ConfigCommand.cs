using System;
using UnityEditor;

internal class ConfigCommand : ICommand
{
    public string Execute(string argument)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            return "error:empty_body";
        }

        try
        {
            ConfigsAsset asset = AssetDatabase.LoadAssetAtPath<ConfigsAsset>(ProjectConfigSettingsProvider.k_AssetPath);
            asset.externalEnvironment = argument;
        }
        catch (Exception ex)
        {
            return "error:invalid_json:" + ex.Message;
        }

        return "ok";
    }
}