using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ProfileSampleEntities();

            var model = context.ImgSources
                .Take(20)
                .Select(image => new ImageModel
                {
                    Name = image.Name,
                    Data = image.Data
                })
                .ToList();

            return View(model);
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                List<ImgSource> imagesToAdd = files.Select(file =>
                {
                    using (var stream = new FileStream(file, FileMode.Open))
                    {
                        byte[] buff = new byte[stream.Length];

                        stream.Read(buff, 0, (int)stream.Length);

                        return new ImgSource()
                        {
                            Name = Path.GetFileName(file),
                            Data = buff,
                        };
                    }
                }).ToList();

                context.ImgSources.AddRange(imagesToAdd);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}