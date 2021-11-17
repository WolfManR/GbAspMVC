using System;
using Autofac;

namespace CalculatorApp
{
    static class AutofacExtensions
    {
        public static ContainerBuilder RegisterMany(this ContainerBuilder self, Action<ContainerBuilder> registration)
        {
            registration?.Invoke(self);
            return self;
        }

        public static ContainerBuilder AddOperation(this ContainerBuilder self, string name, string description, Operation<double> operation)
        {
            OperationMetadata metadata = new()
            {
                Name = name,
                Description = description,
                Operation = operation
            };
            self.RegisterInstance(metadata);
            return self;
        }
    }
}