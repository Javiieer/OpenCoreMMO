﻿using System.Threading;

namespace NeoServer.Server.Tasks.Contracts
{
    public interface IDispatcher
    {

        void AddEvent(IEvent evt, bool hasPriority = false);
        ulong GetCycles();

        void Start(CancellationToken token);
    }
}
