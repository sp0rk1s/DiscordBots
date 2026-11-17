using AliBot.Utilities;
using Microsoft.Data.Sqlite;

namespace AliBot.Data;

public partial class Database {

    public string ConnectionString;
    public Dictionary<ulong, Player> LoadedPlayers;

    public Database(string path) {
        // Verify arguments
        ValidatePath(path);

        if (!File.Exists(path)) {
            Logging.Note($"Created Database at {path}");
        } else {
            Logging.Note($"Database at {path}");
        }

        // Setup Database
        ConnectionString = new SqliteConnectionStringBuilder().DataSource = path;
        using (SqliteConnection connection = new(path)) {
            connection.Open();
            foreach (KeyValuePair<string, string> tableQueryPair in MainDatabaseCreationQueries) {
                using SqliteCommand command = new(tableQueryPair.Value, connection);
                int changes = command.ExecuteNonQuery();
                if (changes > 0) {
                    Logging.Note($"Created \"{tableQueryPair.Key}\" Table in Database");
                }
            }
        }
    }

    private bool ValidatePath(string path) {
        if (string.IsNullOrWhiteSpace(path) ||
            Directory.Exists(path)
        ) {
            Logging.Error($"Could not find directory: \"{path}\"");
            return false;
        }
        if (Directory.Exists(path))
            return false;
        string? fileName = Path.GetFileName(path);
        if (string.IsNullOrEmpty(fileName))
            return false;
        string? directory = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            return false;

        return true;
    }
}