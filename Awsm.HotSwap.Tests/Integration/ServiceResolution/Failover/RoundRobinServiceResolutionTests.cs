using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Awsm.HotSwap.Tests.Integration.ServiceResolution.Default
{
    [TestFixture]
    public class FailoverServiceResolutionTests
    {
        private const string InputString = "Test";
        private const string ReversedResult = "tseT";
        private const string UppercaseResult = "TEST";
        private const string TruncastedResult = "Test";
        private const string ErrorString = "Something went wrong.";
        
        [Test]
        public void WhenTwoServicesAreRegisteredAsFailoverAndOneFails_StartReturningTheNewService()
        {
            // Arrange
            var serviceCol = new ServiceCollection();

            // Act
            serviceCol.AddScopedHotSwapService<IStringService>()
                .AddImplementation<FailoverReverseStringService>() 
                .AddImplementation<UppercaseStringService>()
                .WithAutoRecovery(o =>
                {
                    o.ErrorCount = 2;
                    o.ErrorWindow = TimeSpan.FromMinutes(60);
                });
            
            var results = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                var services = serviceCol.BuildServiceProvider();
                var impl = services.GetRequiredService<IStringService>();
                results.Add(impl.FormatString(InputString));
            }

            // Assert
            Assert.IsTrue(results.Count == 3);
            Assert.AreEqual(new List<string> { ErrorString, ErrorString, UppercaseResult }, results);
        }
        
        [Test]
        public void WhenThreeServicesAreRegisteredAsFailover_ShouldRespectDefaultImplementation()
        {
            // Arrange
            var serviceCol = new ServiceCollection();

            // Act
            serviceCol.AddScopedHotSwapService<IStringService>()
                .AddImplementation<FailoverReverseStringService>() 
                .AddImplementation<TruncatedStringService>()
                .AddDefaultImplementation<UppercaseStringService>()
                .WithAutoRecovery(o =>
                {
                    o.ErrorCount = 2;
                    o.ErrorWindow = TimeSpan.FromMinutes(60);
                });
            
            var results = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                var services = serviceCol.BuildServiceProvider();
                var impl = services.GetRequiredService<IStringService>();
                results.Add(impl.FormatString(InputString));
            }

            // Assert
            Assert.IsTrue(results.Count == 3);
            Assert.AreEqual(new List<string> { UppercaseResult, UppercaseResult, UppercaseResult }, results);
        }
        
        [Test]
        public void WhenThreeServicesAreRegisteredAsFailoverAndOneFails_StartReturningTheNewServiceAndSkipAnyExcluded()
        {
            // Arrange
            var serviceCol = new ServiceCollection();

            // Act
            serviceCol.AddScopedHotSwapService<IStringService>()
                .AddImplementation<FailoverReverseStringService>() 
                .AddImplementation<TruncatedStringService>(o => o.ExcludeFromFailover = true)
                .AddImplementation<UppercaseStringService>()
                .WithAutoRecovery(o =>
                {
                    o.ErrorCount = 2;
                    o.ErrorWindow = TimeSpan.FromMinutes(60);
                });
            
            var results = new List<string>();
            for (var i = 0; i < 3; i++)
            {
                var services = serviceCol.BuildServiceProvider();
                var impl = services.GetRequiredService<IStringService>();
                results.Add(impl.FormatString(InputString));
            }

            // Assert
            Assert.IsTrue(results.Count == 3);
            Assert.AreEqual(new List<string> { ErrorString, ErrorString, UppercaseResult }, results);
        }
    }
}