using System;
using Autofac;

namespace ActualUserOfScanSpamDevice
{
    static class AutofacExtensions
    {
        public static ContainerBuilder RegisterMany(this ContainerBuilder self, Action<ContainerBuilder> registration)
        {
            registration?.Invoke(self);
            return self;
        }
    }
}