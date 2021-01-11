using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Agreement.Handler
{
    public class WorkWithDate
    {
        IOrganizationService service;

        public WorkWithDate(IOrganizationService service)
        {
            this.service = service;
        }

        public bool IsMinDate(EntityReference contact, DateTime dateAgreement)
        {
            QueryExpression query = new QueryExpression(ptest_agreement.EntityLogicalName);
            query.ColumnSet = new ColumnSet(ptest_agreement.Fields.ptest_name, ptest_agreement.Fields.ptest_date);
            query.NoLock = true;

            var link = query.AddLink(Contact.EntityLogicalName, ptest_agreement.Fields.ptest_contact, Contact.PrimaryIdAttribute);
            link.EntityAlias = "con";
            link.Columns = new ColumnSet(Contact.PrimaryIdAttribute);

            query.Criteria.AddCondition(ptest_agreement.Fields.ptest_contact, ConditionOperator.Equal, contact.Id);

            EntityCollection result = service.RetrieveMultiple(query);

            if (result.Entities.Count > 1)
                foreach (var en in result.Entities)
                    if (en.GetAttributeValue<DateTime>(ptest_agreement.Fields.ptest_date) < dateAgreement)
                        return false;
            return true;
        }

        public void setMinDate(EntityReference contact, DateTime dateAgreement)
        {
            Entity updateContact = service.Retrieve(Contact.EntityLogicalName, contact.Id, new ColumnSet(true));
            updateContact[Contact.Fields.ptest_date] = dateAgreement;

            service.Update(updateContact);
        }
    }
}
