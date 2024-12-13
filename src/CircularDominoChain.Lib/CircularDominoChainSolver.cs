using FluentResults;

namespace CircularDominoChain.Lib;

public class CircularDominoChainSolver : ICircularDominoChainSolver
{
    public Task<Result<IEnumerable<Domino>>> SolveAsync(IEnumerable<Domino> dominos, CancellationToken cancellationToken = default)
    {
        var graph = new DominoGraph(dominos);
        var numOfDominos = graph.Count;
        if (numOfDominos == 0)
        {
            return Task.FromResult(Result.Ok(Array.Empty<Domino>() as IEnumerable<Domino>));
        }

        var prevDepth = -1;
        var currentDepth = 0;
        var currentNode = GetStartNode(graph);
        var chain = new Domino[numOfDominos];
        var chainIsSolution = false;
        var stack = new Stack<(int Depth, Domino Node)>();
        stack.Push((currentDepth, currentNode));

        while (stack.Count != 0 && !chainIsSolution)
        {
            (currentDepth, currentNode) = stack.Pop();
            for (int i = currentDepth; i <= prevDepth; ++i)
            {
                graph.Add(chain[i]);
            }

            chain[currentDepth] = currentNode;
            foreach (var neighbor in graph.GetNeighbors(currentNode))
            {
                stack.Push((currentDepth + 1, neighbor));
            }

            graph.Remove(currentNode);
            prevDepth = currentDepth;
            chainIsSolution = currentDepth + 1 == numOfDominos && chain[^1].CanLinkTo(chain[0]);
        }

        if (chainIsSolution)
        {
            return Task.FromResult(Result.Ok(chain as IEnumerable<Domino>));
        }

        return Task.FromResult(Result.Fail<IEnumerable<Domino>>("Cannot determine circular domino chain"));
    }

    private static Domino GetStartNode(DominoGraph graph)
    {
        for (int i = 0; i <= Domino.MAX_VALUE; ++i)
        {
            for (int j = 0; j <= Domino.MAX_VALUE; j++)
            {
                var domino = new Domino(i, j);
                if (graph.Contains(domino))
                {
                    return domino;
                }
            }
        }

        throw new InvalidOperationException("Domino graph is empty");
    }
}
