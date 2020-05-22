using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Awsm.HotSwap.Tests.Integration.ServiceResolution.Default
{
    [TestFixture]
    public class DefaultServiceResolutionTests
    {
        private const string InputString = "Test";
        private const string ReversedResult = "tseT";
        private const string UppercaseResult = "TEST";
        private const string TruncastedResult = "Test";
        private const string ErrorString = "Something went wrong.";
        
        [Test]
        public void WhenTwoServicesAreRegisteredWithNoneAsDefault_ShouldResolveAService()
        {
            // Arrange
            var serviceCol = new ServiceCollection();

            // Act
            serviceCol.AddScopedHotSwapService<IStringService>()
                .AddImplementation<ReverseStringService>()
                .AddImplementation<UppercaseStringService>();
                
            var services = serviceCol.BuildServiceProvider();
            
            // Assert
            var impl = services.GetRequiredService<IStringService>();
            Assert.AreEqual(typeof(ReverseStringService), impl.GetType());
            Assert.IsFalse(string.IsNullOrWhiteSpace(impl.FormatString(InputString)));
        }
        
        [Test]
        public void WhenTwoServicesAreRegisteredWithOneAsDefault_ShouldResolveTheDefaultService()
        {
            // Arrange
            var serviceCol = new ServiceCollection();

            // Act
            serviceCol.AddScopedHotSwapService<IStringService>()
                .AddImplementation<ReverseStringService>() 
                .AddDefaultImplementation<UppercaseStringService>();

            var services = serviceCol.BuildServiceProvider();
            
            // Assert
            var impl = services.GetRequiredService<IStringService>();
            Assert.AreEqual(typeof(UppercaseStringService), impl.GetType());
            Assert.AreEqual(impl.FormatString(InputString), UppercaseResult);
        }
    }
}