using GemBox.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using TestGba.IServices;
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

        public GPA CalculateGPA(List<MaterialSelectViewModel> data, string oldGpa)
        {
            float totalSum = 0.0f, HGPA = 0.0f, HAV = 0.0f;

            data = data.OrderBy(d => d.degree).ToList();

            foreach (var item in data)
            {
                totalSum += item.hours * item.degree;
                
                if (item.degree > 0)
                    HGPA += item.hours;

                HAV += item.hours;
            }

            double OldGPA = Convert.ToDouble(oldGpa);
            double newGpa = 0.0;

            if (OldGPA * 1 == 0)
                newGpa = ((Math.Round((double)(totalSum / HGPA), 2)));

            else
                newGpa = (((Math.Round((double)(totalSum / HGPA), 2) + OldGPA)) / 2);

            GPA result = new GPA
            {
                SemesterAverage = new string(Math.Round((totalSum / HAV), 2).ToString().ToArray()),
                TotalGPA = new string(Math.Round(newGpa, 2).ToString().ToArray())
            };

            if (newGpa < 2.3)
                result.Bad = GetBadMaterials(data, OldGPA, totalSum, HAV);


            return result;
        }

        public List<MaterialViewModel> GetAllMaterialInLevel(string levelName, string departmenName)
        {
            ExcelWorksheet sheet = workBook.Worksheets[0];

            List<ExcelRow> result = sheet.Rows.Where(s => s.Cells[3].StringValue == levelName 
            && s.Cells[4].StringValue == departmenName).ToList();

            return result.Select(r => new MaterialViewModel
            {
                id = r.Cells[0].StringValue,
                title = r.Cells[1].StringValue,
                hours = Convert.ToDouble(r.Cells[2].StringValue)
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

        private List<MaterialViewModel> GetBadMaterials(List<MaterialSelectViewModel> data, double OldGpa,float totalSum, double h)
        {
            data = data.OrderBy(d => d.degree).ToList();
            ExcelWorksheet sheet = workBook.Worksheets[0];
            double newGpa = 0.0;
            
            if (OldGpa * 1 == 0)
                newGpa = ((Math.Round((double)(totalSum / h), 2)));

            else
                newGpa = (((Math.Round((double)(totalSum / h), 2) + OldGpa)) / 2);

            var result = new List<MaterialViewModel>();

            for (int i = 0; i < data.Count; i++)
            {
                if (newGpa < 2.3)
                {
                    var item = sheet.Rows.Where(r => r.Cells[0].StringValue == data[i].id).SingleOrDefault();
                    result.Add(new MaterialViewModel
                    {
                        id = item.Cells[0].StringValue,
                        title = item.Cells[1].StringValue,
                        hours = Convert.ToDouble(item.Cells[2].StringValue)
                    });
                }

                else
                    break;

                if (data[i].hours == 0)
                    h += data[i].hours;
                
                double x = (data[i].degree * data[i].hours) / h;
                newGpa -= x;

                double y = (3 * data[i].hours) / h;
                newGpa += y;
            }
            // 2.54
            return result;
        }
    }
}