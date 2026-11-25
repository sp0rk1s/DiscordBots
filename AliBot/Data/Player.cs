using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AliBot.Data;

public partial class Player {

	public readonly ulong Id;
	public readonly long SignedId;
	public string Name {
		get { return _name; }
		set { UpdateName(value); }
	}
	public DateTime Birthday {
		get { return _birthday; }
		set { UpdateBirthday(value); }
	}
	public int Wallet {
		get { return _wallet; }
		set { UpdateWallet(value); }
	}
	public DateTime LastJob { get { return _lastJob; } }
	public Stack this[int slot] {
		get { return _inventory[slot]; }
		set { UpdateInventory(slot, value); }
	}

	private string _name;
	private DateTime _birthday;
	private int _wallet;
	private DateTime _lastJob;
	private Dictionary<int, Stack> _inventory;
	private readonly Database _database;

	public Player(Database database, ulong id) {
		SignedId = BitConverter.ToInt64(BitConverter.GetBytes(id));

		// Queries
		string queryInsert = """
			INSERT OR IGNORE INTO players (id) VALUES (@id);
		""";
		string querySelect = """
			SELECT * FROM players WHERE id = @id;
		""";

		// Connection
		using SqliteConnection connection = new(database.ConnectionString);
		connection.Open();

		// Query
		bool playerExists = false;
		using (SqliteCommand command = new(querySelect, connection)) {
			command.Parameters.AddWithValue("@id", SignedId);
			SqliteDataReader reader = command.ExecuteReader();
			if (reader.HasRows) {
				Id = id;
				_name = reader.GetString(1);
				_birthday = DateTime.ParseExact(reader.GetString(2), "yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
				_wallet = reader.GetInt32(3);
				_lastJob = reader.GetDateTime(4);
				playerExists = true;
			}
		}

		if (!playerExists) {
			using SqliteCommand command = new(queryInsert, connection);
			command.Parameters.AddWithValue("@id", SignedId);
			command.ExecuteNonQuery();
		}
	}

	public bool Work(Job job) {
		return true;
	}

    public Player Append(Stack stack) {
        return this;
    }

	private bool UpdateName(string name) {
		if (IsValidName(name))
			return false;
		else if (name == Name)
			return false;
		string query = "UPDATE players SET name = @name WHERE id = @id";
		using SqliteConnection connection = new(_database.ConnectionString);
		connection.Open();
		// Query
		using SqliteCommand command = new(query, connection);
		command.Parameters.AddWithValue("@name", name);
		command.Parameters.AddWithValue("@id", SignedId);
		int changes = command.ExecuteNonQuery();
		return changes != 0;
	}

	private bool UpdateBirthday(DateTime date) {
		string formattedDate = date.ToString("yyyy/MM/dd HH:mm:ss");
		string query = "UPDATE players SET birthday = @birthday WHERE id = @id";
		using SqliteConnection connection = new(_database.ConnectionString);
		connection.Open();
		// Query
		using SqliteCommand command = new(query, connection);
		command.Parameters.AddWithValue("@birthday", formattedDate);
		command.Parameters.AddWithValue("@id", SignedId);
		int changes = command.ExecuteNonQuery();
		return changes != 0;
	}

	private void UpdateWallet(int amount) {
		if (amount == 0)
			return;
		string query = "UPDATE players SET wallet = @amount WHERE id = @id";
		using SqliteConnection connection = new(_database.ConnectionString);
		connection.Open();
		// Query
		using SqliteCommand command = new(query, connection);
		command.Parameters.AddWithValue("@amount", amount);
		command.Parameters.AddWithValue("@id", SignedId);
		int changes = command.ExecuteNonQuery();
        if (changes == 0)
            throw new Exception();
	}
	
	private void UpdateInventory(int slot, Stack stack) {
        if (slot is < 0 or > 63)
            throw new IndexOutOfRangeException();
        if (_inventory.TryGetValue(slot, out Stack? old)) {
            old.Delete();
        }
        _inventory[slot] = stack;
	}

	public static bool IsValidName(string name) {
		string[] subs = name.Split(" .");
		foreach (string sub in subs) {
			if (!NameRegex().IsMatch(sub)) {
				return false;
			}
		}
		return true;
	}

	[GeneratedRegex(@"^\p{Lu}\p{Ll}*$")]
	private static partial Regex NameRegex();
}