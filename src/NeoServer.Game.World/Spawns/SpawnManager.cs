﻿using NeoServer.Game.Contracts;
using NeoServer.Game.Contracts.Creatures;
using System;
using System.Linq;

namespace NeoServer.Game.World.Spawns
{
    public class SpawnManager
    {

        private readonly World _world;
        private readonly IMap _map;
        private readonly ICreatureFactory _creatureFactory;
        private readonly ICreatureGameInstance _creatureGameInstance;
        public SpawnManager(World world, IMap map, ICreatureGameInstance creatureGameInstance, ICreatureFactory creatureFactory)
        {
            _world = world;

            _map = map;
            _creatureGameInstance = creatureGameInstance;
            _creatureFactory = creatureFactory;
        }

        public void Respawn()
        {
            foreach (var respawn in _creatureGameInstance.AllKilledMonsters())
            {
                var monster = respawn.Item1;
                var deathTime = respawn.Item2;

                var spawnTime = TimeSpan.FromSeconds(monster.Spawn.SpawnTime);

                if (DateTime.Now.TimeOfDay < deathTime + spawnTime)
                {
                    continue;
                }

                if (_map.ArePlayersAround(monster.Location))
                {
                    continue;
                }
                if (_creatureGameInstance.TryRemoveFromKilledMonsters(monster.CreatureId))
                {
                    monster.Reborn();
                }
            }
        }

        public void StartSpawn()
        {
            foreach (var monsterToSpawn in _world.Spawns.SelectMany(x => x.Monsters))
            {
                var monster = _creatureFactory.CreateMonster(monsterToSpawn.Name, monsterToSpawn.Spawn);
                monster.SetNewLocation(monsterToSpawn.Spawn.Location);
                _map.AddCreature(monster);

                _creatureGameInstance.Add(monster);
            }
        }
    }
}
