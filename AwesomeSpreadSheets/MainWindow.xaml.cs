using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;

namespace AwesomeSpreadSheets
{
    public partial class MainWindow : Window
    {
        private DataTable spreadsheetTable;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpreadsheet();
        }

        private void InitializeSpreadsheet()
        {
            spreadsheetTable = new DataTable();

            // Add default 10 columns
            for (int i = 0; i < 10; i++)
                spreadsheetTable.Columns.Add($"Col {i + 1}");

            // Add default 20 rows
            for (int i = 0; i < 20; i++)
                spreadsheetTable.Rows.Add(spreadsheetTable.NewRow());

            SpreadsheetGrid.ItemsSource = spreadsheetTable.DefaultView;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            InitializeSpreadsheet();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV Files (*.csv)|*.csv" };
            if (ofd.ShowDialog() == true)
            {
                var lines = File.ReadAllLines(ofd.FileName);
                spreadsheetTable = new DataTable();

                string[] headers = lines[0].Split(',');
                foreach (string header in headers)
                    spreadsheetTable.Columns.Add(header);

                for (int i = 1; i < lines.Length; i++)
                    spreadsheetTable.Rows.Add(lines[i].Split(','));

                SpreadsheetGrid.ItemsSource = spreadsheetTable.DefaultView;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV Files (*.csv)|*.csv" };
            if (sfd.ShowDialog() == true)
            {
                StringBuilder sb = new StringBuilder();

                // Headers
                for (int i = 0; i < spreadsheetTable.Columns.Count; i++)
                {
                    sb.Append(spreadsheetTable.Columns[i].ColumnName);
                    if (i < spreadsheetTable.Columns.Count - 1) sb.Append(",");
                }
                sb.AppendLine();

                // Data
                foreach (DataRow row in spreadsheetTable.Rows)
                {
                    for (int i = 0; i < spreadsheetTable.Columns.Count; i++)
                    {
                        sb.Append(row[i]?.ToString());
                        if (i < spreadsheetTable.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(sfd.FileName, sb.ToString());
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.ShowDialog();
        }
    

private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
