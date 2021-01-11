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
    public class PostInvoiceCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            TargetWorker targetWorker = new TargetWorker(serviceProvider);

            if (!targetWorker.GetAttributeTarget<bool>(ptest_invoice.Fields.ptest_fact)) return;

            targetWorker.SetValue(ptest_invoice.Fields.ptest_paydate, DateTime.Now.Date);
        }
    }
}
