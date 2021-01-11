using Microsoft.Xrm.Sdk;
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
    public class CreateInvoiceInAllPay : CodeActivity
    {
        [Input("ptest_agreement")]
        [RequiredArgument]
        [ReferenceTarget("ptest_agreement")]
        public InArgument<EntityReference> AgreementReference { get; set; }

        [Output("Agreement don't have related invoices")]
        public OutArgument<bool> IsHasRelatedInvoices { get; set; }

        [Output("Agreement's contact has free email")]
        public OutArgument<bool> IsRelatedContactHasEmail { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var agreementRef = AgreementReference.Get(context);

            var servicefactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = servicefactory.CreateOrganizationService(null);

            try
            {
                AgreementWorker agreemenWorker = new AgreementWorker(service);

                IsHasRelatedInvoices.Set(context, agreemenWorker.IsHasRelatedInvoices(agreementRef));
                IsRelatedContactHasEmail.Set(context, agreemenWorker.IsRelatedContactHasEmail(agreementRef));
            }
            catch (Exception ex)
            {
                throw new InvalidWorkflowException(ex.Message);
            }
        }
    }
}
