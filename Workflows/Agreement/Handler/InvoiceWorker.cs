using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflows.Agreement.Handler
{
    public class InvoiceWorker
    {
        IOrganizationService service;

        public InvoiceWorker(IOrganizationService service)
        {
            this.service = service;
        }
        public bool CheckInvoice()
        {
            var query = new QueryExpression(ptest_agreement.EntityLogicalName);
            query.Distinct = true;

            query.ColumnSet.AddColumns(ptest_agreement.Fields.Id);

            var af = query.AddLink(ptest_invoice.EntityLogicalName, ptest_agreement.Fields.Id, ptest_invoice.Fields.ptest_dogovorid);
            af.EntityAlias = "af";
            var af_LinkCriteria_0 = new FilterExpression();
            af.LinkCriteria.AddFilter(af_LinkCriteria_0);

            af_LinkCriteria_0.FilterOperator = LogicalOperator.Or;
            af_LinkCriteria_0.AddCondition(ptest_invoice.Fields.ptest_type, ConditionOperator.Equal, (int)ptest_invoice_ptest_type.Ruchnoe_sozdanie);
            af_LinkCriteria_0.AddCondition(ptest_invoice.Fields.ptest_fact, ConditionOperator.Equal, true);

            EntityCollection result = service.RetrieveMultiple(query);

            return result.Entities.Count > 0;
        }

        public void DeleteAutoInvoice()
        {
            var query = new QueryExpression(ptest_agreement.EntityLogicalName);
            query.Distinct = true;

            var ab = query.AddLink(ptest_invoice.EntityLogicalName, ptest_agreement.Fields.Id, ptest_invoice.Fields.ptest_dogovorid);
            ab.EntityAlias = "ab";
            ab.Columns.AddColumns(ptest_invoice.Fields.Id);
            ab.Columns.AddColumns(ptest_invoice.Fields.ptest_name);

            ab.LinkCriteria.AddCondition("ptest_type", ConditionOperator.Equal, (int)ptest_invoice_ptest_type.Avtomaticheskoe_sozdanie);
            EntityCollection result = service.RetrieveMultiple(query);

            foreach (var res in result.Entities)
                service.Delete(ptest_invoice.EntityLogicalName, (Guid)((AliasedValue)res.Attributes["ab." + ptest_invoice.Fields.Id]).Value);
        }

        public void CreateInvoice(EntityReference agreementRef)
        {
            var agreement = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, new ColumnSet(true));

            var creditamount = agreement.GetAttributeValue<Money>(ptest_agreement.Fields.ptest_creditamount);
            var creditperiod = agreement.GetAttributeValue<int>(ptest_agreement.Fields.ptest_creditperiod);
            var nameAgreement = agreement.GetAttributeValue<string>(ptest_agreement.Fields.ptest_name);

            int colMonth = (int)(creditperiod * 12);
            decimal summainvoice = creditamount.Value / colMonth;

            for (int i = 1; i <= colMonth; i++)
            {
                var startDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
                Money money = new Money(summainvoice);
                Entity invoice = new Entity(ptest_invoice.EntityLogicalName);
                invoice[ptest_invoice.Fields.ptest_name] = "Счет договора " + nameAgreement + " от " + startDate.AddMonths(i).ToShortDateString();
                invoice[ptest_invoice.Fields.ptest_date] = DateTime.Now.Date;
                invoice[ptest_invoice.Fields.ptest_paydate] = startDate.AddMonths(i);
                invoice[ptest_invoice.Fields.ptest_dogovorid] = agreementRef;
                invoice[ptest_invoice.Fields.ptest_type] = new OptionSetValue((int)ptest_invoice_ptest_type.Avtomaticheskoe_sozdanie);
                invoice[ptest_invoice.Fields.ptest_fact] = false;
                invoice[ptest_invoice.Fields.ptest_amount] = money;

                Guid contactId = service.Create(invoice);
            }
        }

        public void SetNewDatePaymentShedule(EntityReference agreementRef)
        {
            var agreement = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, new ColumnSet(true));
            agreement.Attributes[ptest_agreement.Fields.ptest_paymentplandate] = DateTime.Now.Date.AddDays(1);
            service.Update(agreement);
        }
    }
}
