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
    public class PreInvoiceDelete : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetDelete = (EntityReference)pluginContext.InputParameters["Target"];

            ServiceWorker serviceWorker = new ServiceWorker(serviceProvider);

            Money amount = serviceWorker.GetAttribute<Money>(targetDelete, ptest_invoice.Fields.ptest_amount);
            bool fact = serviceWorker.GetAttribute<bool>(targetDelete, ptest_invoice.Fields.ptest_fact);
            if (amount == null || !fact) return;

            EntityReference dogovorid = serviceWorker.GetAttribute<EntityReference>(targetDelete, ptest_invoice.Fields.ptest_dogovorid);

            Money factsummaAgreement = serviceWorker.GetAttribute<Money>(dogovorid, ptest_agreement.Fields.ptest_factsumma);
            if (factsummaAgreement == null) return;

            serviceWorker.UpdateAttribute<decimal>(dogovorid, ptest_agreement.Fields.ptest_factsumma, factsummaAgreement.Value - amount.Value);
            serviceWorker.UpdateAttribute<bool>(dogovorid, ptest_agreement.Fields.ptest_fact, false);
        }
    }
}
