using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using TestGba.IServices;
using TestGPA.Helper;
using TestGPA.ViewModels;

namespace TestGba.Services
{
    public class ApplicationServices : IApplicationServices
    {
        private readonly ExcelFile workBook;
        public ApplicationServices(IHostingEnvironment hosting)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            workBook = ExcelFile.Load(hosting.WebRootPath + @"/testGPA.xlsx");
        }

        public GPA CalculateGPA(List<PostData> data, string oldGpa, int count)
        {
            float totalSum = 0.0f;
            float HGPA = 0.0f;
            float HAV = 0.0f;

            foreach (var item in data)
            {
                totalSum += item.baseH * item.selectedH;
                
                if (item.selectedH > 0)
                    HGPA += item.baseH;

                HAV += item.baseH;
            }

            var gpa = (((Math.Round((double)(totalSum / HGPA), 2) + Convert.ToDouble(oldGpa))) / count);

            return new GPA
            {
                SemesterAverage = new string(Math.Round((totalSum / HAV), 2).ToString().ToArray()),
                TotalGPA = new string(Math.Round(gpa, 2).ToString().ToArray())
            };
        }

        public List<MaterialViewModel> GetAllMaterialInLevel(string levelName, string departmenName)
        {
            ExcelWorksheet sheet = workBook.Worksheets[0];

            List<ExcelRow> result = sheet.Rows.Where(s => s.Cells[3].StringValue == levelName 
            && s.Cells[4].StringValue == departmenName).ToList();

            return result.Select(r => new MaterialViewModel
            {
                Id = r.Cells[0].StringValue,
                Title = r.Cells[1].StringValue,
                Hours = Convert.ToByte(r.Cells[2].StringValue)
            }).ToList();
        }

        public List<string> GetDepartmentNames()
        {
            ExcelWorksheet sheet = workBook.Worksheets[0];

            List<string> names = new List<string>();

            for (int i = 1; i < sheet.Rows.Count(); i++)
            {
                string cell = sheet.Cells[i, 4].Value.ToString();
                
                if (!names.Contains(cell))
                    names.Add(cell);
            }

            return names;
        }
    }
}
