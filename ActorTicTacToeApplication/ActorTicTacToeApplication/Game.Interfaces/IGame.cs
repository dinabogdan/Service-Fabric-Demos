﻿using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]

namespace Game.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IGame : IActor
    {
        Task<bool> AcceptPlayerToGameAsync(long playerId, string playerName);
        Task<int[]> GetGameBoardAsync();
        Task<string> GetWinnerAsync();
        Task<bool> AcceptPlayerMoveAsync(long playerId, int x, int y);

    }
}
