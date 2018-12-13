using System;

namespace DeveloperTest
{
    public class Guard
    {
        public static void AgainstNullOrEmpty(string value, string error)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException(error);
            }
        }

        public static void AgainstNull(object store, string error)
        {
            if (store == null)
            {
                throw new InvalidOperationException(error);
            }
        }
    }
}