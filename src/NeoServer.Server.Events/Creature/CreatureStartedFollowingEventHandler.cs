﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Enums.Location.Structs;
using NeoServer.Server.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Server.Events.Creature
{
    public class CreatureStartedFollowingEventHandler
    {
        private readonly Game game;

        public CreatureStartedFollowingEventHandler(Game game)
        {
            this.game = game;
        }
        public void Execute(IWalkableCreature creature, IWalkableCreature following, FindPathParams fpp)
        {
            if(creature.FollowEvent != 0)
            {
                return;
            }

            Follow(creature, following, fpp);

            creature.FollowEvent = game.Scheduler.AddEvent(new SchedulerEvent(1000, () => Follow(creature, following, fpp)));
        }

        private void Follow(IWalkableCreature creature, IWalkableCreature following, FindPathParams fpp)
        {
            if (creature.IsFollowing)
            {
                creature.Follow(following, fpp);
            }
            else
            {
                if(creature.FollowEvent != 0)
                {
                    game.Scheduler.CancelEvent(creature.FollowEvent);
                    creature.FollowEvent = 0;
                }
            }

            if (creature.FollowEvent != 0)
            {
                creature.FollowEvent = 0;
                Execute(creature, following, fpp);
            }
        }
    }
}
