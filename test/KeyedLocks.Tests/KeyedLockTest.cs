using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KeyedLocks.Tests
{
    public class KeyedLockTest : TestBase
    {
        [Fact]
        public void RunWithLock_IdenticalLookingKey_HasCollision()
        {
            // Arrange
            var key1 = new object();
            var key2 = new object();
            var key3 = new object();
            var keyedLock = new KeyedLock<object>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key =>
                keyedLock.RunWithLock(key, () =>
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
        public void RunWithLock_SameKey_HasNoCollision()
        {
            // Arrange
            var key = new object();
            var keyedLock = new KeyedLock<object>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, _ =>
                keyedLock.RunWithLock(key, () =>
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
        public void RunWithLockOfTResult_IdenticalLookingKey_HasCollision()
        {
            // Arrange
            var key1 = new object();
            var key2 = new object();
            var key3 = new object();
            var keyedLock = new KeyedLock<object>();

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
            Assert.True(hasCollision);
        }

        [Fact]
        public void RunWithLockOfTResult_SameKey_HasNoCollision()
        {
            // Arrange
            var key = new object();
            var keyedLock = new KeyedLock<object>();

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