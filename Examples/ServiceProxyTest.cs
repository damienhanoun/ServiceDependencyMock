using DatabasesObjects.CSharp;
using ExternalDependency;
using Mock.Apply.Strategy;
using Mock.Apply.Strategy.MockStrategyQueryImplementations;
using Mock.Define.Strategy;
using Mock.Define.Strategy.Builder;
using Mock.Define.Strategy.Helpers;
using Mock.Define.Strategy.MockStrategyRepositoryImplementations;
using MockStrategiesCSharp;
using NFluent;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;
using YourApplication;
using YourApplication.ServiceMethodsStrategies.Get;
using static YourApplication.ServiceMethodsStrategies.ServiceMethodsIdentifiers;

namespace Examples
{
    public class ServiceProxyTest : IDisposable
    {
        private readonly ServiceProxy serviceProxy;

        private readonly MockStrategyRepository mockStrategyRepository;
        private readonly MockStrategyQuery mockStrategyQuery;
        private readonly Service service;

        public ServiceProxyTest()
        {
            this.mockStrategyRepository = new MockStrategyRepositoryCSharp();
            this.mockStrategyQuery = new MockStrategyQueryCSharp();
            //this.mockStrategyRepository = new MockStrategyRepositorySqlServer();
            //this.mockStrategyQuery = new MockStrategyQuerySqlServer();

            this.service = Substitute.For<Service>();
            this.serviceProxy = new ServiceProxy(this.mockStrategyQuery, this.service);

            //using (var context = new MockStrategiesContext())
            //    context.Database.ExecuteSqlCommand("TRUNCATE TABLE MockStrategy");
        }

        public void Dispose()
        {
            MockStrategies.MockStrategy = new List<MockStrategyEntity>();
            ApplicationDatabase.SessionId = null;
        }

        [Fact]
        public void Should_mock_method_behavior()
        {
            //Arrange
            var mockMethodStrategy = MockStrategyBuilder.ForMethod(GetId).WithStrategy(nameof(ServiceGetOne));
            this.mockStrategyRepository.MockMethod(mockMethodStrategy);

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
                                            .WithStrategy(nameof(ServiceGetOne));
            this.mockStrategyRepository.MockMethod(methodMockStrategy);

            //Act
            this.serviceProxy.Get();

            //Assert
            this.service.Received().Get();
        }

        [Fact]
        public void Should_crash_When_unexisting_method_strategy_is_used()
        {
            //Arrange
            var methodMockStrategy = MockStrategyBuilder.ForMethod(GetId).WithStrategy("unexisting strategy");
            this.mockStrategyRepository.MockMethod(methodMockStrategy);

            //Act
            Action action = () => this.serviceProxy.Get();

            //Assert
            Check.ThatCode(action).Throws<Exception>()
                 .WithMessage($"Method strategy '{methodMockStrategy.Strategy}' is not defined");
        }

        [Fact]
        public void Should_mock_with_object_When_defined()
        {
            //Arrange
            var specificObject = 10;
            var mockObjectStrategy = MockStrategyBuilder.ForMethod(GetId).WithObject(specificObject);
            this.mockStrategyRepository.MockObject(mockObjectStrategy);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(specificObject);
        }

        [Fact]
        public void Should_mock_with_First_In_First_Out_Strategy()
        {
            //Arrange
            var firstMockStrategy = MockStrategyBuilder.ForMethod(GetId).WithObject(1);
            var secondMockStrategy = MockStrategyBuilder.ForMethod(GetId).WithObject(2);

            this.mockStrategyRepository.MockObject(firstMockStrategy);
            this.mockStrategyRepository.MockObject(secondMockStrategy);

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
            var firstMockStrategy = MockStrategyBuilder.ForMethod(GetId).WithObject(1);
            var secondMockStrategy = MockStrategyBuilder.ForMethod(GetId).WithoutMock();
            var thirdMockStrategy = MockStrategyBuilder.ForMethod(GetId).WithObject(3);

            this.service.Get().Returns(2);

            this.mockStrategyRepository.MockObject(firstMockStrategy);
            this.mockStrategyRepository.DontMock(secondMockStrategy);
            this.mockStrategyRepository.MockObject(thirdMockStrategy);

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
                .WithObject(1)
                .WithContext(new GetMockContext { SessionId = sessionId });

            this.mockStrategyRepository.MockObject(mockStrategyWithSameContext);

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
                .WithObject(1)
                .WithContext(new GetMockContext { SessionId = Guid.NewGuid().ToString() });
            this.mockStrategyRepository.MockObject(mockStrategyWithDifferentContext);

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
                .WithObject(1)
                .WithContext(new GetMockContext { SessionId = Guid.NewGuid().ToString() });
            var mockStrategyWithSameContext = MockStrategyBuilder.ForMethod(GetId)
                .WithObject(2)
                .WithContext(new GetMockContext { SessionId = sessionId });

            this.mockStrategyRepository.MockObject(mockStrategyWithDifferentContext);
            this.mockStrategyRepository.MockObject(mockStrategyWithSameContext);

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
                .WithObject(1)
                .WithContext(new GetMockContext { SessionId = null });

            this.mockStrategyRepository.MockObject(mockStrategyWithDifferentContext);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(1);
        }

        [Fact]
        public void Should_always_mock_with_strategy()
        {
            //Arrange
            var mockStrategyAlwaysApplied = MockStrategyBuilder.ForMethod(GetId)
                .WithObject(1)
                .AlwaysApply();

            this.mockStrategyRepository.MockObject(mockStrategyAlwaysApplied);

            //Act
            var result = this.serviceProxy.Get();
            var result2 = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(1);
            Check.That(result2).IsEqualTo(1);
        }
    }
}
