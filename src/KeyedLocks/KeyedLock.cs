using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KeyedLocks
{
    public class KeyedLock<T>
    {
        protected internal readonly ConcurrentDictionary<T, object> _locks;

        public KeyedLock()
            : this(EqualityComparer<T>.Default)
        {
        }

        public KeyedLock(IEqualityComparer<T> comparer)
        {
            _locks = new ConcurrentDictionary<T, object>(comparer);
        }

        public object GetLock(T key)
        {
            return _locks.GetOrAdd(key, _ => new object());
        }

        public TResult RunWithLock<TResult>(T key, Func<TResult> func)
        {
            lock (_locks.GetOrAdd(key, _ => new object()))
            {
                return func();
            }
        }

        public void RunWithLock(T key, Action action)
        {
            lock (_locks.GetOrAdd(key, _ => new object()))
            {
                action();
            }
        }

        public void RemoveLock(T key)
        {
            object o;
            _locks.TryRemove(key, out o);
        }
    }
}