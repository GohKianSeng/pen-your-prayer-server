using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Supporting;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PenYourPrayerServer.Controllers
{
    [HMACAuthorizeAttribute]
    [RoutePrefix("api/Attachment")]
    public class AttachmentController : ApiController
    {
        [Route("AddPrayerImage")]
        public HttpResponseMessage AddPrayerImage(string GUID)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            string savepath = "";
            string filename = GUID;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string extension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    filename = filename + extension;
                    String directoryTosave = QuickReference.PrayerAttachmentImageTemp + @"\" + user.Id.ToString();
                    if (!System.IO.Directory.Exists(directoryTosave))
                        System.IO.Directory.CreateDirectory(directoryTosave);
                    savepath = directoryTosave + @"\" + filename;
                    if (!System.IO.File.Exists(savepath))
                        postedFile.SaveAs(savepath);

                }


            }


            return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = filename });
        }

        [HttpGet]
        [Route("CheckImageUploaded")]
        public HttpResponseMessage CheckImageUploaded(string GUID, string filename)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            string savepath1 = "";
            string savepath2 = "";
            filename = GUID + filename.Substring(filename.LastIndexOf('.'));
            
            String directoryTosave1 = QuickReference.PrayerAttachmentImageTemp + @"\" + user.Id.ToString();
            String directoryTosave2 = QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString();
            if (!System.IO.Directory.Exists(directoryTosave1))
                System.IO.Directory.CreateDirectory(directoryTosave1);

            if (!System.IO.Directory.Exists(directoryTosave2))
                System.IO.Directory.CreateDirectory(directoryTosave2);
            
            savepath1 = directoryTosave1 + @"\" + filename;
            savepath2 = directoryTosave2 + @"\" + filename;

            if (!System.IO.File.Exists(savepath1) && !System.IO.File.Exists(savepath2))
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "NOTEXISTS" });
            }
            else
                return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "EXISTS" });
        }
    }
}
