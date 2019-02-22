using Business;
using Business.Mock;
using Business.Mock.ClientSide;
using Business.Mock.ServiceSide;
using Business.Mock.ServiceSide.ServiceMethodsStrategies;
using Business.Mock.ServiceSide.ServiceMethodsStrategies.Get;
using NFluent;
using NSubstitute;
using ServiceDependencyMock;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestClient
{
    public class ServiceProxyTest : IDisposable
    {
        private readonly ServiceProxy serviceProxy;

        private readonly ClientSideRepository clientSideRepository;
        private readonly ServiceSideQuery serviceSideQuery;
        private readonly Service service;

        private MockStrategyContainer mockMethodReturnOne => new MockStrategyContainer
        {
            MethodIdentifier = ServiceMethodsIdentifiers.GetId,
            Strategy = nameof(ServiceGetOne)
        };

        private MockStrategyContainer mockObjectReturnTwo => new MockStrategyContainer
        {
            MethodIdentifier = ServiceMethodsIdentifiers.GetId,
            Strategy = DefaultStrategies.ObjectOnly,
            Context = new MockContext { MockedObject = 2 }
        };

        private MockStrategyContainer dontMock => new MockStrategyContainer
        {
            MethodIdentifier = ServiceMethodsIdentifiers.GetId,
            Strategy = DefaultStrategies.None
        };

        public ServiceProxyTest()
        {
            this.clientSideRepository = new ClientSideRepositoryImpl();
            this.serviceSideQuery = new ServiceSideQueryImpl();
            this.service = Substitute.For<Service>();
            this.serviceProxy = new ServiceProxy(this.serviceSideQuery, this.service);
        }

        public void Dispose()
        {
            FakeDatabase.MockTypes = new List<MockStrategyContainer>();
            ApplicationDatabase.SessionId = null;
        }

        [Fact]
        public void Should_mock_method_behavior()
        {
            //Arrange
            this.clientSideRepository.Mock(this.mockMethodReturnOne);

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
            var mockType = this.mockMethodReturnOne;

            var anotherMethodId = Guid.NewGuid().ToString();
            mockType.MethodIdentifier = anotherMethodId;
            this.clientSideRepository.Mock(mockType);

            //Act
            this.serviceProxy.Get();

            //Assert
            this.service.Received().Get();
        }

        [Fact]
        public void Should_crash_When_unexisting_strategy_is_used()
        {
            //Arrange
            var mockType = new MockStrategyContainer
            {
                MethodIdentifier = ServiceMethodsIdentifiers.GetId,
                Strategy = "unexisting strategy"
            };
            this.clientSideRepository.Mock(mockType);

            //Act
            Action action = () => this.serviceProxy.Get();

            //Assert
            Check.ThatCode(action).Throws<Exception>()
                 .WithMessage($"Method strategy '{mockType.Strategy}' is not defined");
        }

        [Fact]
        public void Should_mock_with_specific_object_When_defined()
        {
            //Arrange
            var specificObject = 10;
            var mockType = new MockStrategyContainer
            {
                MethodIdentifier = ServiceMethodsIdentifiers.GetId,
                Strategy = DefaultStrategies.ObjectOnly,
                Context = new MockContext { MockedObject = specificObject }
            };
            this.clientSideRepository.Mock(mockType);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(specificObject);
        }

        [Fact]
        public void Should_mock_with_First_In_First_Out_Strategy()
        {
            //Arrange
            this.clientSideRepository.Mock(this.mockMethodReturnOne);
            this.clientSideRepository.Mock(this.mockObjectReturnTwo);

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
            this.service.Get().Returns(0);

            this.clientSideRepository.Mock(this.mockMethodReturnOne);
            this.clientSideRepository.Mock(this.dontMock);
            this.clientSideRepository.Mock(this.mockObjectReturnTwo);

            //Act
            var result1 = this.serviceProxy.Get();
            var result2 = this.serviceProxy.Get();
            var result3 = this.serviceProxy.Get();

            //Assert
            Check.That(result1).IsEqualTo(1);
            Check.That(result2).IsEqualTo(0);
            Check.That(result3).IsEqualTo(2);
        }

        [Fact]
        public void Should_mock_with_object_When_context_is_the_same()
        {
            //Arrange
            int mockedObjectValue = 1;
            var mockMethodReturnOne = this.mockMethodReturnOne;
            var sessionId = Guid.NewGuid().ToString();
            mockMethodReturnOne.Context = new MockContext
            {
                MockedObject = mockedObjectValue,
                SessionId = sessionId
            };
            ApplicationDatabase.SessionId = sessionId;
            this.clientSideRepository.Mock(mockMethodReturnOne);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(mockedObjectValue);
        }

        [Fact]
        public void Should_not_mock_When_context_is_not_the_same()
        {
            //Arrange
            ApplicationDatabase.SessionId = Guid.NewGuid().ToString();

            int objectValue = 1;
            var mockMethodReturnOne = this.mockMethodReturnOne;
            mockMethodReturnOne.Context = new MockContext
            {
                MockedObject = 0,
                SessionId = Guid.NewGuid().ToString()
            };
            this.clientSideRepository.Mock(mockMethodReturnOne);

            this.service.Get().Returns(objectValue);

            //Act
            var result = this.serviceProxy.Get();

            //Assert
            Check.That(result).IsEqualTo(objectValue);
        }
    }
}
