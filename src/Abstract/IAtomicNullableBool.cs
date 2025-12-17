namespace Soenneker.Atomics.NullableBools.Abstract;

/// <summary>
/// A thread-safe nullable boolean backed by atomic operations.
/// </summary>
public interface IAtomicNullableBool
{
    /// <summary>
    /// Indicates whether the value has been set (true or false).
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// Gets the current value, or null if unset.
    /// </summary>
    bool? Value { get; }

    /// <summary>
    /// Gets the value, defaulting to false if unset.
    /// </summary>
    bool GetValueOrFalse();

    /// <summary>
    /// Gets the value, defaulting to true if unset.
    /// </summary>
    bool GetValueOrTrue();

    /// <summary>
    /// Atomically sets the value.
    /// </summary>
    void Set(bool value);

    /// <summary>
    /// Attempts to set the value only if it is currently unset.
    /// </summary>
    /// <returns>true if the value was set; otherwise false</returns>
    bool TrySet(bool value);

    /// <summary>
    /// Resets the value back to null.
    /// </summary>
    void Reset();
}