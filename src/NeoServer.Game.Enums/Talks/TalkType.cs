﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NeoServer.Game.Enums.Talks
{
    public enum TalkType: byte
    {
		Say = 1,
		Whisper = 2,
		Yell = 3,
		PrivatePn = 4,
		PrivateNp = 5,
		Private = 6,
		ChannelY = 7,
		ChannelW = 8,
		RvrChannel = 9,
		RvrAnswer = 10,
		RvrContinue = 11,
		Broadcast = 12,
		ChannelR1 = 13, //red - #c text
		PrivateRed = 14, //@name@text
		Channel_o = 15, //@name@text
		ChannelR2 = 17, //#d
		MonsterSay = 19,
		MonsterYell = 20,
	}
}
