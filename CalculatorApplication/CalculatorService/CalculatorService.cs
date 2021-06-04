using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalculatorService.Interfaces.Contracts;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace CalculatorService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CalculatorService : StatelessService, ICalculatorService
    {
        public CalculatorService(StatelessServiceContext context)
        : base(context)
        { }

        public Task<string> Add(int a, int b)
        {
            return Task.FromResult<string>($"Instance {this.Context.InstanceId} returns {a + b}");
        }
        public Task<string> Subtract(int a, int b)
        {
            return Task.FromResult<string>($"Instance {this.Context.InstanceId} returns {a - b}");
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }
    }
}
