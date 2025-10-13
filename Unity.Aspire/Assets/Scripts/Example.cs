using Microsoft.Extensions.Configuration;
using System.Text;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(DumpConfiguration(ConfigProvider.Configuration));
    }

    public static string DumpConfiguration(IConfiguration config)
    {
        var sb = new StringBuilder();
        void Dump(IConfiguration section, string path, int indent)
        {
            foreach (var child in section.GetChildren())
            {
                var keyPath = string.IsNullOrEmpty(path) ? child.Key : $"{path}:{child.Key}";
                var value = child.Value;
                sb.AppendLine(new string(' ', indent) + keyPath + (value == null ? "" : $" = {value}"));
                Dump(child, keyPath, indent + 2);
            }
        }
        Dump(config, "", 0);
        return sb.ToString();
    }
}
