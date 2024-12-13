
using System.Diagnostics;

namespace CircularDominoChain.Lib;

internal class DominoGraph
{
    private readonly int[,] _graph = new int[Domino.MAX_VALUE + 1, Domino.MAX_VALUE + 1];

    public DominoGraph()
    {
    }

    public DominoGraph(IEnumerable<Domino> dominos)
    {
        ArgumentNullException.ThrowIfNull(dominos);

        foreach (var domino in dominos)
        {
            Add(domino);
        }
    }

    public int Count { get; private set; }

    public void Add(Domino domino)
    {
        ++_graph[domino.Side1, domino.Side2];
        if (domino.Side1 != domino.Side2)
        {
            ++_graph[domino.Side2, domino.Side1];
        }
        ++Count;
    }

    public void Remove(Domino domino)
    {
        --_graph[domino.Side1, domino.Side2];
        if (domino.Side1 != domino.Side2)
        {
            --_graph[domino.Side2, domino.Side1];
        }
        --Count;

        Debug.Assert(_graph[domino.Side1, domino.Side2] >= 0);
        Debug.Assert(Count >= 0);
    }

    public bool Contains(Domino domino)
    {
        return _graph[domino.Side1, domino.Side2] != 0;
    }

    public IEnumerable<Domino> GetNeighbors(Domino domino)
    {
        for (int i = 0; i < Domino.MAX_VALUE; ++i)
        {
            if (domino.Side1 == i)
            {
                if (_graph[domino.Side2, i] > 1)
                {
                    yield return new(domino.Side2, i);
                }
            }
            else if (_graph[domino.Side2, i] != 0)
            {
                yield return new(domino.Side2, i);
            }
        }
    }

    public DominoGraph Copy()
    {
        var copy = new DominoGraph();
        Array.Copy(_graph, copy._graph, _graph.Length);
        copy.Count = Count;
        return copy;
    }
}
