﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Contracts.Spells;
using NeoServer.Game.Enums;
using NeoServer.Game.Enums.Creatures;
using NeoServer.Game.Enums.Location;
using NeoServer.Game.Enums.Location.Structs;
using NeoServer.Game.Enums.Players;
using NeoServer.Game.Enums.Talks;
using System.Collections.Generic;

namespace NeoServer.Server.Model.Players.Contracts
{
    public delegate void CancelWalk(IPlayer player);
    public delegate void ClosedContainer(IPlayer player, byte containerId);
    public delegate void OpenedContainer(IPlayer player, byte containerId, Game.Contracts.Items.Types.IContainer container);
    public delegate void ReduceMana(IPlayer player);
    public delegate void CannotUseSpell(IPlayer player, ISpell spell, InvalidOperation error);

    public interface IPlayer : ICombatActor
    {
        event UseSpell OnUsedSpell;

        ushort Level { get; }

        byte LevelPercent { get; }

        uint Experience { get; }

        byte AccessLevel { get; } // TODO: implement.

        byte SoulPoints { get; } // TODO: nobody likes soulpoints... figure out what to do with them :)

        float CarryStrength { get; }

        IDictionary<SkillType, ISkill> Skills { get; }

        bool CannotLogout { get; }
        ushort StaminaMinutes { get; }

        Location LocationInFront { get; }
        FightMode FightMode { get; }
        ChaseMode ChaseMode { get; }
        byte SecureMode { get; }

        bool InFight { get; }
        bool CanLogout { get; }
        IPlayerContainerList Containers { get; }

        event CancelWalk OnCancelledWalk;
        event CannotUseSpell OnCannotUseSpell;

        IInventory Inventory { get; }
        ushort Mana { get; }
        ushort MaxMana { get; }
        SkillType SkillInUse { get; }

        //  IAction PendingAction { get; }

        /// <summary>
        /// Changes player outfit
        /// </summary>
        /// <param name="outfit"></param>
        void ChangeOutfit(IOutfit outfit);

        uint ChooseToRemoveFromKnownSet();

        /// <summary>
        /// Checks if player knows creature with given id
        /// </summary>
        /// <param name="creatureId"></param>
        /// <returns></returns>
        bool KnowsCreatureWithId(uint creatureId);

        /// <summary>
        /// Get skill info
        /// </summary>
        /// <param name="skillType"></param>
        /// <returns></returns>
        byte GetSkillInfo(SkillType skillType);

        /// <summary>
        /// Changes player's fight mode
        /// </summary>
        /// <param name="fightMode"></param>
        void SetFightMode(FightMode fightMode);
        /// <summary>
        /// Changes player's chase mode
        /// </summary>
        /// <param name="chaseMode"></param>
        void SetChaseMode(ChaseMode chaseMode);
        /// <summary>
        /// Toogle Secure Mode 
        /// </summary>
        /// <param name="secureMode"></param>
        void SetSecureMode(byte secureMode);
        byte GetSkillPercent(SkillType type);

        void AddKnownCreature(uint creatureId);
        /// <summary>
        /// Sets where player is turned to
        /// </summary>
        /// <param name="direction"></param>
        void SetDirection(Direction direction);

        void ResetIdleTime();
        void CancelWalk();
        bool CanMoveThing(Location location);
        void ReceiveManaAttack(ICreature enemy, ushort damage);
        void Say(string message, TalkType talkType);
        bool HasEnoughMana(ushort mana);
        void ConsumeMana(ushort mana);
        bool HasEnoughLevel(ushort level);
    }
}
