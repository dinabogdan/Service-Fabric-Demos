using Grpc.Core;
using gRPCServer.Services;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gRPCServer
{
    public class gRPCCommunicationListener : ICommunicationListener
    {

        Server mServer;
        string mReplicaId;

        public gRPCCommunicationListener(string replicaId)
        {
            mReplicaId = replicaId;
        }

        public async void Abort()
        {
            if (mServer != null) await mServer.KillAsync();
        }

        public async Task CloseAsync(CancellationToken cancellationToken)
        {
            if (mServer != null) await mServer.ShutdownAsync();
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpoint = FabricRuntime.GetActivationContext()
                .GetEndpoint("ServiceEndpoint");

            mServer = new Server
            {
                Services =
                {
                    Calculator.Calculator.BindService(new CalculatorImpl(mReplicaId))
                },
                Ports =
                {
                    new ServerPort("localhost", endpoint.Port, ServerCredentials.Insecure)
                }
            };

            mServer.Start();

            return Task.FromResult<string>("localhost" + ":" + endpoint.Port.ToString());
        }
    }
}
