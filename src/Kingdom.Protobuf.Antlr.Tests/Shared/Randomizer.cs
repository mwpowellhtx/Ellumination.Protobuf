using System;

// ReSharper disable once IdentifierTypo
namespace Kingdom.Protobuf
{
    // ReSharper disable once IdentifierTypo
    internal static class Randomizer
    {
        /// <summary>
        /// Provides a convenience <see cref="Random"/> instance for use throughout testing.
        /// </summary>
        internal static Random Rnd { get; } = new Random((int) (DateTime.UtcNow.Ticks % int.MaxValue));
    }
}
