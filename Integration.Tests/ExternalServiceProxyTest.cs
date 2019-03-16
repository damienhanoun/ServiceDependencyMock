using IntegrationTests.ExternalProject;
using IntegrationTests.Helpers;
using IntegrationTests.ProjectWithProxy;
using IntegrationTests.ProjectWithProxy.ServiceMethodsStrategies.Get;
using Microsoft.EntityFrameworkCore;
using Mock.Dependency.With.Proxy.Apply.Strategy;
using Mock.Dependency.With.Proxy.Data.Transfer.Objects.DatabaseEntities.SqlServer;
using Mock.Dependency.With.Proxy.Define.Strategy;
using NFluent;
using NSubstitute;
using System;
using Xunit;
using static IntegrationTests.ProjectWithProxy.Helpers.ConfigurationReader;
using static IntegrationTests.ProjectWithProxy.ServiceMethodsStrategies.ServiceMethodsIdentifiers;

namespace IntegrationTests
{
    public class ExternalServiceProxyTest
    {
        private readonly HelperRepository helperRepository;

        private readonly Mock.Dependency.With.Proxy.Define.Strategy.MockStrategyRepository defineMockStrategyRepository;
        private readonly Mock.Dependency.With.Proxy.Apply.Strategy.MockStrategyRepository applyMockStrategyRepository;
        private readonly MockConfiguration mockConfiguration;

        private readonly ExternalService service;
        private readonly ExternalService serviceProxy;

        private static readonly DbContextOptionsBuilder<MockStrategiesContext> optionsBuilder;
        private static readonly string connectionString;

        static ExternalServiceProxyTest()
        {
            connectionString = Get("ConnectionString");
            optionsBuilder = new DbContextOptionsBuilder<MockStrategiesContext>();
            optionsBuilder.UseSqlServer(connectionString);
        }

        public ExternalServiceProxyTest()
        {
            this.mockConfiguration = Substitute.For<MockConfiguration>();
            this.mockConfiguration.IsActivated().Returns(true);

            this.defineMockStrategyRepository = new Mock.Dependency.With.Proxy.Define.Strategy.MockStrategyRepositorySqlServer(connectionString);
            this.applyMockStrategyRepository = new Mock.Dependency.With.Proxy.Apply.Strategy.MockStrategyRepositorySqlServer(connectionString, this.mockConfiguration);
            this.helperRepository = new HelperRepositorySqlServer(optionsBuilder);

            this.helperRepository.RemoveAllStrategies();

            this.service = Substitute.For<ExternalService>();
            this.serviceProxy = new ExternalServiceProxy(this.applyMockStrategyRepository, this.service);
        }

        [Fact]
        public void Should_mock_method_behavior()
        {
            //Arrange
            var mockMethodStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithSubstituteBehavior(nameof(ServiceGetOne));
            this.defineMockStrategyRepository.MockBehavior(mockMethodStrategy);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(1);
        }

        [Fact]
        public void Should_not_mock_When_no_strategy_defined()
        {
            //Arrange
            this.service.Get().Returns(0);

            //Act
            this.serviceProxy.Get();

            //Assert
            this.service.Received().Get();
        }

        [Fact]
        public void Should_not_mock_When_strategy_concern_another_method()
        {
            //Arrange
            var methodMockStrategy = MockStrategyBuilder.ForMethod("fakeMethodId")
                                            .OnceWithSubstituteBehavior(nameof(ServiceGetOne));
            this.defineMockStrategyRepository.MockBehavior(methodMockStrategy);

            //Act
            this.serviceProxy.Get();

            //Assert
            this.service.Received().Get();
        }

        [Fact]
        public void Should_crash_When_unexisting_method_strategy_is_used()
        {
            //Arrange
            var methodMockStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithSubstituteBehavior("unexisting strategy");
            this.defineMockStrategyRepository.MockBehavior(methodMockStrategy);

            //Act
            Action action = () => this.serviceProxy.Get();

            //Assert
            Check.ThatCode(action).Throws<Exception>()
                 .WithMessage($"Method strategy '{methodMockStrategy.BehaviorName}' is not defined");
        }

        [Fact]
        public void Should_mock_with_object_When_defined()
        {
            //Arrange
            var specificObject = 10;
            var mockObjectStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithObject(specificObject);
            this.defineMockStrategyRepository.MockObject(mockObjectStrategy);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(specificObject);
        }

        [Fact]
        public void Should_mock_with_First_In_First_Out_Strategy()
        {
            //Arrange
            var firstMockStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithObject(1);
            var secondMockStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithObject(2);

            this.defineMockStrategyRepository.MockObject(firstMockStrategy);
            this.defineMockStrategyRepository.MockObject(secondMockStrategy);

            //Act
            var result1 = this.serviceProxy.Get();
            var result2 = this.serviceProxy.Get();

            //Assert
            Check.That(result1).IsEqualTo(1);
            Check.That(result2).IsEqualTo(2);
        }

