using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliBot.Data;

class UserInfo {
    public ulong Id;
    public IUser User;
    public int Balance;

    public UserInfo(ulong Id) {

    }

    public UserInfo(IUser User) {

    }
}
