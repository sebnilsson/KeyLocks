using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KeyLocks.Tests
{
    public class NameLockTest : TestBase
    {
        [Fact]
        public void RunWithLock_ComparerConstructorDifferentCasedKeys_HasNoCollision()
        {
            // Arrange
            const string key1 = "Key";
            const string key2 = "KEY";
            const string key3 = "key";
            var nameLock = new NameLock(StringComparer.InvariantCultureIgnoreCase);

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => nameLock.RunWithLock(key, () =>
            {
                hasCollision = hasCollision || isRunning;

                isRunning = true;

                Thread.Sleep(SleepTimoutMs);

                isRunning = false;
            }));

            // Assert
            Assert.False(hasCollision);
        }

        [Fact]
        public void RunWithLock_DefaultConstructorDifferentCasedKeys_HasCollision()
        {
            // Arrange
            const string key1 = "Key";
            const string key2 = "KEY";
            const string key3 = "key";
            var nameLock = new NameLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => nameLock.RunWithLock(key, () =>
            {
                hasCollision = hasCollision || isRunning;

                isRunning = true;

                Thread.Sleep(SleepTimoutMs);

                isRunning = false;
            }));

            // Assert
            Assert.True(hasCollision);
        }

        [Fact]
        public void RunWithLock_UniqueObjectSameValueKeys_HasNoCollision()
        {
            // Arrange
            const string key1 = "key";
            const string key2 = "key";
            const string key3 = "key";
            var nameLock = new NameLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => nameLock.RunWithLock(key, () =>
            {
                hasCollision = hasCollision || isRunning;

                isRunning = true;

                Thread.Sleep(SleepTimoutMs);

                isRunning = false;
            }));

            // Assert
            Assert.False(hasCollision);
        }

        [Fact]
        public void RunWithLock_SameInstanceKey_HasNoCollision()
        {
            // Arrange
            const string key = "key";
            var nameLock = new NameLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, _ => nameLock.RunWithLock(key, () =>
            {
                hasCollision = hasCollision || isRunning;

                isRunning = true;

                Thread.Sleep(SleepTimoutMs);

                isRunning = false;
            }));

            // Assert
            Assert.False(hasCollision);
        }

        [Fact]
        public void RunWithLockOfTResult_UniqueObjectKeys_HasNoCollision()
        {
            // Arrange
            const string key1 = "key";
            const string key2 = "key";
            const string key3 = "key";
            var nameLock = new NameLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key =>
            {
                var _ = nameLock.RunWithLock(key, () =>
                {
                    hasCollision = hasCollision || isRunning;

                    isRunning = true;

                    Thread.Sleep(2000);

                    isRunning = false;

                    return true;
                });
            });

            // Assert
            Assert.False(hasCollision);
        }

        [Fact]
        public void RunWithLockOfTResult_UniqueObjectKeys_HasCollision()
        {
            // Arrange
            const string key1 = "key1";
            const string key2 = "key2";
            const string key3 = "key3";
            var nameLock = new NameLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] { key1, key2, key3 }, key =>
            {
                var _ = nameLock.RunWithLock(key, () =>
                {
                    hasCollision = hasCollision || isRunning;

                    isRunning = true;

                    Thread.Sleep(2000);

                    isRunning = false;

                    return true;
                });
            });

            // Assert
            Assert.True(hasCollision);
        }

        [Fact]
        public void RunWithLockOfTResult_SameInstanceKey_HasNoCollision()
        {
            // Arrange
            const string key = "key";
            var nameLock = new NameLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, i =>
            {
                var _ = nameLock.RunWithLock(key, () =>
                {
                    hasCollision = hasCollision || isRunning;

                    isRunning = true;

                    Thread.Sleep(SleepTimoutMs);

                    isRunning = false;

                    return true;
                });
            });

            // Assert
            Assert.False(hasCollision);
        }
    }
}