        [Fact]
        public void Should_not_mock_one_call_between_two_mocked_call()
        {
            //Arrange
            var firstMockStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithObject(1);
            var secondMockStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithoutMock();
            var thirdMockStrategy = MockStrategyBuilder.ForMethod(GetId).OnceWithObject(3);

            this.service.Get().Returns(2);

            this.defineMockStrategyRepository.MockObject(firstMockStrategy);
            this.defineMockStrategyRepository.DontMock(secondMockStrategy);
            this.defineMockStrategyRepository.MockObject(thirdMockStrategy);

            //Act
            var result1 = this.serviceProxy.Get();
            var result2 = this.serviceProxy.Get();
            var result3 = this.serviceProxy.Get();

            //Assert
            Check.That(result1).IsEqualTo(firstMockStrategy.MockedObject);
            Check.That(result2).IsEqualTo(2);
            Check.That(result3).IsEqualTo(thirdMockStrategy.MockedObject);
        }

        [Fact]
        public void Should_mock_with_object_When_context_is_the_same()
        {
            //Arrange
            var sessionId = Guid.NewGuid().ToString();

            ApplicationDatabase.SessionId = sessionId;

            var mockStrategyWithSameContext = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithObject(1)
                .WithContext(new GetMockContext { SessionId = sessionId });

            this.defineMockStrategyRepository.MockObject(mockStrategyWithSameContext);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(mockStrategyWithSameContext.MockedObject);
        }

        [Fact]
        public void Should_not_mock_When_context_is_not_the_same()
        {
            //Arrange
            ApplicationDatabase.SessionId = Guid.NewGuid().ToString();

            var mockStrategyWithDifferentContext = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithObject(1)
                .WithContext(new GetMockContext { SessionId = Guid.NewGuid().ToString() });
            this.defineMockStrategyRepository.MockObject(mockStrategyWithDifferentContext);

            this.service.Get().Returns(2);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(2);
        }

        [Fact]
        public void Should_mock_with_strategy_When_the_first_saved_strategy_is_not_the_same()
        {
            //Arrange
            var sessionId = Guid.NewGuid().ToString();
            ApplicationDatabase.SessionId = sessionId;

            var mockStrategyWithDifferentContext = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithObject(1)
                .WithContext(new GetMockContext { SessionId = Guid.NewGuid().ToString() });
            var mockStrategyWithSameContext = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithObject(2)
                .WithContext(new GetMockContext { SessionId = sessionId });

            this.defineMockStrategyRepository.MockObject(mockStrategyWithDifferentContext);
            this.defineMockStrategyRepository.MockObject(mockStrategyWithSameContext);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(2);
        }

        [Fact]
        public void Should_mock_with_strategy_When_context_exist_but_sessionId_is_not_set()
        {
            //Arrange
            ApplicationDatabase.SessionId = Guid.NewGuid().ToString();

            var mockStrategyWithDifferentContext = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithObject(1)
                .WithContext(new GetMockContext { SessionId = null });

            this.defineMockStrategyRepository.MockObject(mockStrategyWithDifferentContext);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(1);
        }

        [Fact]
        public void Should_alwaysMockWith_strategy()
        {
            //Arrange
            var mockStrategyAlwaysApplied = MockStrategyBuilder.ForMethod(GetId)
                .AlwaysWithObject(1);

            this.defineMockStrategyRepository.MockObject(mockStrategyAlwaysApplied);

            //Act
            var result = this.serviceProxy.Get();
            var result2 = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(1);
            Check.That(result2).IsEqualTo(1);
        }

        [Fact]
        public void Should_get_real_implementation_result_When_alwaysMockWith_strategy_si_deleted()
        {
            //Arrange
            var mockStrategyAlwaysApplied = MockStrategyBuilder.ForMethod(GetId)
                .AlwaysWithObject(1);

            this.defineMockStrategyRepository.MockObject(mockStrategyAlwaysApplied);

            this.service.Get().Returns(0);

            //Act
            var resultMocked = this.serviceProxy.Get();
            this.defineMockStrategyRepository.RemoveStrategy(mockStrategyAlwaysApplied);
            var resultWithoutMock = this.serviceProxy.Get();

            //Assert
            Check.That(resultMocked).IsEqualTo(1);
            Check.That(resultWithoutMock).IsEqualTo(0);
        }

        [Fact]
        public void Should_not_mock_When_library_is_desactivated()
        {
            //Arrange
            this.mockConfiguration.IsActivated().Returns(false);

            var mockMethodStrategy = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithSubstituteBehavior(nameof(ServiceGetOne));
            this.defineMockStrategyRepository.MockBehavior(mockMethodStrategy);

            this.service.Get().Returns(0);

            //Act
            this.serviceProxy.Get();

            //Assert
            this.service.Received().Get();
        }

        [Fact]
        public void Should_clean_unused_strategy_When_not_applied()
        {
            //Arrange
            this.mockConfiguration.IsActivated().Returns(false);

            var mockMethodStrategy = MockStrategyBuilder.ForMethod(GetId)
                .OnceWithSubstituteBehavior(nameof(ServiceGetOne));
            this.defineMockStrategyRepository.MockBehavior(mockMethodStrategy);

            this.service.Get().Returns(0);
            this.serviceProxy.Get();

            //Act
            this.defineMockStrategyRepository.CleanUnUsedStrategiesDefinedByThisRepository();

            //Assert
            var strategiesInDatabase = this.helperRepository.GetStrategies();
            Check.That(strategiesInDatabase).IsEmpty();
        }
    }
}
