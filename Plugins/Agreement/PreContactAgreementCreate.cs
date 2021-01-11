using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugins.Agreement.Handler;

namespace Plugins.Agreement
{
    public class PreContactAgreementCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            traceService.Trace("Get ITracingService");

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetInvoice = (Entity)pluginContext.InputParameters["Target"];

            EntityReference contact = targetInvoice.GetAttributeValue<EntityReference>(ptest_agreement.Fields.ptest_contact);

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(Guid.Empty);

            DateTime date = targetInvoice.GetAttributeValue<DateTime>(ptest_agreement.Fields.ptest_date);

            WorkWithDate workWithDate = new WorkWithDate(service);

            try
            {
                if (workWithDate.IsMinDate(contact, date))
                    workWithDate.setMinDate(contact, date);
            }

            catch (Exception ex)
            {
                traceService.Trace(ex.Message);
            }
        }
    }
}