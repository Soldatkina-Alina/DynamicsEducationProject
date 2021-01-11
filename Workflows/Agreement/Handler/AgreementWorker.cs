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
    class AgreementWorker
    {
        IOrganizationService service;

        public AgreementWorker(IOrganizationService service)
        {
            this.service = service;
        }

        public bool IsHasRelatedInvoices(EntityReference agreementRef)
        {
            var query = new QueryExpression(ptest_agreement.EntityLogicalName);
            query.Distinct = true;
            query.Criteria.AddCondition(ptest_agreement.EntityLogicalName, ConditionOperator.Equal, agreementRef.Id);
            query.ColumnSet.AddColumns(ptest_agreement.Fields.Id);

            var af = query.AddLink(ptest_invoice.EntityLogicalName, ptest_agreement.Fields.Id, ptest_invoice.Fields.ptest_dogovorid, JoinOperator.Inner);
            af.EntityAlias = "af";
            var af_LinkCriteria_0 = new FilterExpression();
            af.LinkCriteria.AddFilter(af_LinkCriteria_0);

            EntityCollection result = service.RetrieveMultiple(query);

            return result.Entities.Count > 0;
        }

        public bool IsRelatedContactHasEmail(EntityReference agreementRef)
        {
            var agreement = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, new ColumnSet(ptest_agreement.Fields.ptest_contact));

            Guid idContact = agreement.GetAttributeValue<Guid>(ptest_agreement.Fields.ptest_contact);

            var contact = service.Retrieve(Contact.EntityLogicalName, idContact, new ColumnSet(Contact.Fields.EMailAddress1, Contact.Fields.DoNotBulkEMail));

            return (contact.GetAttributeValue<string>(Contact.Fields.EMailAddress1) != null && !contact.GetAttributeValue<bool>(Contact.Fields.DoNotBulkEMail));
        }
    }
}
