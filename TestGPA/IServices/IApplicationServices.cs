using System.Collections.Generic;
using TestGPA.ViewModels;

namespace TestGba.IServices
{
    public interface IApplicationServices
    {
        /// <summary>
        ///     Get all materials in level by level name and department name.
        /// </summary>
        /// <param name="levelName">Expresses level name.</param>
        /// <param name="departmenName">Expresses departmen Name.</param>
        /// <returns>List of materials type of MaterialViewModel.</returns>
        public List<MaterialViewModel> GetAllMaterialInLevel(string levelName, string departmenName);

        /// <summary>
        ///     Get all department names.
        /// </summary>
        /// <returns>List of department names type of string</returns>
        public List<string> GetDepartmentNames();

        /// <summary>
        ///     Calculate GPA.
        /// </summary>
        /// <param name="data">Expresses data.</param>
        /// <param name="oldGpa">Expresses old gpa.</param>
        /// <param name="count">Expresses count.</param>
        /// <returns>GPA.</returns>
        public GPA CalculateGPA(List<MaterialSelectViewModel> data, string oldGpa);
    }
}
