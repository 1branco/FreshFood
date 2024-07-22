using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTests.Base
{
    public sealed class ServiceCollectionVerifier
    {
        private readonly Mock<IServiceCollection> serviceCollectionMock;

        public ServiceCollectionVerifier()
        {
            this.serviceCollectionMock = new Mock<IServiceCollection>();
        }

        public void ContainsSingletonService<TService, TInstance>()
        {
            this.IsRegistered<TService, TInstance>(ServiceLifetime.Singleton);
        }

        public void ContainsTransientService<TService, TInstance>()
        {
            this.IsRegistered<TService, TInstance>(ServiceLifetime.Transient);
        }

        public void ContainsScopedService<TService, TInstance>()
        {
            this.IsRegistered<TService, TInstance>(ServiceLifetime.Scoped);
        }

        private void IsRegistered<TService, TInstance>(ServiceLifetime lifetime)
        {
            this.serviceCollectionMock
                .Verify(serviceCollection => serviceCollection.Add(
                    It.Is<ServiceDescriptor>(serviceDescriptor => serviceDescriptor.Is<TService, TInstance>(lifetime))));

        }
    }
}
