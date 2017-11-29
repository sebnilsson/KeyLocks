using System.Collections.Generic;

namespace KeyLocks
{
    public class NameLock : KeyLock<string>
    {
        public NameLock()
        {
        }

        public NameLock(IEqualityComparer<string> comparer) : base(comparer)
        {
        }
    }
}