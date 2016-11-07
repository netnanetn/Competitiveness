using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FreshdeskApi.Services.Ticket
{
   public class CreateTicketWithAttachment
    {
        //public  string Uri = "https://phonglala.freshdesk.com";
        //public  string _APIKey = "4XAM4l2ikNXcheJ4BSg";
        //public  string path = "/api/v2/tickets";
        //public  string _Url = "https://phonglala.freshdesk.com/api/v2/tickets.json";

        private string Uri { get; set; }
        private string ApiKey { get; set; }
        private string Path { get; set; }
        private string Url { get; set; }
        public CreateTicketWithAttachment(string Uri, string ApiKey, string Path)
        {
            this.Uri = Uri;
            this.ApiKey = ApiKey;
            this.Path = Path;
            this.Url = Uri+Path;
        }

     
        public static void writeCRLF(Stream o)
        {
            byte[] crLf = Encoding.ASCII.GetBytes("\r\n");
            o.Write(crLf, 0, crLf.Length);
        }

        public static void writeBoundaryBytes(Stream o, string b, bool isFinalBoundary)
        {
            string boundary = isFinalBoundary == true ? "--" + b + "--" : "--" + b + "\r\n";
            byte[] d = Encoding.ASCII.GetBytes(boundary);
            o.Write(d, 0, d.Length);
        }

        public static void writeContentDispositionFormDataHeader(Stream o, string name)
        {
            string data = "Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n";
            byte[] b = Encoding.ASCII.GetBytes(data);
            o.Write(b, 0, b.Length);
        }

        public static void writeContentDispositionFileHeader(Stream o, string name, string fileName, string contentType)
        {
            string data = "Content-Disposition: form-data; name=\"" + name + "\"; filename=\"" + fileName + "\"\r\n";
            data += "Content-Type: " + contentType + "\r\n\r\n";
            byte[] b = Encoding.ASCII.GetBytes(data);
            o.Write(b, 0, b.Length);
        }

        public static void writeString(Stream o, string data)
        {
            byte[] b = Encoding.ASCII.GetBytes(data);
            o.Write(b, 0, b.Length);
        }

        public void CreateTicketFreshdesk() { 
           
            // Define boundary:
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            // Web Request:
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(Url);

            wr.Headers.Clear();

            // Method and headers:
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;

            // Basic auth:
            string login = ApiKey + ":X"; // It could be your username:password also.
            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(login));
            wr.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            // Body:
            using (var rs = wr.GetRequestStream())
            {
                // Email:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "email");
                writeString(rs, "examplssse@example.com");
                writeCRLF(rs);

                // Subject:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "subject");
                writeString(rs, "Ticket Title");
                writeCRLF(rs);

                // Description:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "description");
                writeString(rs, "Ticket description.");
                writeCRLF(rs);

                // Status:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "status");
                writeString(rs, "2");
                writeCRLF(rs);

                // Priority:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFormDataHeader(rs, "priority");
                writeString(rs, "2");
                writeCRLF(rs);

                // Attachment:
                writeBoundaryBytes(rs, boundary, false);
                writeContentDispositionFileHeader(rs, "attachments[]", "x.txt", "text/plain");
                FileStream fs = new FileStream("D:/DKT/Project/sieweb/Falcon.Web/App_Data/uploads/header.txt", FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                rs.Write(data, 0, data.Length);
                writeCRLF(rs);

                // End marker:
                writeBoundaryBytes(rs, boundary, true);

                rs.Close();

                // Response processing:
                try
                {
                    var response = (HttpWebResponse)wr.GetResponse();
                    Stream resStream = response.GetResponseStream();
                    string Response = new StreamReader(resStream, Encoding.ASCII).ReadToEnd();
                }
                catch (Exception ex)
                {
                  
                }
            }

        }


    }
}
