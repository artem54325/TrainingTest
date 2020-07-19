using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingTests.ViewModels
{
    public class TestView
    {
        // id = a.Id, name = a.Name, description = a.Description, questions = a.Themes.Sum(q => q.CountQuest) }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Questions { get; set; }
    }
}
