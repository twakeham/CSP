using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;

namespace CSP {
    class MagicSquare {

        public SolverStats Stats { get; set; }

        public List<int[,]> Solve (int degree=3) {

            int numberCells = degree * degree;
            Solver solver = new Solver("MagicSquare");

            IntVar sum = solver.MakeIntVar(1, degree * degree * degree, "sum");
            // sum of rows/columns/diagonals in a magic square is constant for degree n
            solver.Add(sum == degree * (numberCells + 1) / 2);

            IntVar[,] cells = solver.MakeIntVarMatrix(degree, degree, 1, numberCells, "cells");
            IntVar[] flat = cells.Flatten<IntVar>();

            solver.Add(flat.AllDifferent());

            IntVar[] diagonal1 = new IntVar[degree];
            IntVar[] diagonal2 = new IntVar[degree];

            for (int i = 0; i < degree; i ++) {
                solver.Add(cells.Row<IntVar>(i).Sum() == sum);
                solver.Add(cells.Column<IntVar>(i).Sum() == sum);
                diagonal1[i] = cells[i, i];
                diagonal2[i] = cells[i, degree - i - 1];
            }

            solver.Add(diagonal1.Sum() == sum);
            solver.Add(diagonal2.Sum() == sum);

            // break symmetry
            solver.Add(cells[0, 0] == 1);

            

            DecisionBuilder builder = solver.MakePhase(flat, Solver.CHOOSE_FIRST_UNBOUND, Solver.ASSIGN_CENTER_VALUE);

            solver.NewSearch(builder);

            List<int[,]> solutions = new List<int[,]>();

            while (solver.NextSolution()) {
                int[,] solution = new int[degree, degree];

                for (int i = 0; i < degree; i++) {
                    for (int j = 0; j < degree; j++) {
                        solution[i, j] = (int) cells[i, j].Value();
                    }
                }
                solutions.Add(solution);
            }

            // save stats
            Stats = new SolverStats() {
                NumberSolutions = solver.Solutions(),
                WallTime = solver.WallTime(),
                Failures = solver.Failures(),
                Branches = solver.Branches()
            };

            return solutions;

        }
    }
}
