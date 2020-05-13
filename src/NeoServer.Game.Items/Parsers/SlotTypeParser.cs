﻿using NeoServer.Game.Enums.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Game.Items.Parsers
{
    internal class SlotTypeParser
    {
        public static Slot Parse(string slotType) => slotType switch
        {
            "body" => Slot.Body,
            "legs" => Slot.Legs,
            "head" => Slot.Head,
            "feet" => Slot.Feet,
            "shield" => Slot.Right,
            "ammo" => Slot.Ammo,
            "backpack" => Slot.Backpack,
            "ring" => Slot.Ring,
            "necklace" => Slot.Necklace,
            "teo-handed" => Slot.TwoHanded,
            _ => Slot.WhereEver
        };
    }
}
