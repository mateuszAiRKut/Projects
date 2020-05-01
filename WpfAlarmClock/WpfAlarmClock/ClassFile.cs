using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WpfAlarmClock
{
    public static class ClassFile
    {
        public static bool ChangeDataFile { get; private set; }
        static ClassFile()
        {
            pathFile = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\Data.txt"));
            listData = new List<string>(5);
            if (File.Exists(pathFile))
            {
                OpenFile();
                setIndexWithAlarmClocks();
            }
        }

        public static string SearchSaveValue(string key)
        {
            string elementList = listData.Find((obj) => obj.Contains(key));
            if (elementList == null)
                return null;
            string valueData = null;
            ClassParserSettings.SetLeftValueAndRightValue(elementList, '-', leftValue: ref key, rightValue: ref valueData);
            return valueData;
        }

        public static void UpdateOrAddDataToList(in string data, byte count = 1)
        {
            string key = null, valueNewData = null, elementList = null, valuesElementList = null;
            int indexList = 0;
            ClassParserSettings.SetLeftValueAndRightValue(data, '-', leftValue: ref key, rightValue: ref valueNewData);
            if(key != null && valueNewData != null)
            {
                elementList = listData.Find((obj) => obj.Contains(key));
                indexList = listData.FindIndex((obj) => obj.Contains(key));
                if (elementList == null)
                {
                    listData.Add((count == 1) ? data : data + "/1"); 
                    ChangeDataFile = true;
                }
                else
                {
                    ClassParserSettings.SetLeftValueAndRightValue(elementList, '-', leftValue: ref key, rightValue: ref valuesElementList);
                    string[] arrayValuesElementList = valuesElementList.Split(',');
                    for(byte index = 0; index < arrayValuesElementList.Length; index++)
                    {
                        if(ClassParserSettings.ExistValue('/', valueElementList: arrayValuesElementList[index], valueNewData: valueNewData))
                        {
                            if (count > 1)
                            {
                                ClassParserSettings.IncreaseValueCount('/', ref arrayValuesElementList[index]);
                                listData[indexList] = key + "-" + string.Join(",", arrayValuesElementList);
                            }
                            return;
                        }
                    }
                    if (count > 1 && arrayValuesElementList.Length >= count)
                        ClassParserSettings.RemoveSmallestValueCount('/', arrayValuesElementList);
                    ClassParserSettings.ChangeValue(ref arrayValuesElementList, valueNewData, count);
                    listData[indexList] = key + "-" + string.Join(",", arrayValuesElementList);
                    ChangeDataFile = true;
                }
                setIndexWithAlarmClocks();
            }
        }

        public static void SaveFile()
        {
            saveDataFile(); //sposob 1
            //saveDataStreamFile(); //sposob 2
        }

        public static void OpenFile()
        {
            readLinesFromFile(); //sposob 1
            //readStreamLinesFromFile(); //sposob 2
        }

        public static string GetValuesAlarmClocks()
        {
            if (indexListWithAlarmClocks == -1)
                return null;
            string key = null, value = null;
            ClassParserSettings.SetLeftValueAndRightValue(listData[indexListWithAlarmClocks], '-', leftValue: ref key, rightValue: ref value);
            return value;
        }

        private static void readLinesFromFile()
        {
            listData = File.ReadAllLines(pathFile).ToList<string>();
        }

        private static void saveDataFile()
        {
            File.WriteAllLines(pathFile, listData);
        }

        private static void readStreamLinesFromFile()
        {
            using (StreamReader reader = new StreamReader(pathFile))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                    listData.Add(line);
            }
        }

        private static void saveDataStreamFile()
        {
            using (StreamWriter file = new StreamWriter(pathFile))
            {
                foreach (string data in listData)
                    file.WriteLine(data);
            }
        }

        private static void setIndexWithAlarmClocks()
        {
            if (indexListWithAlarmClocks == -1)
            {
                int index = listData.FindIndex((obj) => obj.Contains("Alarm clock"));
                if (index != -1)
                    indexListWithAlarmClocks = index;
            }
        }

        private static readonly string pathFile;
        private static int indexListWithAlarmClocks = -1;
        private static List<string> listData;
    }
}
