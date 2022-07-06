using Kristin2.Context;
using Kristin2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Net;

namespace Kristin2.Controllers
{
    public class CustomerController : Controller
    {
        MyContext Events = new MyContext();
        MyContext db = new MyContext();

        // GET: Customer
        public ActionResult CustomersList()
        {
            return View(db.Customers.ToList());

        }

        public ActionResult CustomersPage(int? id)
        {
            int SessionId = Convert.ToInt32(Session["UserID"]);
            id = SessionId == id || id == null ? SessionId : id;
            CustomerModel customer = db.Customers.SingleOrDefault(c => c.ID == id);
            if (customer == null)
                return HttpNotFound();
            List<CalanderModel> EventList = Events.Eventsdb.Where(x => x.Customer == customer.FirstName + " " + customer.LastName).ToList();

            CalanderModel cal = new CalanderModel
            {
                Price = EventList.Sum(x => x.Price)
            };
            EventList.Add(cal);
            var tuple = new Tuple<CustomerModel, List<CalanderModel>>(customer, EventList);
            return View(tuple);
        }


        public ActionResult Delete(int id)
        {

            CustomerModel customer = db.Customers.SingleOrDefault(c => c.ID == id);
            if (customer == null)
                return HttpNotFound();
            if (!(customer.Image == "http://ssl.gstatic.com/accounts/ui/avatar_2x.png"))
            {
                var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
                Cloudinary _cloudinary = new Cloudinary(myAccount);
                int pos = customer.Image.LastIndexOf("placeOfBueaty/Users/");
                string delImg = customer.Image.Substring(pos, customer.Image.Length - pos - 4);
                _cloudinary.DeleteResources(delImg);
            }

            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("CustomersList", "Customer");

        }

        [HttpPost]
        public ActionResult Create(CustomerModel customer)
        {
            customer.Image = customer.Image ?? "http://ssl.gstatic.com/accounts/ui/avatar_2x.png";
            customer.CreatedDate = DateTime.Now;
            customer.LastLoginDate = DateTime.Now;
            if (customer.AdminCode != 222)
                customer.AdminCode = 0;

            db.Customers.Add(customer);
            db.SaveChanges();
            Session["UserID"] = customer.ID.ToString();
            Session["FirstName"] = customer.FirstName.ToString();
            return RedirectToAction("CustomersPage");
        }

        [HttpPost]
        public ActionResult Edit(CustomerModel customer)
        {
            if (ModelState.IsValid)
            {

                CustomerModel user = db.Customers.FirstOrDefault(u => u.ID.Equals(customer.ID));

                // Update fields
                user.UserName = customer.UserName ?? user.UserName;
                user.FirstName = customer.FirstName ?? user.FirstName;
                user.LastName = customer.LastName ?? user.LastName;
                user.Password = customer.Password ?? user.Password;
                user.Email = customer.Email ?? user.Email;
                user.Phone = customer.Phone ?? user.Phone;
                user.Image = customer.Image ?? user.Image;

                db.SaveChanges();

                return RedirectToAction("CustomersPage", customer);
            }

            return View(customer);
        }


        [HttpPost]
        public ActionResult UploadImage()
        {
            //save file in App_Data
            var res = "";
            var file = Request.Files[0];
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Content/imgs/"), fileName);
            file.SaveAs(path);

            //connect to cloudinary account
            var myAccount = new Account { ApiKey = "555682285552641", ApiSecret = "Id-vLH2JZBKc7x0wK3ZEZYCsGkA", Cloud = "dmrx96yqx" };
            Cloudinary _cloudinary = new Cloudinary(myAccount);

            int id = Convert.ToInt32(Session["UserID"]);
            if (id != 0)//if the user is connected, update image
            {
                CustomerModel user = db.Customers.FirstOrDefault(u => u.ID.Equals(id));
                int pos = user.Image.LastIndexOf('/') + 1;
                string delImg = user.Image.Substring(pos, user.Image.Length - pos - 4);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    Folder = "placeOfBueaty/Users",
                    PublicId = delImg,

                };
                var uploadResult = _cloudinary.Upload(uploadParams);
                res = uploadResult.SecureUri.ToString();
                user.Image = res;
                db.SaveChanges();
            }
            else//if the user is register, upload new image
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    Folder = "placeOfBueaty/Users",
                };
                var uploadResult = _cloudinary.Upload(uploadParams);
                res = uploadResult.SecureUri.ToString();
            }
            //delete the image from App_Data
            FileInfo del = new FileInfo(path);
            del.Delete();
            //send back url.
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { success = true, responseText = res }, JsonRequestBehavior.AllowGet);
        }
    }
}