using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;

namespace CSP {
    class NQueens {

        public SolverStats Stats { get; set; }

        public List<int[,]> Solve (int queenCount) {

            Solver solver = new Solver("NQueens");

            // i represents column a queen is on, value of queens[i] is the row
            // this implicitly constrains column to be unique
            IntVar[] queens = solver.MakeIntVarArray(queenCount, 0, queenCount - 1, "Queens");
            solver.Add(queens.AllDifferent());

            // constrain diagonals - row + column or row - column is equal for two queens on a diagonal
            IntVar[] posDiagonal = queens.Map<IntVar>((IntVar[] all, IntVar item, int index) => (all[index] + index).Var());
            solver.Add(posDiagonal.AllDifferent());
            IntVar[] negDiagonal = queens.Map<IntVar>((IntVar[] all, IntVar item, int index) => (all[index] - index).Var());
            solver.Add(negDiagonal.AllDifferent());

            DecisionBuilder db = solver.MakePhase(queens, Solver.CHOOSE_MIN_SIZE_LOWEST_MAX, Solver.ASSIGN_CENTER_VALUE);

            solver.NewSearch(db);

            List<int[,]> solutions = new List<int[,]>();

            while (solver.NextSolution()) {
                int[,] solution = new int[queenCount, 2];
                for (int queen = 0; queen < queenCount; queen++) {
                    solution[queen, 0] = queen;
                    solution[queen, 1] = (int) queens[queen].Value();
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
