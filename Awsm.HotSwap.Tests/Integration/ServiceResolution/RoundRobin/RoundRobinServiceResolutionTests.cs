using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Awsm.HotSwap.Tests.Integration.ServiceResolution.Default
{
    [TestFixture]
    public class RoundRobinServiceResolutionTests
    {
        private const string InputString = "Test";
        private const string ReversedResult = "tseT";
        private const string UppercaseResult = "TEST";
        private const string TruncastedResult = "Test";
        private const string ErrorString = "Something went wrong.";
        
        [Test]
        public void WhenThreeServicesAreRegisteredAsRoundRobin_CallingThreeTimesShouldReturnEachResultInTurn()
        {
            // Arrange
            var serviceCol = new ServiceCollection();

            // Act
            serviceCol.AddScopedHotSwapService<IStringService>()
                .AddImplementation<ReverseStringService>() 
                .AddImplementation<UppercaseStringService>()
                .AddImplementation<TruncatedStringService>()
                .WithRoundRobinSelection();
            
            var results = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                var services = serviceCol.BuildServiceProvider();
                var impl = services.GetRequiredService<IStringService>();
                results.Add(impl.FormatString(InputString));
            }

            // Assert
            Assert.IsTrue(results.Count == 3);
            Assert.AreEqual(new List<string> { ReversedResult, UppercaseResult, TruncastedResult }, results);
        }
    }
}