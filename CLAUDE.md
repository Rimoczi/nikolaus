# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Run Commands

```bash
# Build the project
dotnet build nikolaus.sln

# Run the application
dotnet run --project nikolaus/nikolaus.csproj
```

## Project Overview

This is a .NET 9.0 console application that solves the "House of Nikolaus" puzzle (German: "Das ist das Haus vom Nikolaus"). The puzzle requires drawing a house shape using 8 lines without lifting the pen and without retracing any line - an Eulerian path problem.

## Architecture

The solution uses a brute-force graph traversal approach:

- **PointsEnum**: Defines the 5 vertices of the house (LB=Left Bottom, RB=Right Bottom, LT=Left Top, RT=Right Top, T=Top)
- **Line**: Represents an edge between two points
- **House**: Static class defining all 8 edges of the house graph and providing adjacency queries
- **Drawing**: Tracks a single drawing attempt - the path taken, current position, and success state. A drawing succeeds when all 8 unique lines are drawn
- **Draw**: Orchestrates the search by recursively exploring all possible paths from each starting point, branching when multiple undrawn lines are available

The algorithm starts from each of the 5 vertices and recursively tries all possible next moves, creating new Drawing branches when multiple options exist.
