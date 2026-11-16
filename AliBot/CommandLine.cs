using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace AliBot;

partial class Program {

    enum VerbosityLevel {
        Quiet,
        Minimal,
        Normal,
        Detailed,
        Diagnostic
    }

    private static readonly Option<FileInfo?> s_databasePathOption = new("--config", "-c") {
        Description = "Specify database folder path (default: database)",
        HelpName = "FOLDER"
    };

    private static readonly Option<FileInfo?> s_outputPath = new("--config", "-c") {
        Description = "Specify output logs file path (default: logs/)",
        HelpName = "FOLDER"
    };

    private static readonly Option<FileInfo?> s_configPathOption = new("--config", "-c") {
        Description = "Specify configuration file path (default: config.ini)",
        HelpName = "FILE"
    };

    private static readonly Option<string> s_verbosityOption = new("--verbosity", "-v") {
        Description = "Output verbosity level. Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].",
        HelpName = "LEVEL"
    };

    static RootCommand GetRootCommand() {

        s_verbosityOption.Validators.Add(result => {
            if (result.Tokens.Count == 0) return;
            string value = result.Tokens.Single().Value.ToLowerInvariant();
            string[] validValues = { "quiet", "q", "minimal", "m", "normal", "n", "detailed", "d", "diagnostic", "diag" };
            if (!validValues.Contains(value)) {
                result.AddError($"Argument '{value}' not recognized. Must be one of: 'q[uiet]', 'm[inimal]', 'n[ormal]', 'd[etailed]', 'diag[nostic]'");
            }
        });

        RootCommand rootCommand = new("Run the discord bot with DATABASE_FOLDER.") {
            s_databasePathOption,
            s_outputPath,
            s_configPathOption,
            s_verbosityOption,
        };

        return rootCommand;
    }
}