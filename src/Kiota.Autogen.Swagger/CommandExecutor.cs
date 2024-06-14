using System.Diagnostics;

namespace Kiota.Autogen.Swagger;

public static class CommandExecutor
{
    public static bool Execute(string command, out string output)
    {
        var process = new Process
        {
            StartInfo = new("cmd", $"/c {command}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        try
        {
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            return true;
        }
        catch
        {
            output = process.StandardError.ReadToEnd();
            return false;
        }
    }
}