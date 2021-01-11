using Common.Interfaces;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ServiceWorker : IServiceWorker
    {
        IOrganizationService service;
        ITracingService traceService;
        public ServiceWorker(IServiceProvider serviceProvider, bool isServiseFromUserName = true)
        {
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            if (isServiseFromUserName)
                service = serviceFactory.CreateOrganizationService(Guid.Empty);
            else service = serviceFactory.CreateOrganizationService(null);

            traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            traceService.Trace("Get ITracingService");
        }

        public Entity GetEntityReference(EntityReference entity)
        {
            try
            {
                return service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));
            }
            catch (Exception ex) {
                traceService.Trace(ex.Message);
                return null;
            }
        }

        public T GetAttribute<T>(EntityReference entityReference, string Attribute)
        {
            Entity entity = service.Retrieve(entityReference.LogicalName, entityReference.Id, new ColumnSet(true));
            return GetAttribute<T>(entity, Attribute);
        }

        public T GetAttribute<T>(Entity entity, string Attribute)
        {
            T en;
            if (entity.TryGetAttributeValue<T>(Attribute, out en))
                return en;
            else
            {
                traceService.Trace($"{Attribute} return default");
                return default(T);
            }
        }

        public void UpdateAttribute <T>(EntityReference entity, string Attribute, T value )
        {
            try
            {
                Entity updateContact = service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));
                updateContact[Attribute] = value;

                service.Update(updateContact);
            }
            catch (Exception exc) 
            {
                traceService.Trace(exc.ToString());
                throw new InvalidPluginExecutionException(exc.Message);
            }
        }

        public EntityCollection RetrieveMultiple(QueryExpression queryExpression)
        {
            try
            {
                return service.RetrieveMultiple(queryExpression);
            }
            catch (Exception ex)
            {
                traceService.Trace(ex.Message);
                return null;
            }
        }
    }
}
