using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AliBot.Data;


public class PlayersChangedEventArgs(Player player) : EventArgs {
    public Player Player = player;
}

public class Players {

    public Player this[int index] {
        get { return Index(index); }
    }
    public int Length {
        get { return _length; }
    }

    public event EventHandler<PlayersChangedEventArgs> PlayerAdded;
    public event EventHandler<PlayersChangedEventArgs> PlayerRemoved;

    private readonly Database _database;
    private int _length;
    private ulong[] _playerIds;

    public Players(Database database, string input) {
        _database = database;
        _playerIds = [];
        string[] ids = input.Split(',');
        int index = 0;
        foreach (string id in ids) {
            if (ulong.TryParse(id, out ulong playerId)) {
                _playerIds[index++] = playerId;
            }
        }
        _length = _playerIds.Length;
        PlayerAdded = delegate { };
        PlayerRemoved = delegate { };
    }

    public Players Append(Player player) {
        int index = _playerIds.IndexOf(player.Id);
        if (index != -1) throw new InvalidOperationException();
        _playerIds[index] = player.Id;
        PlayerAdded.Invoke(this, new(player));
        return this;
    }

    public Players Remove(Player player) {
        int index = _playerIds.IndexOf(player.Id);
        if (index == -1) throw new IndexOutOfRangeException();
        _playerIds[index] = player.Id;
        PlayerRemoved.Invoke(this, new(player));
        return this;
    }

    public bool Contains(ulong playerId) {
        return _playerIds.Contains(playerId);
    }

    public IEnumerator<Player> GetEnumerator() {
        for (int i = 0; i < _length; i++) {
            yield return Index(i);
        }
    }

    public override string ToString() {
        string result = "";
        for (int i = 0; i < _length; i++) {
            result += _playerIds[i].ToString();
        }
        return result;
    }

    private Player Index(int index) {
        if (index >= _playerIds.Length || index < 0) {
            throw new IndexOutOfRangeException(nameof(index));
        }
        ulong id = _playerIds[index];
        if (_database.LoadedPlayers.TryGetValue(id, out Player? player)) {
            return player;
        } else {
            return new Player(_database, id);
        }
    }
}