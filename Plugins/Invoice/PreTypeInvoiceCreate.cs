using Common;
using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Plugins.Invoice.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Invoice
{
    public class PreTypeInvoiceCreate : IPlugin
    {
        //public void Execute(IServiceProvider serviceProvider)
        //{
        //    var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        //    traceService.Trace("Get ITracingService");

        //    var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        //    var targetInvoice = (Entity)pluginContext.InputParameters["Target"];

        //    string name = targetInvoice.GetAttributeValue<string>(ptest_invoice.Fields.ptest_name);

        //    //удалить 
        //    if (targetInvoice.Contains(ptest_invoice.Fields.ptest_type))
        //    {
        //        int type = ((OptionSetValue)(targetInvoice.Attributes[ptest_invoice.Fields.ptest_type])).Value;
        //        if (type == (int)ptest_invoice_ptest_type.Avtomaticheskoe_sozdanie)
        //            throw new NotImplementedException(name + " Avtomaticheskoe_sozdanie:  " + type);
        //        else if (type == (int)ptest_invoice_ptest_type.Ruchnoe_sozdanie)
        //            throw new NotImplementedException(name + " Ruchnoe_sozdanie:  " + type);
        //    }
        //    //оставить
        //    else targetInvoice.Attributes[ptest_invoice.Fields.ptest_type] = new OptionSetValue((int)ptest_invoice_ptest_type.Ruchnoe_sozdanie);

        //    if (targetInvoice.Contains(ptest_invoice.Fields.ptest_fact))
        //    {
        //        bool fact = targetInvoice.GetAttributeValue<bool>(ptest_invoice.Fields.ptest_fact);
        //        throw new InvalidPluginExecutionException(fact.ToString());
        //    }
        //}

        public void Execute(IServiceProvider serviceProvider)
        {
            ServiceWorker serviceWorker = new ServiceWorker(serviceProvider);
            TargetWorker targetWorker = new TargetWorker(serviceProvider);

            SetValueInEmptyTypeAttribute(targetWorker);

            if (!targetWorker.GetAttributeTarget<bool>(ptest_invoice.Fields.ptest_fact)) return;

            ChangeFact changeFact = new ChangeFact(targetWorker, serviceWorker);
            changeFact.ReactionOnChangeFact();
        }

        void SetValueInEmptyTypeAttribute(TargetWorker targetWorker)
        {
             if (targetWorker.GetOptionSetValue(ptest_invoice.Fields.ptest_type) == 0)
                targetWorker.SetValue(ptest_invoice.Fields.ptest_type, (int)ptest_invoice_ptest_type.Ruchnoe_sozdanie);
        }
    }
}
