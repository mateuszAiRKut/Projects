using System;

namespace WpfAlarmClock
{
    public static class ClassParserSettings
    {
        static ClassParserSettings()
        {         
        }

        public static void SetLeftValueAndRightValue(in string data, char separator, ref string leftValue, ref string rightValue)
        {
            int index = data.IndexOf(separator);
            if (index == -1)
                return;
            leftValue = data.Substring(0, index);
            rightValue = data.Substring(index + 1, data.Length - index - 1);
        }

        public static bool ExistValue(char separatorCount, in string valueElementList, in string valueNewData)
        {
            string valueWithoutCount = valueElementList;
            int index = valueElementList.IndexOf(separatorCount);
            if (index != -1)
                valueWithoutCount = valueElementList.Substring(0, index);
            return valueWithoutCount.Equals(valueNewData); 
        }

        public static void IncreaseValueCount(char separatorCount, ref string value)
        {
            byte count = 0;
            SetCount(separatorCount, value, ref count);
            if (count != 0)
                value = value.Substring(0, value.IndexOf(separatorCount) + 1) + (++count);
        }

        public static void RemoveSmallestValueCount(char separatorCount, string[] arrayValuesCount)
        {
            byte index = 1, indexSmallestValueCount = 0, count = 0;
            SetCount(separatorCount, arrayValuesCount[0], ref count);
            for(byte tempCount = count; index < arrayValuesCount.Length; index++)
            {
                SetCount(separatorCount, arrayValuesCount[index], ref count);
                if(count < tempCount)
                {
                    tempCount = count;
                    indexSmallestValueCount = index;
                }
            }
            arrayValuesCount[indexSmallestValueCount] = null;
        }

        public static void ChangeValue(ref string[] arrayValues, in string valueNewData, in byte count)
        {
            if (count == 1)
                arrayValues[0] = valueNewData;
            else
            {
                for(byte index = 0; index < arrayValues.Length; index++)
                {
                    if(arrayValues[index] == null)
                    {
                        arrayValues[index] = valueNewData + "/1";
                        return;
                    }
                }
                string[] arrayTemp = new string[arrayValues.Length + 1];
                Array.Copy(arrayValues, arrayTemp, arrayValues.Length);
                arrayTemp[arrayValues.Length] = valueNewData + "/1";
                arrayValues = arrayTemp;
            }
        }

        private static void SetCount(char separatorCount, in string value, ref byte count)
        {
            int index = value.IndexOf(separatorCount);
            if (index == -1)
                return;
            byte.TryParse(value.Substring(index + 1, value.Length - index - 1), out count);
        }

    }
}
