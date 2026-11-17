using System;
using System.Collections.Generic;
using System.Text;

namespace AliBot.Data {
    class Bank {
        private Dictionary<int, BankAccount> _accounts;
        public BankAccount this[int id] {
            get { return _accounts[id]; }
        }

        public Bank() {
            _accounts = new Dictionary<int, BankAccount>();
        }
    }
}
