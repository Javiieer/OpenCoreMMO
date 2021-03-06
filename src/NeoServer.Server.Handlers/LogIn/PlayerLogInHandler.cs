﻿using NeoServer.Networking.Packets.Incoming;
using NeoServer.Networking.Packets.Outgoing;
using NeoServer.Server.Commands;
using NeoServer.Server.Contracts.Network;
using NeoServer.Server.Contracts.Repositories;
using NeoServer.Server.Tasks;

namespace NeoServer.Server.Handlers.Authentication
{
    public class PlayerLogInHandler : PacketHandler
    {
        private readonly IAccountRepository repository;

        private readonly Game game;

        public PlayerLogInHandler(IAccountRepository repository,
         Game game)
        {
            this.repository = repository;
            this.game = game;
        }

        public async override void HandlerMessage(IReadOnlyNetworkMessage message, IConnection connection)
        {
            if (game.State == GameState.Stopped)
            {
                connection.Close();
            }

            var packet = new PlayerLogInPacket(message);

            connection.SetXtea(packet.Xtea);

            //todo linux os

            Verify(connection, packet);

            //todo: ip ban validation

            var accountRecord = await repository.FirstOrDefaultAsync(a => a.AccountName == packet.Account && a.Password == packet.Password);

            if (accountRecord == null)
            {
                connection.Send(new GameServerDisconnectPacket($"Account name or password is not correct."));
                return;
            }

            game.Dispatcher.AddEvent(new Event(new PlayerLogInCommand(accountRecord, packet.CharacterName, game, connection).Execute));

        }

        private void Verify(IConnection connection, PlayerLogInPacket packet)
        {

            if (string.IsNullOrWhiteSpace(packet.Account))
            {
                connection.Send(new GameServerDisconnectPacket("You must enter your account name."));
                return;
            }

            if (ServerConfiguration.Version != packet.Version)
            {
                connection.Send(new GameServerDisconnectPacket($"Only clients with protocol {ServerConfiguration.Version} allowed!"));
                return;
            }

            if (game.State == GameState.Opening)
            {
                connection.Send(new GameServerDisconnectPacket($"Gameworld is starting up. Please wait."));
                return;
            }
            if (game.State == GameState.Maintaining)
            {
                connection.Send(new GameServerDisconnectPacket($"Gameworld is under maintenance. Please re-connect in a while."));
                return;
            }

            if (game.State == GameState.Closed)
            {
                connection.Send(new GameServerDisconnectPacket("Server is currently closed.\nPlease try again later."));
                return;
            }
        }
    }
}
