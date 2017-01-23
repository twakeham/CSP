using System;

namespace CSP {
    public static class ArrayExtensions {

        /// Extension method adding Row to 2D arrays to return row at index
        public static T[] Row<T> (this T[,] array, int index) {
            int width = array.GetLength(1);
            T[] row = new T[width];
            for (int i = 0; i < width; i++) {
                row[i] = array[index, i];
            }
            return row;
        }

        /// Extension method adding Column to 2D arrays to return column at index
        public static T[] Column<T> (this T[,] array, int index) {
            int height = array.GetLength(0);
            T[] col = new T[height];
            for (int i = 0; i < height; i++) {
                col[i] = array[i, index];
            }
            return col;
        }

        // extension method for 2d array slices
        public static T[,] Slice<T> (this T[,] array, int startCol, int startRow, int endCol, int endRow) {
            // check bounds
            if (startCol < 0 || startRow < 0 || endCol >= array.GetLength(1) || endRow >= array.GetLength(0))
                throw new IndexOutOfRangeException(string.Format("Array Slice index out of range - {0}:{1} - {2}:{3}", startCol, startRow, endCol, endRow));

            // swap start and end if required so stride is positive
            if (startCol > endCol) {
                int _ = endCol;
                endCol = startCol;
                startCol = _;
            }

            int columns = endCol - startCol;

            if (startRow > endRow) {
                int _ = endRow;
                endRow = startRow;
                startRow = _;
            }

            int rows = endRow - startRow;

            T[,] slice = new T[rows, columns];

            for (int row = 0; row < rows; row++) {
                for (int col = 0; col < columns; col++) {
                    slice[row, col] = array[startRow + row, startCol + col];
                }
            }

            return slice;
        }

        // extension method creates a 1D array in row major form from a 2D array
        public static T[] Flatten<T> (this T[,] array) {
            int width = array.GetLength(1);
            int height = array.GetLength(0);

            T[] flat = new T[width * height];

            for (int row = 0; row < height; row++) {
                for (int col = 0; col < width; col++) {
                    flat[row * width + col] = array[row, col];
                }
            }

            return flat;
        }

        public static T[] Map<T> (this T[] array, Func<T[], T, int, T> callback) {
            T[] mapped = new T[array.Length];

            for (int i = 0; i < array.Length; i ++) {
                mapped[i] = callback(array, array[i], i);
            }

            return mapped;
        }
    }
}
