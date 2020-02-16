using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.IO;

namespace WpfGamePuzzle
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            objectGameCreate = new ClassGameCreate(Directory.GetCurrentDirectory());
            objectGameLogic = new ClassGameLogic();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textB = (sender as Button).Content as TextBlock;
            if (textB.Text.Equals("Search"))
            {
                try
                {
                    OpenFileDialog dialogFile = new OpenFileDialog();
                    dialogFile.InitialDirectory = objectGameCreate.PathImage;
                    dialogFile.Filter = "All Files (*.*)|*.*"; //"Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                    dialogFile.RestoreDirectory = true;
                    if (dialogFile.ShowDialog().HasValue)
                    {
                        ImageViewer1.Source = objectGameCreate.CreateBitmap(dialogFile.FileName);
                        textFileName.Text = System.IO.Path.GetFileName(dialogFile.FileName);
                        textCreateGame.Visibility = createGameButton.Visibility =
                           textCreateGame2.Visibility = textRows.Visibility = textColumns.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } else
            {
                objectGameLogic.ShuffleElements();
            }
        }

        private void TextCreateGame_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox objectText = sender as TextBox;
            if (objectText == null || string.IsNullOrEmpty(objectText.Text))
                return;
            if (Regex.IsMatch(objectText.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                objectText.Text = objectText.Text.Remove(objectText.Text.Length - 1);
            }
            else
            {
                short actualValue = short.Parse(objectText.Text);
                if (!(actualValue >= 3 && actualValue < 9))
                {
                    MessageBox.Show("Value out of range");
                    objectText.Text = objectText.Text.Remove(objectText.Text.Length - 1);
                }
            }
        }

        private void TextCreateGame_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox objectText = sender as TextBox;
            if (objectText == null)
                return;
            if (!string.IsNullOrEmpty(objectText.Text) && e.Key == Key.Enter)
            {
                if (objectText.Name.Equals("textCreateGame"))
                    objectGameLogic.CountRow = byte.Parse(objectText.Text);
                else
                    objectGameLogic.CountColumns = byte.Parse(objectText.Text);

                if (objectGameLogic.CountRow != 0 && objectGameLogic.CountColumns != 0)
                {
                    objectGameLogic.ClearGame(gridImageViewer);
                    objectGameLogic.CreateGame(gridImageViewer, objectGameCreate, ImageViewer1);
                    browseButtonText.Text = "Start";
                }
            }
        }

        private void CreateGameButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(textCreateGame.Text) && !string.IsNullOrEmpty(textCreateGame2.Text))
            {
                objectGameLogic.CountRow = byte.Parse(textCreateGame.Text);
                objectGameLogic.CountColumns = byte.Parse(textCreateGame2.Text);

                objectGameLogic.ClearGame(gridImageViewer);
                objectGameLogic.CreateGame(gridImageViewer, objectGameCreate, ImageViewer1);
                browseButtonText.Text = "Start";
            }
            else
                MessageBox.Show("Empty value");
        }

        private void ImageViewer1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image ob = sender as Image;
            if (ob == null)
                return;
            ob.Focus();
            MessageBox.Show("control version");
        }

        private ClassGameLogic objectGameLogic;
        private ClassGameCreate objectGameCreate;
    }
}
