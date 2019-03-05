using Mock.Define.Strategy.Builder;
using Mock.Strategies;
using NFluent;
using Optional;
using Xunit;
using YourApplication.ServiceMethodsStrategies.Get;

namespace Mock.Define.Strategy.Tests
{
    public class MockBuilderTest
    {
        [Fact]
        public void Should_build_noMockStrategy_with_context()
        {
            //Arrange
            var methodId = "id";
            var context = new GetMockContext { };

            //Act
            var mockStrategy = MockStrategyBuilder
                .ForMethod(methodId)
                .WithoutMock()
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
                .WithoutMock();

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
            var context = new GetMockContext { };

            //Act
            var mockStrategy = MockStrategyBuilder
                .ForMethod(methodId)
                .WithStrategy(nameof(ServiceGetOne))
                .WithContext(context);

            //Assert
            var expectedMockStrategy = new MethodToMockWithMethodStrategy()
            {
                MethodId = methodId,
                Strategy = nameof(ServiceGetOne),
                Context = Option.Some<dynamic>(context)
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_mockMethodStrategy()
        {
            //Arrange
            var methodId = "id";

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .WithStrategy(nameof(ServiceGetOne));

            //Assert
            var expectedMockStrategy = new MethodToMockWithMethodStrategy()
            {
                MethodId = methodId,
                Strategy = nameof(ServiceGetOne)
            };
            Check.That(mockStrategy).IsEqualTo(expectedMockStrategy);
        }

        [Fact]
        public void Should_build_mockObjectStrategy_with_context()
        {
            //Arrange
            var methodId = "id";
            var mockedObject = 1;
            var context = new GetMockContext { };

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .WithObject(mockedObject)
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
        public void Should_build_mockObjectStrategy()
        {
            //Arrange
            var methodId = "id";
            var mockedObject = 1;

            //Act
            var mockStrategy = MockStrategyBuilder.ForMethod(methodId)
                .WithObject(mockedObject);

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
