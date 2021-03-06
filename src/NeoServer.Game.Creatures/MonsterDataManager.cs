using NeoServer.Game.Contracts.Creatures;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NeoServer.Game.Creature
{
    public class MonsterDataManager : IMonsterDataManager
    {
        private IImmutableDictionary<string, IMonsterType> _monsters;
        private bool _loaded = false;

        public bool TryGetMonster(string name, out IMonsterType monster) => _monsters.TryGetValue(name, out monster);

        public void Load(IEnumerable<IMonsterType> monsters)
        {
            if (_loaded == true)
            {
                throw new InvalidOperationException("Monsters already loaded");
            }

            _monsters = monsters.ToImmutableDictionary(x => x.Name);
            _loaded = true;
        }
    }
}