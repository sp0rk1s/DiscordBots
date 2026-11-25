using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AliBot.Data;

public class Item {
    public readonly string Id;
    public readonly string Name;
    public readonly string Description;
    public readonly int Value;
    public readonly int Price;

    private Database _database;

    public Item(Database database) {
        _database = database;


    }
}