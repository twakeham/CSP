using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;


namespace CSP {
    class SkolemSequence {

        // Skolem sequence is a set s of n numbers k=1..n of length 2n where
        // si = s(i+k) = k
        // Reflections are considered valid and thus number of solutions should
        // be multiplied by two.

        public SolverStats Stats { get; set; }

        public List<int[]> Solve (int n) {

            Solver solver = new Solver("SkolemSequence");

            int positions = n * 2;

            IntVar[] position = solver.MakeIntVarArray(positions, 0, positions - 1, "Positions");
            IntVar[] sequence = solver.MakeIntVarArray(positions, 1, n, "Sequence");

            // indicies into sequence array, so all different
            solver.Add(position.AllDifferent());

            for (int i = 1; i < n + 1; i ++) {
                // s[i] = s[i+k] = k constraint
                solver.Add(position[n + i - 1] == (position[i - 1] + solver.MakeIntVar(i, i)));
                solver.Add(sequence.Element(position[i - 1]) == i);
                solver.Add(sequence.Element(position[n + i - 1]) == i);
            }

            // symmetry breaking
            solver.Add(sequence[0] < sequence[positions - 1]);

            DecisionBuilder db = solver.MakePhase(position, Solver.CHOOSE_FIRST_UNBOUND, Solver.ASSIGN_MIN_VALUE);
            solver.NewSearch(db);

            List<int[]> solutions = new List<int[]>();

            while (solver.NextSolution()) {
                int[] solution = new int[positions];
                for (int i = 0; i < n * 2; i ++) {
                    solution[i] = (int) sequence[i].Value();
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
