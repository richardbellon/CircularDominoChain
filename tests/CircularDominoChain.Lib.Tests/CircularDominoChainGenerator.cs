using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CircularDominoChain.Lib.Tests;

public static class CircularDominoChainGenerator
{
    public static IEnumerable<Domino> Generate(int randomSeed, long cardinality)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(cardinality, 0);

        if (cardinality == 0)
        {
            yield break;
        }

        var random = new Random(randomSeed);
        var firstValue = random.Next(Domino.MAX_VALUE);
        var side1 = firstValue;
        int side2;

        for (long i = 1; i < cardinality; ++i)
        {
            side2 = random.Next(Domino.MAX_VALUE);
            yield return new(side1, side2);
            side1 = side2;
        }

        yield return new(side1, firstValue);
    }
}
