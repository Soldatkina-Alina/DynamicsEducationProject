using Common;
using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Communication
{
    public class Handler
    {
        ServiceWorker serviceWorker;
        TargetWorker targetWorker;

        public Handler(ServiceWorker serviceWorker, TargetWorker targetWorker)
        {
            this.serviceWorker = serviceWorker;
            this.targetWorker = targetWorker;
        }

        public bool HasAnotherCommunication(bool isMainTelephon = false)
        {
            EntityReference entityReference = targetWorker.GetAttribute<EntityReference>(ptest_communication.Fields.ptest_contactid); // взять ссылку на контакт
            
            QueryExpression query = new QueryExpression();
            query.ColumnSet = new ColumnSet(ptest_communication.Fields.Id);
            query.NoLock = true;
            query.Criteria.AddCondition(ptest_communication.Fields.ptest_contactid, ConditionOperator.Equal, entityReference.Id);

            if (isMainTelephon)
            {
                query.Criteria.AddCondition(ptest_communication.Fields.ptest_type, ConditionOperator.Equal, ptest_communication_ptest_type.Telefon);
                query.Criteria.AddCondition(ptest_communication.Fields.ptest_main, ConditionOperator.Equal, true);
            }

            EntityCollection result = serviceWorker.RetrieveMultiple(query);
            if(result.Entities.Count > 0)
                return true;
            return false;
        }
    }
}
