using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Kristin2.Context;
using Kristin2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Kristin2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Gallery()
        {
            List<string> urls = new List<string>();

            var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            SearchResult result = _cloudinary.Search().Expression("folder:\"placeOfBueaty/Gallery\"").SortBy("created_at", "desc").Execute();
            foreach (var item in result.Resources)
            {
                var getResult = _cloudinary.GetResource(item.PublicId);
                urls.Add(getResult.SecureUrl);
            }

            return View(urls);
        }

        public ActionResult NailPolishPicker()
        {
            return View();
        }
        public ActionResult Colors()
        {
            List<string> urls = new List<string>();

            var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            SearchResult result = _cloudinary.Search().Expression("folder:\"placeOfBueaty/Colors\"").SortBy("created_at", "desc").Execute();
            foreach (var item in result.Resources)
            {
                var getResult = _cloudinary.GetResource(item.PublicId);
                urls.Add(getResult.SecureUrl);
            }

            return View(urls);
        }

        [HttpPost]
        public ActionResult UploadToGallery(IEnumerable<HttpPostedFileBase> file)
        {
            var res = "";
            //save file in App_Data
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file2 = Request.Files[i];
                var fileName = Path.GetFileName(file2.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/imgs/"), fileName);
                file2.SaveAs(path);

                //connect to cloudinary account
                var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
                Cloudinary _cloudinary = new Cloudinary(myAccount);


                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    Folder = "placeOfBueaty/Gallery",
                };
                var uploadResult = _cloudinary.Upload(uploadParams);
                res = uploadResult.SecureUri.ToString();


                //delete the image from App_Data
                FileInfo del = new FileInfo(path);
                del.Delete();
                //send back url.
                Response.StatusCode = (int)HttpStatusCode.OK;
            }

            return RedirectToAction("Gallery", "Home");
        }
        [HttpPost]
        public ActionResult UploadToColors(IEnumerable<HttpPostedFileBase> file)
        {

            //save file in App_Data
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file2 = Request.Files[i];
                var fileName = Path.GetFileName(file2.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/imgs/"), fileName);
                file2.SaveAs(path);

                //connect to cloudinary account
                var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
                Cloudinary _cloudinary = new Cloudinary(myAccount);


                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    Folder = "placeOfBueaty/Colors",
                };
                var uploadResult = _cloudinary.Upload(uploadParams);
                // res = uploadResult.SecureUri.ToString();


                //delete the image from App_Data
                FileInfo del = new FileInfo(path);
                del.Delete();
                //send back url.
                Response.StatusCode = (int)HttpStatusCode.OK;
            }

            return RedirectToAction("Colors", "Home");
        }

        public ActionResult GalleryDel(string link)
        {
            var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            int pos = link.LastIndexOf("placeOfBueaty/Gallery/");
            string delImg = link.Substring(pos, link.Length - pos - 4);
            _cloudinary.DeleteResources(delImg);
            return RedirectToAction("Gallery", "Home");

        }

        public ActionResult ColorsDel(string link)
        {
            var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);
            int pos = link.LastIndexOf("placeOfBueaty/Colors/");
            string delImg = link.Substring(pos, link.Length - pos - 4);
            _cloudinary.DeleteResources(delImg);
            return RedirectToAction("Colors", "Home");

        }
    }
}

