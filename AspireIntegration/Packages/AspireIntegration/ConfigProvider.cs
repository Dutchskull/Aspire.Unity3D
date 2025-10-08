using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class ConfigsLoader
{
    public static IConfigurationRoot BuildMergedConfig(ConfigsAsset asset, string[] commandLineArgs = null)
    {
        if (asset == null)
        {
            throw new ArgumentNullException(nameof(asset));
        }

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJson(asset.localEnvironment)
            .AddJson(asset.externalEnvironment);

        if (commandLineArgs != null && commandLineArgs.Length > 0)
        {
            builder.AddCommandLine(commandLineArgs);
        }

        return builder.Build();
    }

    public static IConfigurationBuilder AddJson(this IConfigurationBuilder builder, string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return builder;
        }

        return builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)));
    }

    public static void LoadAndApply(ConfigsAsset asset, string[] commandLineArgs = null)
    {
        IConfigurationRoot merged = BuildMergedConfig(asset, commandLineArgs);
        ConfigProvider.ReplaceConfiguration(merged);
    }
}

public static class ConfigProvider
{
    private static readonly object _lock = new();
    private static IConfigurationRoot _configuration;

    public static event Action<IConfigurationRoot> OnConfigurationReloaded;

    public static IConfigurationRoot Configuration
    {
        get
        {
            lock (_lock)
            {
                return _configuration;
            }
        }
    }

    public static IConfigurationRoot BuildFromJson(string json)
    {
        if (json == null)
        {
            throw new ArgumentNullException(nameof(json));
        }

        ConfigurationBuilder builder = new();
        using MemoryStream ms = new(Encoding.UTF8.GetBytes(json));
        builder.AddJsonStream(ms);
        return builder.Build();
    }

    public static void ReplaceConfiguration(IConfigurationRoot newConfiguration)
    {
        if (newConfiguration == null)
        {
            throw new ArgumentNullException(nameof(newConfiguration));
        }

        lock (_lock)
        {
            _configuration = newConfiguration;
        }

        OnConfigurationReloaded?.Invoke(newConfiguration);
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
internal static class ConfigsEditorInitializer
{
    static ConfigsEditorInitializer()
    {
        BuildFromAsset();
        EditorApplication.projectChanged += BuildFromAsset;
    }

    private static void BuildFromAsset()
    {
        ConfigsAsset asset = AssetDatabase.LoadAssetAtPath<ConfigsAsset>(ProjectConfigSettingsProvider.k_AssetPath);

        if (asset == null && !File.Exists(ProjectConfigSettingsProvider.k_AssetPath))
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ConfigsAsset>(), ProjectConfigSettingsProvider.k_AssetPath);
        }

        ConfigsLoader.LoadAndApply(asset, null);
        Debug.Log("ConfigProvider: merged configuration built from ConfigsAsset (Editor).");
    }
}
#endif