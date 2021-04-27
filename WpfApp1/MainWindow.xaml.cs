using ClassLibrary1;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private V5MainCollection Data = new V5MainCollection();

        /* 1. Service Methods */
        public MainWindow()
        {
            InitializeComponent();
            InitializeDataContext();
        }

        private void UpdateSaveFlag()
        {
            if (Data.AddChangesAfterSave)
            {
                SaveFlag.Foreground = Brushes.Yellow;
                SaveFlag.Text = "There are unsaved changes";
            }
            else
            {
                SaveFlag.Foreground = Brushes.Green;
                SaveFlag.Text = "All changes saved";
            }
        }

        private bool WantSaveData()
        {
            return (MessageBox.Show("Do you want to save changes?", "", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes);
        }

        private void SaveData()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Serialized Data|*.sd|All|*.*";
            dialog.FilterIndex = 2;

            if ((bool)dialog.ShowDialog())
            {
                Data.Save(dialog.FileName);
            }
        }

        private void InitializeDataContext()
        {
            DataContext = Data;
            UpdateSaveFlag();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            if (Data.AddChangesAfterSave && WantSaveData())
            {
                SaveData();
            }
        }

        private void Filter_DataOnGrid(object sender, FilterEventArgs args)
        {
            args.Accepted = args.Item is V5DataOnGrid;
        }

        private void Filter_DataCollection(object sender, FilterEventArgs args)
        {
            args.Accepted = args.Item is V5DataCollection;
        }


        /* 2. Menu->File Methods */
        private void New(object sender, RoutedEventArgs e)
        {
            if (Data.AddChangesAfterSave && WantSaveData())
            {
                SaveData();
            }
            Data = new V5MainCollection();
            InitializeDataContext();
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            if (Data.AddChangesAfterSave && WantSaveData())
            {
                SaveData();
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Serialization data|*.sd|All|*.*";
            dialog.FilterIndex = 2;

            if ((bool)dialog.ShowDialog())
            {
                Data = new V5MainCollection();
                try
                {
                    Data.Load(dialog.FileName);
                }
                catch (Exception exeption)
                {
                    MessageBox.Show(
                    exeption.Message,
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
                InitializeDataContext();
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            SaveData();
            UpdateSaveFlag();
        }

        /* 3. Menu->Edit Methods */
        private void AddDefaults(object sender, RoutedEventArgs e)
        {
            Data.AddDefaults();
            UpdateSaveFlag();
        }

        private void AddDefaultV5DataCollection(object sender, RoutedEventArgs e)
        {
            Data.AddOneDefaultColection(3);
            UpdateSaveFlag();
        }

        private void AddDefaultV5DataOnGrid(object sender, RoutedEventArgs e)
        {
            Data.AddOneDefaultGrid(new Grid2D(1, 1, 1.0f, 1.0f));
            UpdateSaveFlag();
        }

        private void AddElementFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Serialization data|*.sd|All|*.*";
            dialog.FilterIndex = 2;

            if ((bool)dialog.ShowDialog())
            {
                try
                {
                    Data.AddOneFileGrid(dialog.FileName);
                }
                catch (Exception exeption)
                {
                    MessageBox.Show(
                    exeption.Message,
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning,
                    MessageBoxResult.OK,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
                UpdateSaveFlag();
            }
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            var select_data = (sender as MenuItem).DataContext;
            if (select_data != null)
            {
                Data.RemoveElement(select_data as V5Data);
                UpdateSaveFlag();
            }
        }
    }
}
