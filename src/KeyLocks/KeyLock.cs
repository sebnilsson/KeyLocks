using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KeyLocks
{
    public class KeyLock<T>
    {
        protected internal readonly ConcurrentDictionary<T, object> Locks;

        public KeyLock() : this(EqualityComparer<T>.Default)
        {
        }

        public KeyLock(IEqualityComparer<T> comparer)
        {
            Locks = new ConcurrentDictionary<T, object>(comparer);
        }

        public object GetLock(T key)
        {
            return Locks.GetOrAdd(key, _ => new object());
        }

        public TResult RunWithLock<TResult>(T key, Func<TResult> func)
        {
            lock (Locks.GetOrAdd(key, _ => new object()))
            {
                return func();
            }
        }

        public void RunWithLock(T key, Action action)
        {
            lock (Locks.GetOrAdd(key, _ => new object()))
            {
                action();
            }
        }

        public void RemoveLock(T key)
        {
            object o;
            Locks.TryRemove(key, out o);
        }
    }
}