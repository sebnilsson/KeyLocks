using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KeyLocks.Tests
{
    public class KeyLockIntTest : TestBase
    {
        [Fact]
        public void RunWithLock_SameInstanceKey_HasNoCollision()
        {
            // Arrange
            const int key = 123;
            var keyLock = new KeyLock<int>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, _ => keyLock.RunWithLock(key, () =>
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
        public void RunWithLock_UniqueObjectKeys_HasNoCollision()
        {
            // Arrange
            const int key1 = 123;
            const int key2 = 122 + 1;
            const int key3 = 121 + 2;
            var keyLock = new KeyLock<int>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key => keyLock.RunWithLock(key, () =>
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
        public void RunWithLockOfTResult_SameInstanceKey_HasNoCollision()
        {
            // Arrange
            const int key = 123;
            var keyLock = new KeyLock<int>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.For(0, ParallelRunCount, i =>
            {
                var _ = keyLock.RunWithLock(key, () =>
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

        [Fact]
        public void RunWithLockOfTResult_UniqueObjectKeys_HasCollision()
        {
            // Arrange
            const int key1 = 123;
            const int key2 = 12345;
            const int key3 = 1234567;
            var keyLock = new KeyLock<int>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key =>
            {
                var _ = keyLock.RunWithLock(key, () =>
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
        public void RunWithLockOfTResult_UniqueObjectKeys_HasNoCollision()
        {
            // Arrange
            const int key1 = 123;
            const int key2 = 122 + 1;
            const int key3 = 121 + 2;
            var keyLock = new KeyLock<int>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new[] {key1, key2, key3}, key =>
            {
                var _ = keyLock.RunWithLock(key, () =>
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
    }
}