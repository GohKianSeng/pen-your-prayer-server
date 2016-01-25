using PenYourPrayerServer.Models;
using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Supporting;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                return Request.CreateResponse(HttpStatusCode.Accepted, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Accepted, Description = "EXISTS-" + filename });
        }

        [HttpGet]
        [AllowAnonymous]        
        [Route("DownloadPrayerAttachment")]
        public HttpResponseMessage DownloadPrayerAttachment(long AttachmentID, long UserID)
        {
            string filename = "";
            long? OwnerID = -1;
            using (DBDataContext db = new DBDataContext())
            {
                db.usp_GetPrayerAttachmentInformation(AttachmentID, UserID, ref filename, ref OwnerID);
                if (filename.Length > 0 && OwnerID != -1)
                {
                    var path = QuickReference.PrayerAttachmentImage + @"\" + OwnerID.ToString() + @"\" + filename;                    
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(path, FileMode.Open);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = filename;
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Headers.CacheControl = new CacheControlHeaderValue();
                    result.Headers.CacheControl.MaxAge = new TimeSpan(87600, 0, 0);  // 10 min. or 600 sec.
                    
                    return result;                    
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Forbidden, Description = "" });
                }
            }

            
        }
    }
}
