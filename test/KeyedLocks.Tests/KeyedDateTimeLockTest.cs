using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KeyedLocks.Tests
{
    public class KeyedDateTimeLockTest : TestBase
    {
        [Fact]
        public void RunWithLock_UniqueObjectKeys_HasNoCollision()
        {
            // Arrange
            var key1 = new DateTime(2017, 1, 2, 3, 4, 5);
            var key2 = new DateTime(2017, 1, 2, 3, 4, 5);
            var key3 = new DateTime(2017, 1, 2, 3, 4, 5);
            var keyedLock = new KeyedLock<DateTime>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => keyedLock.RunWithLock(key, () =>
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
            var key = new DateTime(2017, 1, 2, 3, 4, 5);
            var keyedLock = new KeyedLock<DateTime>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, _ => keyedLock.RunWithLock(key, () =>
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
            var key1 = new DateTime(2017, 1, 2, 3, 4, 5);
            var key2 = new DateTime(2017, 1, 2, 3, 4, 5);
            var key3 = new DateTime(2017, 1, 2, 3, 4, 5);
            var keyedLock = new KeyedLock<DateTime>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key =>
            {
                var _ = keyedLock.RunWithLock(key, () =>
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
            var key1 = new DateTime(2017, 1, 2, 3, 4, 5);
            var key2 = new DateTime(2016, 3, 4, 5, 6, 7);
            var key3 = new DateTime(2015, 5, 6, 7, 8, 9);
            var keyedLock = new KeyedLock<DateTime>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] { key1, key2, key3 }, key =>
            {
                var _ = keyedLock.RunWithLock(key, () =>
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
            var key = new DateTime(2017, 1, 2, 3, 4, 5);
            var keyedLock = new KeyedLock<DateTime>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, i =>
            {
                var _ = keyedLock.RunWithLock(key, () =>
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