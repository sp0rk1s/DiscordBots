using System;
using System.Collections.Generic;
using System.Text;

namespace AliBot.Data;

public class Stack {
    public readonly Player Player;
    public readonly Item Item;
    public int Amount {
        get { return _amount; }
        set { UpdateAmount(value); }
    }

    private int _amount;
    private Database _database;

    public Stack(Database database, Player player, Item item, int slot) {
        Player = player;
        Item = item;
        player[slot] = this;
    }

    public void Delete() {
        _amount = 0;
        // TODO
    }

    private void UpdateAmount(int amount) {
        if (amount == _amount)
            return;

    }
}
