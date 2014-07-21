using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using MedicalCompoundManagement.Models;

namespace MedicalCompoundManagement.Controllers
{
    public class MedicalCompoundsController : Controller
    {
        private MedicalCompoundDbContext db = new MedicalCompoundDbContext();
        private string ExcelFileName = "MedicalCompounds.xlsx";

        // GET: MedicalCompounds
        public ActionResult Index()
        {
            return View(db.MedicalCompounds.ToList());
        }

        // GET: MedicalCompounds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalCompound medicalCompound = db.MedicalCompounds.Find(id);
            if (medicalCompound == null)
            {
                return HttpNotFound();
            }
            return View(medicalCompound);
        }

        public ActionResult UploadMasterList()
        {
            return View();
        }

        // GET: MedicalCompounds/Create
        public ActionResult Create()
        {
            return View();
        }


        //POST: Get Excel File Master List
        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), ExcelFileName);
                file.SaveAs(path);
            }

            LoadExcelSheetData();
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }


        // POST: MedicalCompounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CreateTs,CreateUser,UpdateTs,UpdateUser")] MedicalCompound medicalCompound)
        {
            if (ModelState.IsValid)
            {
                db.MedicalCompounds.Add(medicalCompound);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicalCompound);
        }

        // GET: MedicalCompounds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalCompound medicalCompound = db.MedicalCompounds.Find(id);
            if (medicalCompound == null)
            {
                return HttpNotFound();
            }
            return View(medicalCompound);
        }

        // POST: MedicalCompounds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CreateTs,CreateUser,UpdateTs,UpdateUser")] MedicalCompound medicalCompound)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalCompound).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicalCompound);
        }

        // GET: MedicalCompounds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalCompound medicalCompound = db.MedicalCompounds.Find(id);
            if (medicalCompound == null)
            {
                return HttpNotFound();
            }
            return View(medicalCompound);
        }

        // POST: MedicalCompounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalCompound medicalCompound = db.MedicalCompounds.Find(id);
            db.MedicalCompounds.Remove(medicalCompound);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void LoadExcelSheetData()
        {
            MedicalCompound mc = new MedicalCompound();
            if (ExcelFileName != null)
            {
                mc.BulkLoadMedicalCompounds(ExcelFileName);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
