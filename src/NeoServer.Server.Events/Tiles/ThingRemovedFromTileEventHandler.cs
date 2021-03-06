﻿using NeoServer.Enums.Creatures.Enums;
using NeoServer.Game.Contracts;
using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Contracts.Items;
using NeoServer.Game.Contracts.Items.Types;
using NeoServer.Game.Contracts.World;
using NeoServer.Game.World.Map;
using NeoServer.Networking.Packets.Outgoing;
using NeoServer.Server.Contracts.Network;
using NeoServer.Server.Model.Players.Contracts;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NeoServer.Server.Events
{
    public class ThingRemovedFromTileEventHandler
    {
        private readonly Game game;

        public ThingRemovedFromTileEventHandler(IMap map, Game game)
        {   
            this.game = game;
        }
        public void Execute(IThing thing, ICylinder cylinder)
        {

            cylinder.ThrowIfNull();
            cylinder.TileSpectators.ThrowIfNull();
            thing.ThrowIfNull();

            var tile = cylinder.FromTile;
            tile.ThrowIfNull();

            foreach (var spectator in cylinder.TileSpectators)
            {
                var creature = spectator.Value.Spectator;

                if (creature is IMonster monster && thing is IPlayer target)
                {
                    monster.RemoveFromTargetList(target);
                    continue;
                }

                if (!game.CreatureManager.GetPlayerConnection(creature.CreatureId, out var connection))
                {
                    continue;
                }

                if(!(creature is IPlayer player))
                {
                    continue;
                }

                if (player.IsDead)
                {
                    continue;
                }

                var stackPosition = spectator.Value.FromStackPosition;

                connection.OutgoingPackets.Enqueue(new MagicEffectPacket(tile.Location, EffectT.Puff));

                if (thing is ICumulativeItem cumulative && cumulative.Amount > 0)
                {
                    connection.OutgoingPackets.Enqueue(new UpdateTileItemPacket(tile.Location, stackPosition, cumulative));
                }
                else
                {
                    connection.OutgoingPackets.Enqueue(new RemoveTileThingPacket(tile, stackPosition));
                }

                connection.Send();
            }

        }
    }
}
