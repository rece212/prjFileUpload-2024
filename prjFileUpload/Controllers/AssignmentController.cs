using Microsoft.AspNetCore.Mvc;
using prjFileUpload.Models;

namespace prjFileUpload.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public AssignmentController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        private static List<Assignment> assignments = new List<Assignment>();
        public IActionResult Index()
        {
            return View(assignments);
        }
        [HttpPost]
        public IActionResult Upload(IFormFile file, string uploaderName)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var encryptedBytes = EncryptionHelper.Encrypt(fileBytes);
                    System.IO.File.WriteAllBytes(path, encryptedBytes);
                }
                assignments.Add(new Assignment
                {
                    Id = assignments.Count + 1,
                    FileName = fileName,
                    UploaderName = uploaderName,
                    UploadDate = DateTime.Now
                });
            }
            return RedirectToAction("Index");
        }
        public IActionResult OpenFile(string fileName)
        {
            var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            var encryptedBytes = System.IO.File.ReadAllBytes(path);
            var decryptedBytes=EncryptionHelper.Decrypt(encryptedBytes);
            return File(decryptedBytes, "application/octet-stream", fileName);
        }
    }
}
