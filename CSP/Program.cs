using System;


namespace CSP {
    class Program {
        static void Main (string[] args) {

            // -- SUDOKU

            // sudoki puzzle with 1 known solution
            int[,] puzzle = new int[9, 9] {
                {0, 5, 0, 0, 3, 7, 0, 4, 0},
                {0, 0, 0, 2, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 6, 7, 2},
                {0, 3, 4, 9, 0, 0, 0, 0, 0},
                {5, 7, 0, 0, 0, 0, 0, 6, 4},
                {0, 0, 0, 0, 0, 6, 1, 3, 0},
                {2, 9, 3, 5, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 6, 0, 1, 8, 0, 0, 2, 0}
            };

            Sudoku sudoku = new Sudoku(puzzle);
            sudoku.Solve();

            Console.WriteLine("Sudoku Solutions: {0}", sudoku.Stats.NumberSolutions);

            // -- N Queens

            NQueens nqueens = new NQueens();
            nqueens.Solve(8);

            Console.WriteLine("NQueens Solutions: {0}", nqueens.Stats.NumberSolutions);

            // -- Skolem Sequence

            SkolemSequence sequence = new SkolemSequence();
            sequence.Solve(8);

            // Reflections of a sequence are considered valid sequences, so multiply results by 2
            Console.WriteLine("Skolem Solutions: {0}", sequence.Stats.NumberSolutions * 2);

            // -- Magic Square
            MagicSquare magicSquare = new MagicSquare();
            magicSquare.Solve(4);

            Console.WriteLine("Magic Square Solutions: {0}", magicSquare.Stats.NumberSolutions);
        }
    }
}
