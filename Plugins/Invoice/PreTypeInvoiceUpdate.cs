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
    }
}
