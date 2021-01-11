using Common;
using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Invoice.Handler
{
    public class ChangeFact
    {
        TargetWorker targetWorker;
        ServiceWorker serviceWorker;
        public ChangeFact(TargetWorker targetWorker, ServiceWorker serviceWorker)
        {
            this.targetWorker = targetWorker;
            this.serviceWorker = serviceWorker;
        }

        public void ReactionOnChangeFact()
        {
            Money amount = targetWorker.GetAttribute<Money>(ptest_invoice.Fields.ptest_amount);
            if (amount == null) return;

            EntityReference dogovorid = targetWorker.GetAttribute<EntityReference>(ptest_invoice.Fields.ptest_dogovorid);

            Money summaAgreement = serviceWorker.GetAttribute<Money>(dogovorid, ptest_agreement.Fields.ptest_summa);
            Money factsummaAgreement = serviceWorker.GetAttribute<Money>(dogovorid, ptest_agreement.Fields.ptest_factsumma);

            if (factsummaAgreement == null)
                factsummaAgreement = new Money(0);

            decimal allSummaInvoice = GetAllSummaFromInvoices().Value + amount.Value;

            if (allSummaInvoice > summaAgreement.Value)
                throw new InvalidPluginExecutionException("Сумма по счетам превышает сумму в договоре " + allSummaInvoice);
            else if (allSummaInvoice == summaAgreement.Value)
                serviceWorker.UpdateAttribute<bool>(dogovorid, ptest_agreement.Fields.ptest_fact, true);

            serviceWorker.UpdateAttribute<decimal>(dogovorid, ptest_agreement.Fields.ptest_factsumma, factsummaAgreement.Value + amount.Value);
        }

        Money GetAllSummaFromInvoices()
        {
            Money summaAllinvoice = new Money(0);

            QueryExpression query = new QueryExpression(ptest_invoice.EntityLogicalName);
            query.ColumnSet = new ColumnSet(ptest_invoice.Fields.Id, ptest_invoice.Fields.ptest_amount);
            query.NoLock = true;
            query.Criteria.AddCondition(ptest_invoice.Fields.ptest_fact, ConditionOperator.Equal, true);
            query.Criteria.AddCondition(ptest_invoice.Fields.Id, ConditionOperator.NotEqual, targetWorker.GetCurrentId());

            EntityCollection result = serviceWorker.RetrieveMultiple(query);

            Money money = new Money(0);
            foreach (var en in result.Entities)
                if (en.TryGetAttributeValue<Money>(ptest_invoice.Fields.ptest_amount, out money))
                    summaAllinvoice.Value += money.Value;

            return summaAllinvoice;
        }
    }
}
