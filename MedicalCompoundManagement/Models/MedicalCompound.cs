using System;
using System.Linq;
using System.Web;
using System.Data;
using LinqToExcel;
using System.Data.SqlClient; 
using System.Data.Entity;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 


namespace MedicalCompoundManagement.Models
{
    
    public class MedicalCompound : Object 
    {
        private string MedicalCompoundDbConnection = ConfigurationManager.ConnectionStrings["MedicalCompoundDbContext"].ConnectionString;
        private string connectionString = string.Empty;

        #region Properties
        [Required]
        public int ID { get; set; }

        [Display(Name = "Compound Name")]
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateTs { get; set; }

        [Display(Name = "Created By")]
        [StringLength(50)]
        public string CreateUser { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]

        public DateTime UpdateTs { get; set; }

        [Display(Name = "Updated By")]
        [StringLength(50)]
        public string UpdateUser { get; set; } 
        #endregion

        #region Import Excel Methods
        public void BulkLoadMedicalCompounds(string excelFileName)
        {
            using (SqlBulkCopy bulkLoad = new SqlBulkCopy(MedicalCompoundDbConnection))
            {
                bulkLoad.DestinationTableName = "MedicalCompounds";
                bulkLoad.WriteToServer(GetMedicalCompoundsDataTable());
            }
        }

        private DataTable GetMedicalCompoundsDataTable()
        {
            return ConvertToDataTable(SetMedicalCompoundsFromExcel());
        }

        //refactor
        private DataTable ConvertToDataTable<T>(IList<T> list)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        private List<MedicalCompound> SetMedicalCompoundsFromExcel()
        {
            var excel = new ExcelQueryFactory(ConfigurationManager.ConnectionStrings["MedicalCompoundExcelSheet"].ConnectionString);
            var medicalCompounds = from x in excel.Worksheet<MedicalCompound>()
                                   select x;

            foreach (var v in medicalCompounds)
            {
                v.CreateTs = DateTime.Now;
                v.UpdateTs = DateTime.Now;
                v.CreateUser = HttpContext.Current.User.Identity.Name;
                v.UpdateUser = HttpContext.Current.User.Identity.Name;
            }

            return (List<MedicalCompound>)medicalCompounds;
        } 
        #endregion

    }



    public class MedicalCompoundDbContext : DbContext
    {
        public DbSet<MedicalCompound> MedicalCompounds { get; set; }
    }

}