using Common;
using Common.Entities;
using Common.Interfaces;
using Microsoft.Xrm.Sdk;
using Plugins.Invoice.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Invoice
{
    public class PreTypeInvoiceUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ServiceWorker serviceWorker = new ServiceWorker(serviceProvider);

            ITargetWorker imageWorker = new ImageWorker(serviceProvider, true, "Image");
            TargetWorker targetWorker = new TargetWorker(serviceProvider, imageWorker);
            

            if (!targetWorker.GetAttributeTarget<bool>(ptest_invoice.Fields.ptest_fact)) return;

            ChangeFact changeFact = new ChangeFact(targetWorker, serviceWorker);
            changeFact.ReactionOnChangeFact();
        }

        //public void Execute(IServiceProvider serviceProvider)
        //{
        //    var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        //    traceService.Trace("Get ITracingService");

        //    var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        //    var targetInvoice = (Entity)pluginContext.InputParameters["Target"];

        //    string name = targetInvoice.GetAttributeValue<string>(ptest_invoice.Fields.ptest_name);
        //    traceService.Trace("Название trace" + name);

        //    var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        //    var service = serviceFactory.CreateOrganizationService(Guid.Empty);

        //    GetType(service, targetInvoice);

        //    //throw new InvalidPluginExecutionException(name);
        //}

        //void GetType(IOrganizationService service, Entity target)
        //{
        //    string name = target.GetAttributeValue<string>("ptest_name");
        //    int type = ((OptionSetValue)(target.Attributes["ptest_type"])).Value;

        //    //var r = target.FormattedValues["ptest_type"].ToString();

        //    //target["ptest_type"] = new OptionSetValue(234260000);

        //    var r = ptest_invoice_ptest_type.Avtomaticheskoe_sozdanie;

        //    target["ptest_name"] = "New" + name + type;
        //}
    }
}
