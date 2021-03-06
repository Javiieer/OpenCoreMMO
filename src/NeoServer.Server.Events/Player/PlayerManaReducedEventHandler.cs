﻿using NeoServer.Networking.Packets.Outgoing;
using NeoServer.Server.Model.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Server.Events.Player
{
    public class PlayerManaReducedEventHandler
    {
        private readonly Game game;

        public PlayerManaReducedEventHandler(Game game)
        {
            this.game = game;
        }
        public void Execute(IPlayer player)
        {
            if (!game.CreatureManager.GetPlayerConnection(player.CreatureId, out var connection))
            {
                return;
            }
            connection.OutgoingPackets.Enqueue(new PlayerStatusPacket(player));
            connection.Send();

        }
    }
}
