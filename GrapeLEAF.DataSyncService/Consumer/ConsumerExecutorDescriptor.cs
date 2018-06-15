using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GrapeLEAF.DataSyncService
{
    public class ConsumerExecutorDescriptor
    {
        public MethodInfo MethodInfo { get; set; }

        public TypeInfo ImplTypeInfo { get; set; }

        public SubscribeAttribute Attribute { get; set; }
    }

    public static class ConsumerExecutorDescriptorExtensions
    {
        public static List<ConsumerExecutorDescriptor> ToDescriptors(this Dictionary<string, string> queueServiceMapping)
        {
            List<ConsumerExecutorDescriptor> descriptors = new List<ConsumerExecutorDescriptor>();

            foreach (var item in queueServiceMapping)
            {
                Type t = Type.GetType(item.Value);

                MethodInfo[] methods = t.GetMethods();

                foreach (var method in methods)
                {
                    var attrs = method.GetCustomAttributes().Where(a => a.GetType() == typeof(SubscribeAttribute));

                    foreach (var attr in attrs)
                    {
                        descriptors.Add(new ConsumerExecutorDescriptor()
                        {
                            MethodInfo = method,
                            ImplTypeInfo = t.GetTypeInfo(),
                            Attribute = (SubscribeAttribute)attr
                        });
                    }
                }
            }

            return descriptors;
        }
    }
}
