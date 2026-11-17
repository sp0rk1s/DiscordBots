using System;
using System.Collections.Generic;
using System.Text;

namespace AliBot.Data;

class Bank {
    private readonly Dictionary<int, BankAccount> _accounts;
    public BankAccount this[int id] {
        get { return _accounts[id]; }
    }

    public Bank(Database database) {
        _accounts = [];
    }

   
}