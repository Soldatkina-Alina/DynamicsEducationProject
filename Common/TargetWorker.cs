using Common.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class TargetWorker : ITargetWorker
    {
        Entity target;
        ITargetWorker image;

        public Guid GetCurrentId()
        {
            return target.Id;
        }
        public TargetWorker(IServiceProvider serviceProvider, ITargetWorker image = null)
        {
            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            target = (Entity)pluginContext.InputParameters["Target"];

            this.image = image;
        }
        public bool Contains(string attribute)
        {
            return target.Attributes.Contains(attribute);
        }
        public T GetAttribute<T>(string attributeLogicalName)
        {
            if (Contains(attributeLogicalName))
                return target.GetAttributeValue<T>(attributeLogicalName);
            else if (image != null)
                return GetAttributeImage<T>(attributeLogicalName);
            return default(T);
        }

        public T GetAttributeTarget<T>(string attributeLogicalName)
        {
            if (target.Contains(attributeLogicalName))
                return target.GetAttributeValue<T>(attributeLogicalName);
            return default(T);
        }

        public T GetAttributeImage<T>(string attributeLogicalName)
        {
             if (image.Contains(attributeLogicalName))
                return image.GetAttribute<T>(attributeLogicalName);
            return default(T);
        }

        public int GetOptionSetValue(string attributeLogicalName)
        {
            if (Contains(attributeLogicalName))
                return ((OptionSetValue)(target.Attributes[attributeLogicalName])).Value;
            else if (image != null)
                return image.GetOptionSetValue(attributeLogicalName);
            else return 0;
        }

        public void SetOptionValue(string Attribute, int value)
        {
            target.Attributes[Attribute] = new OptionSetValue(value);
        }
        public void SetValue<T>(string Attribute, T value)
        {
            target.Attributes[Attribute] = value;
        }
    }

}
