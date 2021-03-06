﻿using NeoServer.Game.Contracts;
using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Enums.Location;
using NeoServer.Networking.Packets.Outgoing;
using NeoServer.Server.Contracts.Network;
using NeoServer.Server.Model.Players.Contracts;
using System;
using System.Collections.Generic;

namespace NeoServer.Server.Events
{
    public class CreatureTurnedToDirectionEventHandler
    {
        private readonly IMap map;
        private readonly Game game;

        public CreatureTurnedToDirectionEventHandler(IMap map, Game game)
        {
            this.map = map;
            this.game = game;
        }
        public void Execute(IWalkableCreature creature, Direction direction)
        {
            creature.ThrowIfNull();
            direction.ThrowIfNull();

            foreach (var spectatorId in map.GetPlayersAtPositionZone(creature.Location))
            {

                if (!game.CreatureManager.GetPlayerConnection(spectatorId, out var connection))
                {
                    continue;
                }

                if(!game.CreatureManager.TryGetPlayer(spectatorId, out var player))
                {
                    continue;
                }

                if(!creature.Tile.TryGetStackPositionOfThing(player,creature, out var stackPosition))
                {
                    continue;
                }

                connection.OutgoingPackets.Enqueue(new TurnToDirectionPacket(creature, direction, stackPosition));

                connection.Send();

            }

        }
    }
}
