namespace CircularDominoChain.Lib;

public class Domino
{
    public const int MAX_VALUE = 6;

    public Domino(int side1, int side2)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(side1, MAX_VALUE);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(side2, MAX_VALUE);
        Side1 = side1;
        Side2 = side2;
    }

    public int Side1 { get; }

    public int Side2 { get; }

    public bool CanLinkTo(Domino other)
    {
        return Side2 == other.Side1;
    }

    public bool Equals(Domino? other)
    {
        return other is not null
            && (Side1 == other.Side1 && Side2 == other.Side2
                || Side1 == other.Side2 && Side2 == other.Side1);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Domino);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Math.Min(Side1, Side2), Math.Max(Side1, Side2));
    }
}
