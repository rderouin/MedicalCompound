using System;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Data.Entity;
using System.ComponentModel; 
using System.Configuration;
using System.Data.SqlClient; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MedicalCompoundManagement.Models
{
    
    public class MedicalCompound : Object 
    {
        private string MedicalCompoundDbConnection = ConfigurationManager.ConnectionStrings["MedicalCompoundDbContext"].ConnectionString;
        private string connectionString = string.Empty;
        private OleDbConnection oleDbConnection;
        private OleDbCommand oleDbCommand;


        #region Properties
        public int ID { get; set; }

        [Display(Name = "Compound Name")]
        [StringLength(250)]
        public string Name { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateTs { get; set; }

        [Display(Name = "Created By")]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CreateUser { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]

        public DateTime UpdateTs { get; set; }

        [Display(Name = "Updated By")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string UpdateUser { get; set; } 
        #endregion

        #region Import Excel Methods
        public void BulkLoadMedicalCompounds(string excelFileName)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MedicalCompoundExcelSheet"].ConnectionString; ;

            using (SqlBulkCopy bulkLoad = new SqlBulkCopy(MedicalCompoundDbConnection))
            {
                SetOleDbConnection();
                if (oleDbConnection.State == ConnectionState.Closed)
                {
                    oleDbConnection.Open();
                }

                DataTable schemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string excelConnectionString = string.Format("SELECT CompoundId AS [ID], CompoundName AS [Name], CreateTs, CreateUser, UpdateTs, UpdateUser FROM [{0}]", schemaTable.Rows[0].Field<string>("TABLE_NAME"));
                SetOleDbCommand(excelConnectionString);

                bulkLoad.DestinationTableName = "MedicalCompounds";
                bulkLoad.WriteToServer(oleDbCommand.ExecuteReader());
            }
            CloseOleDbConnection();
        }

        private void SetOleDbCommand(string SQLQuery)
        {
            oleDbCommand = new OleDbCommand(SQLQuery,oleDbConnection);
        }

        private void SetOleDbConnection()
        {
            oleDbConnection = new OleDbConnection(connectionString);
        }


        private void CloseOleDbConnection()
        {
            if (oleDbConnection.State != ConnectionState.Closed)
            {
                this.oleDbConnection.Close();
            }
        }
        #endregion
    }

    public class MedicalCompoundDbContext : DbContext
    {
        public DbSet<MedicalCompound> MedicalCompounds { get; set; }
    }

}