﻿using NeoServer.Game.Contracts;
using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Enums.Location.Structs;

namespace NeoServer.Server.Events.Creature
{
    public class CreatureWasBornEventHandler
    {
        private readonly IMap map;
        private readonly Game game;

        public CreatureWasBornEventHandler(IMap map, Game game)
        {
            this.map = map;
            this.game = game;
        }
        public void Execute(IMonster creature, Location location)
        {
            map.AddCreature(creature);
        }
    }
}
