using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.Entity;
using System.Web.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 


namespace MedicalCompoundManagement.Models
{
    
    public class MedicalCompound
    {
        private string MedicalCompoundDbConnection = WebConfigurationManager.OpenWebConfiguration(null).ConnectionStrings["MedicalCompoundDbContext"];


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

        public void BulkLoadMedicalCompounds(string ExcelFileName)
        {
            string excelConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;Persist Security Info=False", ExcelFileName);
            DataTable excelTables = getOleSchemaTableNames(excelConnectionString);

            


           

        }

        //for reuse
        private DataTable getOleSchemaTableNames(string ConnectionString)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(ConnectionString))
                {
                    if (connection.State == ConnectionState.Closed )
                    {
                        connection.Open();
                    }

                    DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    return schemaTable;
                }
            }
            catch (OleDbException e)
            {
                throw new OleDbException();
            }
        }
    }

    public class MedicalCompoundDbContext : DbContext
    {
        public DbSet<MedicalCompound> MedicalCompounds { get; set; }
    }

}