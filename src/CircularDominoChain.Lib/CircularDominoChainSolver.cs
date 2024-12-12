using FluentResults;

namespace CircularDominoChain.Lib;

public class CircularDominoChainSolver : ICircularDominoChainSolver
{
    public Task<Result<IEnumerable<Domino>>> SolveAsync(IEnumerable<Domino> dominos, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
