using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Falcon.Mvc.Controllers;
using Falcon.Infrastructure;
using Falcon.Services.Supports;
using Falcon.Modules.Home.Models;
using Falcon.Common;
using Common.Logging;
using Falcon.Data.Domain;
using Falcon.Common.UI;
using FreshdeskApi.Entities;
using FreshdeskApi.Services;
using System.Collections.ObjectModel;
using System.IO;
using FreshdeskApi.Services.Ticket;

namespace Falcon.Modules.Home.Controllers
{
    public class HomeController : BaseController
    {
        FreshdeskService freshdeskService = new FreshdeskService(Settings.FreshdeskApiKey, Settings.FreshdeskUri);
        //private readonly IArticleService _articleService;
        //private readonly ICategoryTypeService _categoryTypeService;
        //private readonly ICategoryService _categoryService;
        //private readonly ISupportTypeService _supportTypeService;
        //private readonly ILog _logger = LogManager.GetCurrentClassLogger();
        //private readonly int _PageSize = 9;

        //public HomeController(IArticleService articleService, ICategoryTypeService categoryTypeService, ICategoryService categoryService, ISupportTypeService supportTypeService)
        //{
        //    _articleService = articleService;
        //    _categoryTypeService = categoryTypeService;
        //    _categoryService = categoryService;
        //    _supportTypeService = supportTypeService;
        //}
        //
        // GET: /Home/Home/       
        [Layout("Home")]
        public ActionResult Index()
        {
            // CreateTicketWithAttachment fr = new CreateTicketWithAttachment();
            //fr.CreateTicketFreshdesk();

            Title = "Tài liệu hướng dẫn";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Index");
            ViewData["CSS"] = "default";
            return View();
        }
        [Layout("Home")]
        public ActionResult CreateSupport()
        {
            Title = "Chào mừng bạn đến với Trung tâm trợ giúp khách hàng Bizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("CreateSupport");
            ViewData["CSS"] = "default";
            return View();
        }

        [Layout("Home")]
        [HttpPost]
        public ActionResult CreateSupport(TicketModel ticket)
        {
            Title = "Chào mừng bạn đến với Trung tâm trợ giúp khách hàng Bizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("CreateSupport");
            ViewData["CSS"] = "default";
            if (ticket.attach_files != null)
            {
                ticket.attach_files = ticket.attach_files.Where(m => m != null).ToList();
            }
            if (ticket.attach_files != null && ticket.attach_files.Any())
            {
                foreach (var file in ticket.attach_files)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        file.SaveAs(path);
                    }
                }
                GetTicketResponse ticketResponse = FreshdeskCreateTicketWithAttachment(ticket);

            }
            else
            {
                GetTicketResponse ticketResponse = FreshdeskCreateTicket(ticket);
            }
            return View(ticket);
        }





        public GetTicketResponse FreshdeskCreateTicket(TicketModel ticket)
        {
            // create Support Ticket for a followup demonstration with Support software integration
            GetTicketResponse ticketResponse = freshdeskService.CreateTicket(new CreateTicketInfo()
            {
                Subject = ticket.Subject,
                Email = ticket.Email,
                Description = ticket.ContentRequired,
                Priority = 1,
                Status = 2,
                CustomFields = new Field()
                {
                    Name = ticket.Name,
                    Website = ticket.Website,
                    Title = ticket.TitleRequired
                }

            });
            return ticketResponse;

        }

        public GetTicketResponse FreshdeskCreateTicketWithAttachment(TicketModel ticket)
        {
           
            Collection<Attachment> Collection = new Collection<Attachment>();
            foreach (var file in ticket.attach_files)
            {
                if (file.ContentLength > 0)
                {
                    Attachment attachment = new Attachment
                    {
                       Content = System.IO.File.OpenRead(Server.MapPath("~/App_Data/uploads/") + file.FileName),
                        FileName = file.FileName
                    };
                    Collection.Add(attachment);
                }
            }

            // create Support Ticket for a followup demonstration with Support software integration
            var ticketResponse = freshdeskService.CreateTicketWithAttachment(new CreateTicketInfo
            {
                Subject = ticket.Subject,
                Email = ticket.Email,
                Description = ticket.ContentRequired,
                Priority = 1,
                Status = 2,
                CustomFields = new Field()
                {
                    Name = ticket.Name,
                    Website = ticket.Website,
                    Title = ticket.TitleRequired
                }

            },
            Collection
            );
            return ticketResponse;
        }


        [Layout("Home")]
        public ActionResult Login()
        {
            Title = "Chào mừng bạn đến với Trung tâm trợ giúp khách hàng Bizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Login");
            ViewData["CSS"] = "default";
            return View();
        }

        [Layout("Home")]
        public ActionResult Register()
        {
            Title = "Đăng ký tài khoản SupportBizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Register");
            ViewData["CSS"] = "default";
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password)
        {
            Title = "Đăng ký tài khoản SupportBizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Register");
            ViewData["CSS"] = "default";
            return View();
        }

        [Layout("Home")]
        public ActionResult CheckStatusSupport()
        {
            Title = "Đăng ký tài khoản SupportBizweb";
            MetaDescription = "Tài liệu hướng dẫn";
            CanonicalLink = FalconConfig.DomainName + Url.Action("Register");
            ViewData["CSS"] = "default";
            return View();
        }



    }
}
