using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace WpfGamePuzzle
{
    class ClassGameCreate
    {

        public string PathImage { get; private set; }

        public ClassGameCreate(string bazowa)
        {
            PathImage = GetPathImage(bazowa);
            arrayBorder = new Border[2] {null, null };
        }

        public Border CreateViewImageDynamically(Image ImageViewer1, double width, double height, double widthC, double heightC, int i, int j)
        {
            Image dynamicImage = CreateFrame(width, height);
            Border dynamicBorder = CreateBorder(width, height, mLeft: width * j, mTop: height * i, element: dynamicImage);
            dynamicImage.Source = CreateBitmapCropped(ImageViewer1, (int)(widthC * j), (int)(heightC *i), (int)widthC, (int)heightC);
            return dynamicBorder;
        }

        public Image CreateFrame(double width, double height, double mLeft = 0, double mTop = 0, 
            VerticalAlignment va = VerticalAlignment.Center, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            Image imageRame = new Image();
            imageRame.Width = width;
            imageRame.Height = height;
            imageRame.VerticalAlignment = va;
            imageRame.HorizontalAlignment = ha;
            imageRame.Stretch = Stretch.Fill;
            imageRame.MouseLeftButtonDown += ImageRame_MouseLeftButtonDown;
            imageRame.MouseRightButtonDown += ImageRame_MouseRightButtonDown;
            if (mLeft != 0 || mTop != 0)
                imageRame.Margin = new Thickness(mLeft, mTop, 0, 0);
            return imageRame;
        }

        private void ImageRame_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(arrayBorder[0] != null && arrayBorder[1] != null)
            {
                ImageSource temp = ((Image)arrayBorder[0].Child).Source; string tempTag = arrayBorder[0].Tag.ToString();
                ((Image)arrayBorder[0].Child).Source = ((Image)arrayBorder[1].Child).Source; arrayBorder[0].Tag = arrayBorder[1].Tag;
                ((Image)arrayBorder[1].Child).Source = temp; arrayBorder[1].Tag = tempTag;
            }
        }

        private void ImageRame_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image objectImage = sender as Image;
            Border objectBorder = objectImage.Parent as Border;
            if (objectBorder.BorderBrush == Brushes.Black)
            {
                objectBorder.BorderBrush = Brushes.DarkOrange;
                setArrayBorder(objectBorder);
            }
            else
            {
                objectBorder.BorderBrush = Brushes.Black;
                removeArrayBorder(objectBorder);
            }              
        }

        public Border CreateBorder(double width, double height, double mLeft, double mTop,
            VerticalAlignment va = VerticalAlignment.Top, HorizontalAlignment ha = HorizontalAlignment.Left, UIElement element = null)
        {
            Border border = new Border();
            border.Background = Brushes.White;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(2);
            border.Width = width;
            border.Height = height;
            border.VerticalAlignment = va;
            border.HorizontalAlignment = ha;
            border.Margin = new Thickness(mLeft, mTop, 0,0);
            if (element != null)
                border.Child = element;
            return border;
        }

        public BitmapImage CreateBitmap(string fileName)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileName);
            bitmap.EndInit();
            return bitmap;
        }

        public CroppedBitmap CreateBitmapCropped(Image imageS, int x, int y, int width, int height)
        {
            if (imageS.Source == null)
                return null;
            CroppedBitmap croppedBitmap = new CroppedBitmap((BitmapSource)imageS.Source, new Int32Rect(x, y, width, height));
            /*double factorX = ((BitmapSource)imageS.Source).PixelWidth / ((BitmapSource)imageS.Source).Width;
            double factorY = ((BitmapSource)imageS.Source).PixelHeight / ((BitmapSource)imageS.Source).Height;
            CroppedBitmap croppedBitmap = new CroppedBitmap((BitmapSource)imageS.Source, new Int32Rect((int)Math.Round(x * factorX), (int)Math.Round(y * factorY), (int)Math.Round(width * factorX), (int)Math.Round(height * factorY)));*/
            return croppedBitmap;
        }

        private string GetPathImage(string bazowa)
        {
            if (bazowa.Contains("bin") || bazowa.Contains("Debug"))
                return GetPathImage(Directory.GetParent(bazowa).FullName);
            return bazowa + @"\Images";
        }

        private void removeArrayBorder(Border border)
        {
            if (arrayBorder[0].Equals(border))
                arrayBorder[0] = null;
            else
                arrayBorder[1] = null;
        }

        private void setArrayBorder(Border border)
        {
            if (arrayBorder[0] == null)
                arrayBorder[0] = border;
            else
            {
                if (arrayBorder[1] != null)
                {
                    arrayBorder[0].BorderBrush = Brushes.Black;
                    arrayBorder[0] = arrayBorder[1];
                }
                arrayBorder[1] = border;
            }
        }

        private Border[] arrayBorder;
    }
}
