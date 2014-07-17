using System;
using System.Linq;
using System.Web;
using System.Data;
using LinqToExcel;
using System.Data.Entity;
using System.Web.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 


namespace MedicalCompoundManagement.Models
{
    
    public class MedicalCompound
    {
        private string MedicalCompoundDbConnection = WebConfigurationManager.OpenWebConfiguration(null).ConnectionStrings["MedicalCompoundDbContext"];
        private string connectionString = string.Empty;

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

        public void BulkLoadMedicalCompounds
        {

        }

        public IQueryable<MedicalCompound> setMedicalCompoundsFromExcel(string ExcelFileName)
        {
            var excel = new ExcelQueryFactory(ExcelFileName);
            var medicalCompounds = from x in excel.Worksheet<MedicalCompound>()
                                   select x;

            foreach (var v in medicalCompounds)
            {
                v.CreateTs = DateTime.Now;
                v.UpdateTs = DateTime.Now;
                v.CreateUser = HttpContext.Current.User.Identity.Name;
                v.UpdateUser = HttpContext.Current.User.Identity.Name;
            }

            return medicalCompounds; 

        }

    }



    public class MedicalCompoundDbContext : DbContext
    {
        public DbSet<MedicalCompound> MedicalCompounds { get; set; }
    }

}