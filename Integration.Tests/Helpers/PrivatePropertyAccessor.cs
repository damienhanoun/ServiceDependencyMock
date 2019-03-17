using System.Reflection;

namespace Integration.Tests.Helpers
{
    public static class PrivatePropertyAccessorExtension
    {
        public static T GetPrivateProperty<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType()
                          .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                          .GetValue(obj);
        }

        public static void SetPrivateProperty<T>(this object obj, string propertyName, T value)
        {
            obj.GetType()
               .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
               .SetValue(obj, value);
        }
    }
}
