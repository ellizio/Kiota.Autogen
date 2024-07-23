using CliWrap;
using CliWrap.Buffered;

namespace Kiota.Autogen.Swagger;

public static class CommandExecutor
{
    public static bool Execute(string command, out string output)
    {
        var (target, args) = PrepareCommand(command);
        var result = Cli.Wrap(target)
            .WithArguments(args)
            .ExecuteBufferedAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        output = result.IsSuccess ? result.StandardOutput : result.StandardError;
        return result.IsSuccess;
    }

    private static (string target, string args) PrepareCommand(string command)
    {
        var delimiterIndex = command.IndexOf(" ");
        var target = command.Substring(0, delimiterIndex);
        var args = command.Substring(delimiterIndex + 1, command.Length - delimiterIndex - 1);

        return (target, args);
    }
}