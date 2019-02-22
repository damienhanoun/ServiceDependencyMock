using Business;
using Business.Mock;
using Business.Mock.ServiceMethods;
using Business.Mock.ServiceMethods.Get;
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

        private MockType mockMethodReturnOne => new MockType
        {
            MethodIdentifier = ServiceIdentifier.GetIdentifier,
            Strategy = nameof(ServiceGetOne)
        };

        private MockType mockObjectReturnTwo => new MockType
        {
            MethodIdentifier = ServiceIdentifier.GetIdentifier,
            Strategy = Strategies.ObjectOnly,
            Context = new GetContext { MockedObject = 2 }
        };

        private MockType dontMock => new MockType
        {
            MethodIdentifier = ServiceIdentifier.GetIdentifier,
            Strategy = Strategies.None
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
            FakeDatabase.MockTypes = new List<MockType>();
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

            mockType.MethodIdentifier = "e68dbe94-29d3-4b32-8ee9-e16fc907ae00";
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
            var mockType = new MockType
            {
                MethodIdentifier = ServiceIdentifier.GetIdentifier,
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
            var mockType = new MockType
            {
                MethodIdentifier = ServiceIdentifier.GetIdentifier,
                Strategy = Strategies.ObjectOnly,
                Context = new GetContext { MockedObject = specificObject }
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
        public void Should_mock_with_object_according_to_context()
        {
            //Arrange
            int mockedObjectValue = 1;
            var mockMethodReturnOne = this.mockMethodReturnOne;
            var sessionId = "9c877052-5cc8-4793-8bd4-d3ea5122f8da";
            mockMethodReturnOne.Context = new GetContext
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
            var sessionId = "9c877052-5cc8-4793-8bd4-d3ea5122f8da";
            ApplicationDatabase.SessionId = sessionId;

            int objectValue = 1;
            var mockMethodReturnOne = this.mockMethodReturnOne;
            mockMethodReturnOne.Context = new GetContext
            {
                MockedObject = 0,
                SessionId = sessionId
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
