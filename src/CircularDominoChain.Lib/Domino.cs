using System.Diagnostics.CodeAnalysis;

namespace CircularDominoChain.Lib;

public readonly struct Domino
{
    public const int MAX_VALUE = 6;

    public Domino(int side1, int side2)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(side1, MAX_VALUE);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(side2, MAX_VALUE);
        Side1 = side1;
        Side2 = side2;
    }

    public static bool operator ==(Domino d1, Domino d2)
    {
        return d1.Side1 == d2.Side1 && d1.Side2 == d2.Side2
            || d1.Side1 == d2.Side2 && d1.Side2 == d2.Side1;
    }

    public static bool operator !=(Domino d1, Domino d2)
    {
        return !(d1 == d2);
    }

    public int Side1 { get; }

    public int Side2 { get; }

    public bool CanLinkTo(Domino other)
    {
        return Side2 == other.Side1;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Domino other && this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Math.Min(Side1, Side2), Math.Max(Side1, Side2));
    }
}
