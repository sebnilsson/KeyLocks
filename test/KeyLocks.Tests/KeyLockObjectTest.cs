using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KeyLocks.Tests
{
    public class KeyLockObjectTest : TestBase
    {
        [Fact]
        public void RunWithLock_UniqueObjectKeys_HasCollision()
        {
            // Arrange
            var key1 = new { Key1 = "Key1" };
            var key2 = new { Key2 = "Key2" };
            var key3 = new { Key3 = "Key3" };
            var keyLock = new KeyLock<object>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new object[] {key1, key2, key3}, key => keyLock.RunWithLock(key, () =>
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
        public void RunWithLock_SameInstanceKey_HasNoCollision()
        {
            // Arrange
            var key = new object();
            var keyLock = new KeyLock<object>();

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
        public void RunWithLockOfTResult_UniqueObjectKeys_HasCollision()
        {
            // Arrange
            var key1 = new { Key1 = "Key1" };
            var key2 = new { Key2 = "Key2" };
            var key3 = new { Key3 = "Key3" };
            var keyLock = new KeyLock<object>();

            var isRunning = false;
            var hasCollision = false;

            // Act
            Parallel.ForEach(new object[] {key1, key2, key3}, key =>
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
        public void RunWithLockOfTResult_SameInstanceKey_HasNoCollision()
        {
            // Arrange
            var key = new object();
            var keyLock = new KeyLock<object>();

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
    }
}