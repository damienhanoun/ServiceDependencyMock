using Optional;

namespace Mock.Library
{
    public class ForceNoMockStrategy : MockStrategy
    {
        public ForceNoMockStrategy WithContext(dynamic context)
        {
            this.Context = Option.Some<dynamic>(context);
            return this;
        }
    }
}
