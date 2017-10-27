using System.Collections.Generic;

namespace KeyedLocks
{
    public class NamedLock : KeyedLock<string>
    {
        public NamedLock()
        {
        }

        public NamedLock(IEqualityComparer<string> comparer) : base(comparer)
        {
        }
    }
}