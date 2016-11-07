using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Falcon.Modules.Home.Models
{
    public class TicketModel
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("title_required")]
        public string TitleRequired { get; set; }
        [AllowHtml]
        [JsonProperty("content_required")]
        public string ContentRequired { get; set; }

        public List<HttpPostedFileBase> attach_files { get; set; }

        public TicketModel()
        {
            attach_files = new List<HttpPostedFileBase>();
        }
    }
    public class file
    {
        [JsonProperty("attach_file")]
        public HttpPostedFileBase AttachFile { get; set; }
    }
}