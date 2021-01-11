using Common;
using Common.Entities;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Invoice
{
    public class PostInvoiceUpdate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            TargetWorker targetWorker = new TargetWorker(serviceProvider);

            ServiceWorker serviceWorker = new ServiceWorker(serviceProvider);

            if (!targetWorker.GetAttributeTarget<bool>(ptest_invoice.Fields.ptest_fact)) return;

            Entity entity = serviceWorker.GetEntitty(targetWorker.GetCurrentId(), ptest_invoice.EntityLogicalName, new Microsoft.Xrm.Sdk.Query.ColumnSet(ptest_invoice.Fields.ptest_paydate));

            serviceWorker.UpdateAttribute<DateTime>(entity, ptest_invoice.Fields.ptest_paydate, DateTime.Now.Date);
        }
    }
}
