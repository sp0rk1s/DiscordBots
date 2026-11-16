using Microsoft.Extensions.Configuration;
using System.CommandLine;
using AliBot.Databases;

namespace AliBot;

partial class Program {

    void Main(string[] args) {
        RootCommand rootCommand = GetRootCommand();
        rootCommand.SetAction(parseResult =>
        {
            string verbosityString = parseResult.GetValue(s_verbosityOption) ?? "normal";

            VerbosityLevel verbosity = verbosityString switch {
                "quiet" or "q" => VerbosityLevel.Quiet,
                "minimal" or "m" => VerbosityLevel.Minimal,
                "normal" or "n" => VerbosityLevel.Normal,
                "detailed" or "d" => VerbosityLevel.Detailed,
                "diagnostic" or "diag" => VerbosityLevel.Diagnostic,
                _ => VerbosityLevel.Normal
            };

            Console.WriteLine($"Verbosity level: {verbosity}");
        });
    }

    void Execute(string databaseDirectory, string configFilePath, string outputDirectory) {
        // Environment
        IConfiguration config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddIniFile(configFilePath, optional: false, reloadOnChange: true)
        .Build();

        Database database = new(databaseDirectory);

        Bot bot = new(database);

        var _ = Task.Run(async () => {
            try {
                await bot.Start();
            } catch (Exception exception) {
                // Log
            }
        });
    }
}



/*using Microsoft.Extensions.Configuration;

// Build the configuration
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddIniFile("config.ini", optional: false, reloadOnChange: true)
    .Build();

// Read values using "Section:Key"
string token = config["Discord:BotToken"];

Console.WriteLine($"Token: {token}");
*/