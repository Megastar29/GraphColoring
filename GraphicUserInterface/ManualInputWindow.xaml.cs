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
        public int[,]? ResultMatrix { get; private set; } = null;

        public ManualInputWindow()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            string sizeText = MatrixSizeInput.Text;
            if (!int.TryParse(sizeText, out int size))
            {
                MessageBox.Show("Invalid matrix size entered. Please enter a number");
                return;
            }

            if (size <= 0 || size > 20)
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
