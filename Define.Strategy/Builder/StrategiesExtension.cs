using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;
using Optional;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public static class StrategiesExtension
    {
        public static SubstituteBehaviorStrategy WithContext(this SubstituteBehaviorStrategy strategy, dynamic context)
        {
            strategy.Context = Option.Some<dynamic>(context);
            return strategy;
        }

        public static ObjectStrategy<T> WithContext<T>(this ObjectStrategy<T> strategy, dynamic context)
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
