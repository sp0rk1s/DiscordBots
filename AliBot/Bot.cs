using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using AliBot.Databases;

namespace AliBot;

class Bot {
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;
    public Bot(Database database) {
        _client = new(new DiscordSocketConfig {
            LogLevel = LogSeverity.Info,
        });

        _commands = new(new CommandServiceConfig {
            LogLevel = LogSeverity.Info,
            CaseSensitiveCommands = false,
        });

        _client.Log += Logging.Log;
        _commands.Log += Logging.Log;

        _services = ConfigureServices();
    }
    async Task Start() {
        await MainAsync();
    }

    private IServiceProvider ConfigureServices() {
        var map = new ServiceCollection()
            .AddSingleton(new SomeServiceClass());
        return map.BuildServiceProvider();
    }

    private async Task MainAsync() {
        await InitCommands();

        // Login and connect.
        await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken"));
        await _client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }

    private async Task InitCommands() {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        await _commands.AddModuleAsync<SomeModule>(_services);
        _client.MessageReceived += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage arg) {
        // Conditions
        var msg = arg as SocketUserMessage;
        if (msg == null) return;
        if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;

        int pos = 0;
        if (msg.HasCharPrefix('/', ref pos)) {
            var context = new SocketCommandContext(_client, msg);
            var result = await _commands.ExecuteAsync(context, pos, _services);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                await msg.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}