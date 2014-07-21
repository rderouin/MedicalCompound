namespace MedicalCompoundManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnlargeDataSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MedicalCompounds", "Name", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MedicalCompounds", "Name", c => c.String(maxLength: 100));
        }
    }
}
