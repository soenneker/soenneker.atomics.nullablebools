using System.Runtime.CompilerServices;
using System.Threading;
using Soenneker.Atomics.NullableBools.Abstract;

namespace Soenneker.Atomics.NullableBools;

/// <inheritdoc cref="IAtomicNullableBool"/>
public sealed class AtomicNullableBool : IAtomicNullableBool
{
    private const int _null = -1;
    private const int _false = 0;
    private const int _true = 1;
    private int _state;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AtomicNullableBool() => _state = _null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AtomicNullableBool(bool initialValue) => _state = initialValue ? _true : _false;

    public bool HasValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _state) != _null;
    }

    public bool? Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            int s = Volatile.Read(ref _state);
            return s == _null ? null : s == _true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetValueOrFalse()
    {
        int s = Volatile.Read(ref _state);
        return s == _true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetValueOrTrue()
    {
        int s = Volatile.Read(ref _state);
        return s != _false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(bool value) => Interlocked.Exchange(ref _state, value ? _true : _false);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySet(bool value) => Interlocked.CompareExchange(ref _state, value ? _true : _false, _null) == _null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset() => Interlocked.Exchange(ref _state, _null);

    public override string ToString()
    {
        int s = Volatile.Read(ref _state);
        return s switch { _null => "null", _true => "true", _ => "false" };
    }
}