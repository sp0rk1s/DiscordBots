using System;
using System.Collections.Generic;
using System.Text;

namespace AliBot.Data;

public class Player {

    public ulong Id;

    private Database _database;

    public Player(Database database, ulong id) {
        _database = database;
        Id = id;
    }
}