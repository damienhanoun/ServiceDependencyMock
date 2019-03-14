using Mock.Data.Tranfer.Objects.Strategies;
using Mock.Define.Strategy.Builder;
using Mock.Define.Strategy.Tests.Helpers;
using NFluent;
using Optional;
using Xunit;

namespace Mock.Define.Strategy.Tests
{
    public class MockBuilderTest
    {
        [Fact]
        public void Should_build_noMockStrategy_with_context()
        {
            //Arrange
            var methodId = "id";
            var context = new MockContext { };

            //Act
            var mockStrategy = MockStrategyBuilder
                .ForMethod(methodId)
                .OnceWithoutMock()
                .WithContext(context);

            //Assert
            var expectedMockStrategy = new ForceNoMockStrategy()
            {
                MethodId = methodId,
                Context = Option.Some<dynamic>(context)
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_noMockStrategy()
        {
            //Arrange
            var methodId = "id";

            //Act
            var mockStrategy = MockStrategyBuilder
                .ForMethod(methodId)
                .OnceWithoutMock();

            //Assert
            var expectedMockStrategy = new ForceNoMockStrategy()
            {
                MethodId = methodId
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_mockMethodStrategy_with_context()
        {
            //Arrange
            var methodId = "id";
            var context = new MockContext { };

            //Act
            var mockStrategy = MockStrategyBuilder
                .ForMethod(methodId)
                .OnceWithMethodMockStrategy(nameof(MockMethodStrategy))
                .WithContext(context);

            //Assert
            var expectedMockStrategy = new MethodToMockWithMethodStrategy()
            {
                MethodId = methodId,
                MethodMockStrategy = nameof(MockMethodStrategy),
                Context = Option.Some<dynamic>(context)
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_callOnceMockMethodStrategy()
        {
            //Arrange
            var methodId = "id";

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .OnceWithMethodMockStrategy(nameof(MockMethodStrategy));

            //Assert
            var expectedMockStrategy = new MethodToMockWithMethodStrategy()
            {
                MethodId = methodId,
                MethodMockStrategy = nameof(MockMethodStrategy)
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_callAlwaysMockMethodStrategy()
        {
            //Arrange
            var methodId = "id";

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .AlwaysWithMethodMockStrategy(nameof(MockMethodStrategy));

            //Assert
            var expectedMockStrategy = new MethodToMockWithMethodStrategy()
            {
                MethodId = methodId,
                MethodMockStrategy = nameof(MockMethodStrategy),
                IsAlwaysApplied = true
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_mockObjectStrategy_with_context()
        {
            //Arrange
            var methodId = "id";
            var mockedObject = 1;
            var context = new MockContext { };

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .OnceWithObject(mockedObject)
                .WithContext(context);

            //Assert
            var expectedMockStrategy = new MethodToMockWithObjectStrategy<int>()
            {
                MethodId = methodId,
                MockedObject = mockedObject,
                Context = Option.Some<dynamic>(context)
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_callAlwaysMockObjectStrategy()
        {
            //Arrange
            var methodId = "id";
            var mockedObject = 1;

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .AlwaysWithObject(mockedObject);

            //Assert
            var expectedMockStrategy = new MethodToMockWithObjectStrategy<int>
            {
                MethodId = methodId,
                MockedObject = mockedObject,
                IsAlwaysApplied = true
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_callOnceMockObjectStrategy()
        {
            //Arrange
            var methodId = "id";
            var mockedObject = 1;

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .OnceWithObject(mockedObject);

            //Assert
            var expectedMockStrategy = new MethodToMockWithObjectStrategy<int>
            {
                MethodId = methodId,
                MockedObject = mockedObject
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }
    }
}
