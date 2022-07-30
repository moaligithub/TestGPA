using System.Collections.Generic;

namespace TestGPA.ViewModels
{
    public class GPA
    {
        public string SemesterAverage { get; set; }
        public string TotalGPA { get; set; }
        public List<MaterialViewModel> Bad { get; set; }
    }
}
