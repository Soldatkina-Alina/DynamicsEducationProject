using Common.Interfaces;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ImageWorker : ITargetWorker
    {
        Entity image;

        public ImageWorker(IServiceProvider serviceProvider, bool preImage, string ImageName)
        {
            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (preImage)
            image = pluginContext.PreEntityImages[ImageName];
            else image = pluginContext.PostEntityImages[ImageName];
        }

        public bool Contains(string attribute)
        {
            return image.Attributes.Contains(attribute);
        }
        public T GetAttribute<T>(string attribute)
        {
            if (image.Attributes.Contains(attribute))
                return image.GetAttributeValue<T>(attribute);
            else return default(T);
        }

        public int GetOptionSetValue(string attribute)
        {
            if (Contains(attribute))
                return ((OptionSetValue)(image.Attributes[attribute])).Value;
            else return 0;
        }
    }
}
