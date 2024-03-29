﻿using System.Reflection;
using BSXIV.Utilities;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace BSXIV
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private InteractionService _interaction;
        private IServiceProvider _services;
        private Logger _logger;
        
        public CommandHandler(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();
            _interaction = services.GetRequiredService<InteractionService>();
            _services = services;

            _logger = services.GetRequiredService<Logger>();

            _client.SlashCommandExecuted += SlashCommand;
        }

        private async Task SlashCommand(SocketSlashCommand args)
        {
            var result = await _interaction.ExecuteCommandAsync(
                new SocketInteractionContext<SocketSlashCommand>(_client, args),
                _services);

            if (result.Error != null)
            { 
                _logger.Error(result.ErrorReason);
            }
        }

        public async Task InitializeAsync()
        {
            var modules = await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            foreach (var servers in _client.Guilds)
            {
                if (modules.ToArray() == null)
                {
                    _logger.Error("'modules.ToString()' is null! Commands won't show up!");
                }
                await _interaction.AddModulesToGuildAsync(servers, true, modules.ToArray());
            }
        }
    }
}