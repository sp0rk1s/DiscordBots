namespace AliBot.Data {
	public partial class Database {

        public Dictionary<string, string> MainDatabaseCreationQueries = new() {
            ["Players"] = CreatePlayersTableQuery,
            ["BankAccounts"] = CreateBankAccountsTableQuery,
            ["Skills"] = CreateSkillsTableQuery,
            ["PlayerSkills"] = CreatePlayerSkillsTableQuery,
            ["ItemTableQuery"] = CreateItemTableQuery,
            ["Inventories"] = CreatePlayerInventoryTableQuery,
            ["Jobs"] = CreateJobsTableQuery
        };

		private const string CreatePlayersTableQuery = """
			CREATE TABLE IF NOT EXISTS "players" (
				"id"	        INTEGER NOT NULL UNIQUE,
				"name"          STRING,
				"birthday"      TEXT,
				"wallet"	    INTEGER NOT NULL DEFAULT 0 CHECK (wallet >= 0),
				"last_job"	    TEXT NOT NULL DEFAULT '2000/01/01 00:00:00',
				PRIMARY KEY("id") ON CONFLICT ABORT
			) WITHOUT ROWID,STRICT;
		""";

		private const string CreateBankAccountsTableQuery = """
			CREATE TABLE IF NOT EXISTS "bank_accounts" (
				"id"	    INTEGER NOT NULL UNIQUE,
				"name"      TEXT NOT NULL,
				"balance"   INTEGER NOT NULL,
				"admins"    TEXT NOT NULL,
				"access"    TEXT NOT NULL,
				PRIMARY KEY("id") ON CONFLICT ABORT
			) WITHOUT ROWID,STRICT;
		""";

		private const string CreateSkillsTableQuery = """
			CREATE TABLE IF NOT EXISTS "skills" (
				"id"	        TEXT NOT NULL UNIQUE,
				"name"	        TEXT NOT NULL,
				"description"	TEXT,
				"difficulty"	INTEGER NOT NULL DEFAULT 1 CHECK (skill_points >= 1),
				PRIMARY KEY("id") ON CONFLICT ABORT
			) WITHOUT ROWID,STRICT;
		""";

		private const string CreatePlayerSkillsTableQuery = """
			CREATE TABLE IF NOT EXISTS "player_skills" (
				player_id           INTEGER NOT NULL,
				skill_id            TEXT NOT NULL,
				level               INTEGER NOT NULL DEFAULT 0 CHECK (level BETWEEN 0 AND 5),
				points              INTEGER NOT NULL DEFAULT 0 CHECK (points >= 0),
				PRIMARY KEY (player_id, skill_id),
				FOREIGN KEY (player_id) REFERENCES players(player_id) ON DELETE CASCADE,
				FOREIGN KEY (skill_id) REFERENCES skills(skill_id)
			) WITHOUT ROWID,STRICT;
		""";

		private const string CreateItemTableQuery = """
			CREATE TABLE IF NOT EXISTS "items" (
				id                  TEXT NOT NULL UNIQUE,
				name                TEXT NOT NULL,
				description         TEXT,
				value               INTEGER CHECK (value >= 0),
				price               INTEGER CHECK (price >= 0),
				PRIMARY KEY (id)
			) WITHOUT ROWID,STRICT;
		""";

		private const string CreatePlayerInventoryTableQuery = """
			CREATE TABLE IF NOT EXISTS "stacks" (
				player_id           INTEGER NOT NULL,
				item_id             TEXT NOT NULL,
				quantity            INTEGER NOT NULL DEFAULT 1 CHECK (quantity > 0),
				PRIMARY KEY (player_id, item_id),
				FOREIGN KEY (player_id) REFERENCES players(player_id) ON DELETE CASCADE,
				FOREIGN KEY (item_id) REFERENCES items(item_id)
			) WITHOUT ROWID,STRICT;
		""";

		private const string CreateJobsTableQuery = """
			CREATE TABLE IF NOT EXISTS "jobs" (
				id                  TEXT NOT NULL UNIQUE,
				name                TEXT NOT NULL,
				description         TEXT,
				payment             INTEGER NOT NULL CHECK (payment >= 0),
				duration            INTEGER NOT NULL CHECK (duration >= 0),
				cooldown            INTEGER NOT NULL CHECK (cooldown >= 0),
				PRIMARY KEY (id)
			) WITHOUT ROWID,STRICT;
		""";
	}
}