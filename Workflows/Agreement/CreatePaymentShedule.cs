﻿using Common.Entities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflows.Agreement.Handler;

namespace Workflows.Agreement
{
    public class CreatePaymentShedule : CodeActivity
    {
        [Input("ptest_agreement")]
        [RequiredArgument]
        [ReferenceTarget("ptest_agreement")]

        public InArgument<EntityReference> AgreementReference { get; set; }

        [Output("Agreement don't have invoice with type = Ruchnoe_sozdanie and status  = Payd")]
        public OutArgument<bool> IsValid { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // контекст исполнения 
            var wfContext = context.GetExtension<IWorkflowContext>() ;
            var servicefactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = servicefactory.CreateOrganizationService(null);

            try
            {
                var agreementRef = AgreementReference.Get(context);
                InvoiceWorker invoiceWorker = new InvoiceWorker(service);

                if (invoiceWorker.CheckInvoice())
                {
                    IsValid.Set(context, false);
                    return;
                }
                else
                {
                    invoiceWorker.DeleteAutoInvoice();
                    invoiceWorker.CreateInvoice(agreementRef);
                    invoiceWorker.SetNewDatePaymentShedule(agreementRef);
                }

                IsValid.Set(context, true);
            }
            catch (Exception ex)
            {
                IsValid.Set(context, false);
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}
