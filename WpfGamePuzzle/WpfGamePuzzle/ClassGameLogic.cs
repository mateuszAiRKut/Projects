using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfGamePuzzle
{
    class ClassGameLogic
    {
        public byte CountRow { get; set; }
        public byte CountColumns { get; set; }
        public Border this[int index]
        {
            get
            {
                if (index >= 0 && index < elementsGame.Length)
                    return elementsGame[index];
                throw new Exception("Error index out of range");
            }
        }

        public ClassGameLogic()
        {
        }

        public void CreateGame(Grid gridImageViewer, ClassGameCreate objectCreateGame, Image ImageViewer1)
        {
            double sizeW = gridImageViewer.Width / CountColumns, sizeH = gridImageViewer.Height / CountRow,
                sizeWPix = ImageViewer1.Source.Width / CountColumns, sizeHPix = ImageViewer1.Source.Height / CountRow;
            prepareSettings();
            for (int i = 0, index = 0; i < CountRow; i++)
            {
                for (int j = 0; j < CountColumns; j++, index = (i * CountColumns + j))
                {
                    Border newB = objectCreateGame.CreateViewImageDynamically(ImageViewer1, sizeW, sizeH, sizeWPix, sizeHPix, i, j);
                    newB.Tag = index.ToString();
                    elementsGame[index] = newB;
                    gridImageViewer.Children.Add(newB);
                }
            }
            ShuffleElements();
        }

        public void ClearGame(Grid gridImageViewer)
        {
            if (gridImageViewer.Children.Count != 0)
                gridImageViewer.Children.RemoveRange(0, gridImageViewer.Children.Count);
        }

        public void ShuffleElements()
        {
            ImageSource temp = null; string tempTag = null;
            /*List<byte> listIndex = new List<byte>(elementsGame.Length);
            for (byte i = 0; i < elementsGame.Length; i++)
                listIndex.Add(i);*/
            List<byte> listIndex = new List<byte>(Enumerable.Range(0, elementsGame.Length).Select(x => (byte) x));
            Random rand = new Random();
            //for (int i = 0; i < elementsGame.Length; i++)
                //MessageBox.Show(String.Format("indekes {0} i tag {1}", i, elementsGame[i].Tag));
            for (byte i = (byte)(listIndex.Count / 2), drawIndex = 0, drawIndex2 = 0; i > 0; i--)
            {
                setIndex(ref drawIndex, rand, listIndex);
                setIndex(ref drawIndex2, rand, listIndex);
                changeElementInGame(temp, tempTag, drawIndex, drawIndex2);
            }
            if(listIndex.Count == 1)
            {
                byte drawIndex = 0;
                do
                {
                    drawIndex = (byte)rand.Next(0, elementsGame.Length);
                } while (drawIndex == listIndex[0]);
                changeElementInGame(temp, tempTag, drawIndex, listIndex[0]);
            }
        }

        public void ShuffleElementsAdvanced()
        {
            ImageSource temp; string tempTag;
            byte drawIndex = 0, drawIndex2 = 0, removeIndex = 0, removeIndex2 = 0, flaga = 0;
            HashSet<byte> setN = new HashSet<byte>(), set = new HashSet<byte>();
            HashSet<byte> setI = new HashSet<byte>(Enumerable.Range(0, elementsGame.Length).Select(x => (byte) x));
            Random rand = new Random();
            if(flaga == 0)
            {
                setIndex(ref drawIndex, rand, setI);
                setIndexFar(ref drawIndex2, rand, setI);
            }
            
        }

        private void setIndex(ref byte index, Random rand, List<byte> listE)
        {
            index = listE[rand.Next(0, listE.Count)];
            listE.Remove(index);
        }

        private void setIndex(ref byte index, Random rand, HashSet<byte> listE)
        {
            index = listE.ElementAt(rand.Next(0, listE.Count));
            listE.Remove(index);
        }

        private void setIndexFar(ref byte index, Random rand, HashSet<byte> listE)
        {
            byte copyI = index;
            var collection = listE.Where(x => (Math.Abs(x - copyI) != 1 && Math.Abs(x - copyI) != CountColumns));
            if (collection == null || collection.Count() == 0)
                setIndex(ref index, rand, listE);
            else
            {
                index = collection.ElementAt(rand.Next(0, collection.Count()));
                listE.Remove(index);
            }
        }

        private void changeElementInGame(ImageSource temp, string tempTag, byte drawIndex, byte drawIndex2)
        {
            temp = ((Image)elementsGame[drawIndex].Child).Source; tempTag = elementsGame[drawIndex].Tag.ToString();
            ((Image)elementsGame[drawIndex].Child).Source = ((Image)elementsGame[drawIndex2].Child).Source; elementsGame[drawIndex].Tag = elementsGame[drawIndex2].Tag;
            ((Image)elementsGame[drawIndex2].Child).Source = temp; elementsGame[drawIndex2].Tag = tempTag;
            //MessageBox.Show(String.Format("zamieniono element {0} na element {1}", drawIndex, drawIndex2));
        }

        private void setNeighbour()
        {

        }

        private void setIndexNeighbour(ref byte index, Random rand, HashSet<byte> listE, HashSet<byte> listN)
        {
            if (index >= 1)
                listN.Add((byte)(index - 1));
            if (index >= CountColumns)
                listN.Add((byte)(index - CountColumns));
            if(index <= countAllElements - 1)
                listN.Add((byte)(index + 1));
            if(index <= countAllElements - CountRow)
                listN.Add((byte)(index + CountColumns));
            byte[] exceptA = listE.Except(listN).ToArray();
            if (exceptA != null && exceptA.Length != 0)
            {
                index = exceptA[rand.Next(0, exceptA.Length)];
                listE.Remove(index);
                listN.Clear();
                exceptA = null;
            }
            else
                setIndex(ref index, rand, listE);
        }

        private void prepareSettings()
        {
            if (CountRow != 0 && CountColumns != 0)
            {
                countAllElements = CountRow * CountColumns;
                setArrayElementsGame();
                setArrayElementsCorrectGame();
            }
        }

        private void setArrayElementsGame()
        {
            if (elementsGame == null)
            {
                elementsGame = new Border[countAllElements];
                return;
            }
            for (int i = 0; i < countAllElements; i++)
                elementsGame[i] = null;
        }

        private void setArrayElementsCorrectGame()
        {
            if (elementsCorrectGame == null)
            {
                int capacity = countAllElements / 8 + (countAllElements % 8 == 0 ? 0 : 1);
                elementsCorrectGame = new List<byte>(capacity);
                for (int i = 0; i < capacity; i++)
                    elementsCorrectGame.Add(0);

                /*elementsCorrectGame = new List<byte>(countAllElements);
                for (int i = 0; i < countAllElements; i++)
                    elementsCorrectGame.Add(0);*/
                return;
            }
            for (int i = 0; i < elementsCorrectGame.Count; i++)
                elementsCorrectGame[i] = 0;
        }

        private void checkNeighbor(int element, List<byte> listN)
        {
            if (element + 1 < countAllElements)
                listN.Add(byte.Parse(elementsGame[(byte)(element + 1)].Tag.ToString()));
            if (element + CountColumns < countAllElements)
                listN.Add(byte.Parse(elementsGame[(byte)(element + CountRow)].Tag.ToString()));
            
        }

        private byte checkNeighbor(int indexElement)
        {
            byte variableNeighbor = (byte)(((indexElement + 1 < countAllElements) ? 1 : 0) << 0);
            variableNeighbor |= (byte)(((indexElement + 1 + CountColumns < countAllElements) ? 1 : 0) << 1);
            return variableNeighbor;
        }

        public bool ConditionEndGame()
        {
            for(int i = 0; i < elementsCorrectGame.Count - 1; i++)
            {
                if (elementsCorrectGame[i] != 255)
                    return false;
            }
            for(int i = countAllElements % 8, index = elementsCorrectGame.Count - 1; i >= 0; i--)
            {
                if ((elementsCorrectGame[index] & (1 << i)) == 0)
                    return false;
            }
            return true;         
            //return (elementsCorrectGame.Count == countAllElements);
        }

        public void ChangePositionElement(Border element1, Border element2)
        {
            changeElement(element1, element2);
        }

        private void changeElement(Border element1, Border element2)
        {

           //int i = Array.IndexOf(elementsGame, element1);
           int index1 = Array.IndexOf(elementsGame, elementsGame.First(obj => obj.Tag.Equals(element1.Tag)));
           int index2 = Array.IndexOf(elementsGame, elementsGame.First(obj => obj.Tag.Equals(element2.Tag)));
           elementsGame[index1] = element2;
           elementsGame[index2] = element1;
           checkSetting(index1);
           checkSetting(index2);
        }

        private void checkSetting(int indexElement)
        {
            if (elementsGame[indexElement].Tag.Equals(indexElement.ToString()))
                elementsCorrectGame[indexElement / 8] |= (byte)(1 << indexElement);
            else if ((elementsCorrectGame[indexElement / 8] & (1 << indexElement)) == 1)
                elementsCorrectGame[indexElement / 8] &= (byte)(~(1 << indexElement));

            /*if (elementsGame[indexElement].Tag.Equals(indexElement.ToString()))
                elementsCorrectGame.Add((byte)indexElement);
            else if (elementsCorrectGame.Contains((byte)indexElement))
                elementsCorrectGame.Remove((byte)indexElement);*/
        }

        private void searchIdenticalFragmentImage()
        {
            BitmapImage fragmentImage = null;
            ImageSource z = null;
     
            foreach(Border el in elementsGame)
            {
                fragmentImage = (BitmapImage)((Image)el.Child).Source;
                for (int j = 0; j < fragmentImage.Height; j++)
                {
                    for (int i = 0; i < fragmentImage.Width; i++)
                    {
                        //reduce colors to true / false                
                        //fragmentImage.p
                    }
                }
            }
        }

        private Border[] elementsGame;
        private Dictionary<byte, List<byte>> mapIdenticalFragmentImage;
        private List<byte> elementsCorrectGame;
        private int countAllElements;

    }
}
