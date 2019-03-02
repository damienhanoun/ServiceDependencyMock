using Mock.Strategies;
using Optional;

namespace Mock.Define.Strategy.Builder
{
    public static class StrategiesExtension
    {
        public static MethodToMockWithMethodStrategy WithContext(this MethodToMockWithMethodStrategy strategy, dynamic context)
        {
            strategy.Context = Option.Some<dynamic>(context);
            return strategy;
        }

        public static MethodToMockWithObjectStrategy<T> WithContext<T>(this MethodToMockWithObjectStrategy<T> strategy, dynamic context)
        {
            strategy.Context = Option.Some<dynamic>(context);
            return strategy;
        }

        public static ForceNoMockStrategy WithContext(this ForceNoMockStrategy strategy, dynamic context)
        {
            strategy.Context = Option.Some<dynamic>(context);
            return strategy;
        }
    }
}
