using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using FileMenagingClass;
using Graphs;

namespace GraphicUserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AlgorithmNameInitText = "Algorithm name: ";
        private const string TimeInitText = "Time: ";
        private const string PractDifInitText = "Practical difficulty: ";
        private const string StartTime = "--";
        private const string StartPracDif = "--";
        private const int AlgSelectionNoneIndex = 3;
        private const int InputSelectionNoneIndex = 2;
        private Graph MainGraph = new Graph();

        public MainWindow()
        {
            InitializeComponent();

            AlgorithmNameText.Text += (AlgSelection.Items[AlgSelectionNoneIndex] as ComboBoxItem)?.Content.ToString();
            TimeText.Text += StartTime;
            PractDifText.Text += StartPracDif;
        }

        private void AlgSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgorithmNameText is null)
            {
                return;
            }

            AlgorithmNameText.Text = AlgorithmNameInitText + (AlgSelection.SelectedItem as ComboBoxItem)?.Content.ToString();
        }

        private void InputSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InputSelection is null || (InputSelection.SelectedItem as ComboBoxItem) == (InputSelection.Items[InputSelectionNoneIndex] as ComboBoxItem))
            {
                return;
            }

            ComboBoxItem selected = InputSelection.SelectedItem as ComboBoxItem;
            if (selected is null)
            {
                return;
            }

            string choice = selected.Content.ToString();

            switch (choice)
            {
                case "From file":
                    HandleFileImport();
                    break;

                case "By hand":
                    HandleManualInput();
                    break;

                default:

                    break;
            }
        }

        private void HandleFileImport()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    int[,] matrix = FileManager.GetDataFromFile(openFileDialog.FileName);
                    MainGraph?.GetGraphFromAdjacencyMatrix(matrix);
                    MessageBox.Show("Matrix loaded successfully!");
                    // Тут логіка малювання графа на Canvas
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                InputSelection.SelectedIndex = 2;
            }
        }

        private void HandleManualInput()
        {
            ManualInputWindow inputWindow = new ManualInputWindow();

            if (inputWindow.ShowDialog() == true)
            {
                var matrix = inputWindow.ResultMatrix;

                if (matrix is null)
                {
                    InputSelection.SelectedIndex = 2;
                    MessageBox.Show("The matrix is empty");
                    return;
                }

                this.MainGraph.GetGraphFromAdjacencyMatrix(matrix);
            }
            else
            {
                InputSelection.SelectedIndex = 2;
                //MessageBox.Show("Matrix is not entered. Please try again");
            }
        }
    }
}