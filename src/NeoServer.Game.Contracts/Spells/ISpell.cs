﻿using NeoServer.Enums.Creatures.Enums;
using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Enums;
using NeoServer.Game.Enums.Creatures.Players;

namespace NeoServer.Game.Contracts.Spells
{
    public interface ISpell
    {
        EffectT Effect { get; }
        ConditionType ConditionType { get; }
        ushort Mana { get; set; }
        ushort MinLevel { get; set; }
        string Name { get; set; }
        uint Cooldown { get; set; }

        bool Invoke(ICombatActor actor, out InvalidOperation error);
    }
}
