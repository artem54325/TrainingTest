using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TrainingTests.Models;

namespace TrainingTests.Helpers
{
    public class TypeConverters
    {

        public static List<string> StringToList(String data)
        {
            if (data == null)
            {
                return new List<string>();
            }
            return JsonConvert.DeserializeObject<List<string>>(data);
        }

        public static string ListToString(List<string> someObjects)
        {
            return JsonConvert.SerializeObject(someObjects);
        }


        public static List<Mark> StringToMark(String data)
        {
            if (data == null)
            {
                return new List<Mark>();
            }
            return JsonConvert.DeserializeObject<List<Mark>>(data);
        }

        public static string MarksToString(List<Mark> someObjects)
        {
            return JsonConvert.SerializeObject(someObjects);
        }
    }
}
