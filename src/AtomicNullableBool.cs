using System.Runtime.CompilerServices;
using Soenneker.Atomics.Ints;

namespace Soenneker.Atomics.NullableBools;

/// <summary>
/// A lightweight, allocation-free atomic tri-state flag implemented on top of an inline
/// <see cref="AtomicInt"/>.
/// <para/>
/// Backing values:
/// <list type="bullet">
/// <item><description><c>-1</c> = null / unknown</description></item>
/// <item><description><c>0</c> = false</description></item>
/// <item><description><c>1</c> = true</description></item>
/// </list>
/// </summary>
/// <remarks>
/// <para>
/// Reads establish acquire semantics and writes establish release semantics.
/// </para>
/// <para>
/// This is a mutable <see langword="struct"/> intended for use as a <b>private field</b>
/// or inline synchronization primitive. Avoid copying this type or exposing it publicly.
/// </para>
/// </remarks>
public struct AtomicNullableBool
{
    private const int _null = -1;
    private const int _false = 0;
    private const int _true = 1;

    private AtomicInt _state;

    /// <summary>
    /// Initializes a new instance in the <c>null</c>/<c>unknown</c> state.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AtomicNullableBool()
        => _state = new AtomicInt(_null);

    /// <summary>
    /// Initializes a new instance with the specified initial value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AtomicNullableBool(bool initialValue)
        => _state = new AtomicInt(initialValue ? _true : _false);

    /// <summary>
    /// Gets a value indicating whether the current state is non-null.
    /// </summary>
    public bool HasValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _state.Read() != _null;
    }

    /// <summary>
    /// Gets the current value as a nullable boolean.
    /// </summary>
    public bool? Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            int s = _state.Read();
            return s == _null ? null : s == _true;
        }
    }

    /// <summary>
    /// Reads the raw backing state.
    /// </summary>
    /// <returns>
    /// <c>-1</c> (null), <c>0</c> (false), or <c>1</c> (true).
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Read() => _state.Read();

    /// <summary>
    /// Writes a raw backing state.
    /// </summary>
    /// <remarks>
    /// Callers must only provide valid values: <c>-1</c>, <c>0</c>, or <c>1</c>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(int state) => _state.Write(state);

    /// <summary>
    /// Gets the value, treating <c>null</c>/<c>unknown</c> as <see langword="false"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetValueOrFalse() => _state.Read() == _true;

    /// <summary>
    /// Gets the value, treating <c>null</c>/<c>unknown</c> as <see langword="true"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetValueOrTrue() => _state.Read() != _false;

    /// <summary>
    /// Sets the state to <see langword="true"/> or <see langword="false"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(bool value)
        => _state.Write(value ? _true : _false);

    /// <summary>
    /// Attempts to set the state to <see langword="true"/> or <see langword="false"/>
    /// only if the current state is <c>null</c>/<c>unknown</c>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySet(bool value)
        => _state.CompareExchange(value ? _true : _false, _null) == _null;

    /// <summary>
    /// Attempts to transition the state from <paramref name="expected"/> to
    /// <paramref name="newState"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryCompareExchange(int newState, int expected)
        => _state.CompareExchange(newState, expected) == expected;

    /// <summary>
    /// Resets the state to <c>null</c>/<c>unknown</c>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => _state.Write(_null);

    /// <summary>
    /// Returns a string representation of the current state.
    /// </summary>
    public override string ToString()
        => _state.Read() switch
        {
            _null => "null",
            _true => "true",
            _ => "false"
        };
}
