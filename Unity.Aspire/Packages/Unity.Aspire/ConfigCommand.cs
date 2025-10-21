using System;

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
            ConfigSourceRegistry.RegisterSource(builder => builder.AddJson(argument), "Aspire");
        }
        catch (Exception ex)
        {
            return "error:invalid_json:" + ex.Message;
        }

        return "ok";
    }
}