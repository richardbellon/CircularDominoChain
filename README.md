# CircularDominoChain

The repository contains a C#/.NET8 based implementation to solve the following problem:

Given a random set of dominos, compute a way to order the set in such a way that they form a correct circular domino chain. For every stone the dots on one half of a stone match the dots on the neighboring half of an adjacent stone. For example given the stones [2|1], [2|3] and [1|3] you should compute something like [1|2] [2|3] [3|1] or [3|2] [2|1] [1|3] or [1|3] [3|2] [2|1] etc, where the first and last numbers are the same meaning they’re in a circle. For stones [1|2], [4|1] and [2|3] the resulting chain is not valid: [4|1] [1|2] [2|3]'s first and last numbers are not the same so it’s not a circle.

Write a program which computes the chain for a random set of dominos. If a circular chain is not possible the program should output this.

## Components

### src/CircularDominoChain.Console

Provides a CLI to run the circular path solver. Run `dotnet -- help` for more information about its usage.

### src/CircularDominoChain.Lib

Contains the implementation of the circular path solver. 

To solve the problem, graph theory was used as a basis. A set of dominos can be represented by an undirected graph where two nodes are linked if at least one of their sides are the same (assuming flipping). Having a graph as such built up we want to find a Hamiltonian circuit inside, if exists.

This solution uses a custom graph representation as well as a slightly modified (DFS) backtracking algorithm to reach its goal.

Some things to look into in the future:
- performance
- memory footprint.

### tests/CircularDominoChain.Lib.Tests

Contains tests targeting the solver. Has baked-in tests as well as generated tests with fixed random seeds.

Issue `dotnet test` from the workspace root to run all implemented test cases.
