using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KeyedLocks.Tests
{
    public class NamedLockTest : TestBase
    {
        [Fact]
        public void RunWithLock_ComparerConstructorDifferentCasedKey_HasNoCollision()
        {
            // Arrange
            var key1 = "Key";
            var key2 = "KEY";
            var key3 = "key";
            var namedLock = new NamedLock(StringComparer.InvariantCultureIgnoreCase);

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => namedLock.RunWithLock(key, () =>
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
        public void RunWithLock_DefaultConstructorDifferentCasedKey_HasCollision()
        {
            // Arrange
            var key1 = "Key";
            var key2 = "KEY";
            var key3 = "key";
            var namedLock = new NamedLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => namedLock.RunWithLock(key, () =>
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
        public void RunWithLock_IdenticalLookingKey_HasNoCollision()
        {
            // Arrange
            var key1 = "key";
            var key2 = "key";
            var key3 = "key";
            var namedLock = new NamedLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => namedLock.RunWithLock(key, () =>
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
        public void RunWithLock_SameKey_HasNoCollision()
        {
            // Arrange
            var key = "key";
            var namedLock = new NamedLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, _ => namedLock.RunWithLock(key, () =>
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
        public void RunWithLockOfTResult_IdenticalLookingKey_HasNoCollision()
        {
            // Arrange
            var key1 = "key";
            var key2 = "key";
            var key3 = "key";
            var namedLock = new NamedLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key =>
            {
                var _ = namedLock.RunWithLock(key, () =>
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
        public void RunWithLockOfTResult_SameKey_HasNoCollision()
        {
            // Arrange
            var key = "key";
            var namedLock = new NamedLock();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, i =>
            {
                var _ = namedLock.RunWithLock(key, () =>
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