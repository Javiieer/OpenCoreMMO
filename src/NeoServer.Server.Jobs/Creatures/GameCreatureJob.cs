﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.World.Spawns;
using NeoServer.Server.Model.Players.Contracts;
using NeoServer.Server.Tasks;

namespace NeoServer.Server.Jobs.Creatures
{
    public class GameCreatureJob
    {
        private const ushort EVENT_CHECK_CREATURE_INTERVAL = 500;
        private readonly Game game;
        private readonly SpawnManager spawnManager;

        public GameCreatureJob(Game game, SpawnManager spawnManager)
        {
            this.game = game;
            this.spawnManager = spawnManager;
        }

        public void StartCheckingCreatures()
        {
            game.Scheduler.AddEvent(new SchedulerEvent(EVENT_CHECK_CREATURE_INTERVAL, StartCheckingCreatures));

            foreach (var creature in game.CreatureManager.GetCreatures()) //todo: change to only creatures to check
            {
                if (creature is ICombatActor actor && actor.IsDead)
                {
                    continue;
                }
                if (creature is IPlayer)
                {
                    PlayerPingJob.Execute((IPlayer)creature, game);
                }

                CreatureConditionJob.Execute(creature as ICombatActor);

                RespawnJob.Execute(spawnManager);
            }

        }
    }
}
