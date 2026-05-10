using FileEmpty;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphicUserInterface
{
    /// <summary>
    /// Interaction logic for ManualInputWindow.xaml
    /// </summary>
    public partial class ManualInputWindow : Window
    {
        /// <summary>
        /// Gets the parsed and validated 2D adjacency matrix resulting from the user's input. 
        /// Returns null if the input has not yet been successfully processed.
        /// </summary>
        public int[,]? ResultMatrix { get; private set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualInputWindow"/> class.
        /// </summary>
        public ManualInputWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the confirmation button click. Validates the inputted graph size, 
        /// triggers the matrix parsing process, and handles any resulting formatting errors by alerting the user.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            string sizeText = MatrixSizeInput.Text;
            if (!int.TryParse(sizeText, out int size))
            {
                MessageBox.Show("Invalid matrix size entered. Please enter a number");
                return;
            }

            if (size <= 0 || size > MainWindow.MaxGraphSize)
            {
                MessageBox.Show("The size must be between 1 and 20. Try again.");
                return;
            }

            try
            {
                this.ResultMatrix = ReadMatrix(size);
            }
            catch (FormatException fex)
            {
                MessageBox.Show($"Invalid matrix format: {fex.Message}");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Matrix error: {ex.Message}");
                return;
            }

            this.DialogResult = true;
        }

        /// <summary>
        /// Parses the raw multi-line string input from the UI text box into a structured 2D integer array 
        /// representing an adjacency matrix. Rigorously validates graph rules.
        /// </summary>
        /// <param name="size">The expected number of rows and columns (vertices) in the matrix.</param>
        /// <returns>A validated, symmetrical 2D integer array representing the graph's connections.</returns>
        /// <exception cref="FormatException">
        /// Thrown under the following conditions:
        /// - The input field is empty.
        /// - The number of rows does not match the specified <paramref name="size"/>.
        /// - The matrix is not perfectly square.
        /// - The elements contain non-integer characters or values other than 0 and 1.
        /// - The main diagonal contains non-zero elements (self-loops are not allowed).
        /// - The matrix is not symmetrical across the main diagonal (undirected graph requirement).
        /// </exception>
        private int[,] ReadMatrix(int size)
        {
            var res = MatrixInput.Text.Trim();

            if (string.IsNullOrEmpty(res) || string.IsNullOrWhiteSpace(res))
            {
                throw new FormatException("The matrix is not entered");
            }

            var lines = res.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length != size)
            {
                throw new FormatException($"The number of rows ({lines.Length}) does not match the specified size ({size}).");
            }

            foreach (var line in lines)
            {
                if (line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length != size)
                {
                    throw new FormatException($"The matrix is not in a correct format. Each row must contain exactly {size} elements (square matrix required)");
                }
            }

            int[,] matrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                var lineOfNumbers = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < lineOfNumbers.Length; j++)
                {
                    int number;
                    if (!int.TryParse(lineOfNumbers[j], out number))
                    {
                        throw new FormatException("The element of matrix is not in a correct format");
                    }

                    if (number != 0 && number != 1)
                    {
                        throw new FormatException("The element of adjacency matrix must be 0 or 1");
                    }

                    matrix[i, j] = number;
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, i] != 0)
                {
                    throw new FormatException("The main diagonal of the matrix must have only 0. No loops allowed");
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = i; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                    {
                        throw new FormatException("The matrix must be symmetrical");
                    }
                }
            }

            return matrix;
        }
    }
}
