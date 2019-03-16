using Mock.Dependency.With.Proxy.Data.Transfer.Objects.Strategies;

namespace Mock.Dependency.With.Proxy.Define.Strategy
{
    public class MethodToMock
    {
        public string MethodId;

        public SubstituteBehaviorStrategy OnceWithSubstituteBehavior(string strategy)
        {
            return new SubstituteBehaviorStrategy
            {
                MethodId = MethodId,
                BehaviorName = strategy
            };
        }

        public ObjectStrategy<T> OnceWithObject<T>(T mockedObject)
        {
            return new ObjectStrategy<T>
            {
                MethodId = MethodId,
                MockedObject = mockedObject
            };
        }

        public ForceNoMockStrategy OnceWithoutMock()
        {
            return new ForceNoMockStrategy
            {
                MethodId = MethodId
            };
        }

        public SubstituteBehaviorStrategy AlwaysWithSubstituteBehavior(string behaviorName)
        {
            return new SubstituteBehaviorStrategy
            {
                MethodId = MethodId,
                BehaviorName = behaviorName,
                IsAlwaysApplied = true
            };
        }

        public ObjectStrategy<T> AlwaysWithObject<T>(T mockedObject)
        {
            return new ObjectStrategy<T>
            {
                MethodId = MethodId,
                MockedObject = mockedObject,
                IsAlwaysApplied = true
            };
        }
    }
}
