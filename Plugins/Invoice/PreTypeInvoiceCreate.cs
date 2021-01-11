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
