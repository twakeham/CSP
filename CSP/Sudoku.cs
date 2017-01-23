using System;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;


namespace CSP {
    public class Sudoku {

        private int[,] _puzzle;
        private int _degree;

        public SolverStats Stats { get; set; }

        public Sudoku (int[,] puzzle) {
            _puzzle = puzzle;
            _degree = 3;
        }

        public Sudoku (int[,] puzzle, int degree) {
            _puzzle = puzzle;
            _degree = degree;
        }

        public List<int[,]> Solve () {

            int dimension = _degree * _degree;
            Solver solver = new Solver("Sudoku");

            IntVar[,] cells = solver.MakeIntVarMatrix(dimension, dimension, 1, 9, "cells");

            // constrain initial values
            for (int row = 0; row < dimension; row++) {
                for (int col = 0; col < dimension; col++) {
                    if (_puzzle[row, col] != 0)
                        solver.Add(cells[row, col] == _puzzle[row, col]);
                }
            }

            // constrain rows and columns
            for (int i = 0; i < dimension; i++) {
                solver.Add(cells.Column<IntVar>(i).AllDifferent());
                solver.Add(cells.Row<IntVar>(i).AllDifferent());
            }

            // constrain cells to be all different
            for (int row = 0; row < dimension; row += _degree) {
                for (int col = 0; col < dimension; col += _degree) {
                    IntVar[] subGrid = new IntVar[dimension];
                    for (int i = 0; i < _degree; i++) {
                        for (int j = 0; j < _degree; j++) {
                            subGrid[i * _degree + j] = cells[row + i, col + j];
                        }
                    }
                    solver.Add(subGrid.AllDifferent());
                }
            }

            DecisionBuilder builder = solver.MakePhase(cells.Flatten<IntVar>(), Solver.INT_VAR_SIMPLE, Solver.INT_VALUE_SIMPLE);
            solver.NewSearch(builder);

            // put solutions into a nice format
            List<int[,]> solutions = new List<int[,]>();

            while (solver.NextSolution()) {
                int[,] solution = new int[dimension, dimension];

                for (int i = 0; i < dimension; i++) {
                    for (int j = 0; j < dimension; j++) {
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
