using FluentResults;

namespace CircularDominoChain.Lib;

public interface ICircularDominoChainSolver
{
    Task<Result<IEnumerable<Domino>>> SolveAsync(IEnumerable<Domino> dominos, CancellationToken cancellationToken = default);
}
