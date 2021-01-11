using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IPluginProvider
    {
        Entity Target { get; }
        ITracingService TraceService { get; }
        IOrganizationService Service{ get; }
    }
}
