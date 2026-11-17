using Microsoft.Data.Sqlite;

namespace AliBot.Data;
public class Database {

    private const string CREATE_TABLES_COMMAND = """
        
        CREATE TABLE IF NOT EXISTS "Users" (
            "userId"	INTEGER NOT NULL UNIQUE,
            PRIMARY KEY("userId") ON CONFLICT ABORT
        ) WITHOUT ROWID,STRICT;
        
    """;
    public string path;
    public string[] tables;

    public Database(string path) {
        this.path = path;
        tables = [];
        using (SqliteConnection connection = new(path)) {
            connection.Open();
            //DataTable tableSchema = connection.GetSchema("Tables");
            using (SqliteCommand command = new(CREATE_TABLES_COMMAND, connection)) {
                command.ExecuteNonQuery(); // Create new if the necessary tables are missing
            }
        }
    }
}