using System;
using System.IO;
using UnityEngine;

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
            File.WriteAllText(Application.persistentDataPath + "/aspire.json", argument);
        }
        catch (Exception ex)
        {
            return "error:invalid_json:" + ex.Message;
        }

        return "ok";
    }
}