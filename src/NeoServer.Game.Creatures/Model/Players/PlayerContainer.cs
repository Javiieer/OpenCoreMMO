﻿using NeoServer.Game.Contracts.Creatures;
using NeoServer.Game.Contracts.Items;
using NeoServer.Game.Contracts.Items.Types;
using NeoServer.Server.Model.Players.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Game.Creatures.Model.Players
{
    internal class PlayerContainer
    {
        public IPlayer Player { get; }
        public PlayerContainer(IContainerItem container, IPlayer player)
        {
            Container = container;
            Player = player;
        }

        public byte Id { get; set; }

        public IContainerItem Container { get; }

        public RemoveItemFromOpenedContainer RemoveItem { get; private set; }
        public AddItemOnOpenedContainer AddItem { get; private set; }
        private bool eventsAttached;

        public void ItemAdded(IItem item)
        {
            AddItem?.Invoke(Player, Id, item);
        }
        public void ItemRemoved(byte slotIndex, IItem item)
        {
            RemoveItem?.Invoke(Player, Id, slotIndex, item);
        }

        public void AttachActions(RemoveItemFromOpenedContainer removeItemAction, AddItemOnOpenedContainer addItemAction)
        {
            if (RemoveItem == null)
            {
                RemoveItem += removeItemAction;
            }
            if (AddItem == null)
            {
                AddItem += addItemAction;
            }
        }

        public void AttachContainerEvent()
        {
            if (eventsAttached)
            {
                return;
            }
            Container.OnItemAdded += ItemAdded;
            Container.OnItemRemoved += ItemRemoved;
            eventsAttached = true;
        }

        internal void DetachContainerEvents()
        {

            Container.OnItemRemoved -= ItemRemoved;
            Container.OnItemAdded -= ItemAdded;
        }

        public override bool Equals(object obj)
        {
            return Container == (obj as PlayerContainer).Container;
        }
    }
}