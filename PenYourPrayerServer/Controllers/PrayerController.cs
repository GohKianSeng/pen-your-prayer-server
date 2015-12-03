using PenYourPrayerServer.Models;
using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Supporting;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace PenYourPrayerServer.Controllers
{
    [HMACAuthorizeAttribute]
    [RoutePrefix("api/Prayer")]
    public class PrayerController : ApiController
    {
        [Route("AddNewPrayer")]
        public HttpResponseMessage AddNewPrayer(Prayer prayer)
        {
            long? prayerid = -1;
            using (DBDataContext db = new DBDataContext())
            {
                PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
                XElement selectedFriends = new XElement("Friends");
                if (prayer.selectedFriends != null)
                {
                    foreach (Friends friend in prayer.selectedFriends)
                    {
                        XElement t = new XElement("Friend");
                        t.Add(new XElement("UserID", friend.UserID));
                        selectedFriends.Add(t);
                    }
                }
                String res = "";
                db.usp_AddQueueAction((long?)user.Id, prayer.IfExecutedGUID, ref res);
                if(res.ToUpper() == "OK")
                    db.usp_AddNewPrayer((long?)user.Id, prayer.Content, prayer.CreatedWhen.ToUniversalTime(), prayer.TouchedWhen.ToUniversalTime(), prayer.publicView, selectedFriends, prayer.IfExecutedGUID, ref prayerid);
                else
                {
                    db.usp_GetCreatedPrayerFromQueueActionGUID((long?)user.Id, prayer.IfExecutedGUID, ref prayerid);
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = "EXISTS-" + prayerid.ToString() });
                }

                if (prayer.attachments != null)
                {
                    foreach (PrayerAttachment att in prayer.attachments)
                    {
                        long? attachmentID = -1;
                        string extension = att.FileName.Substring(att.FileName.LastIndexOf('.'));
                        string fileFrom = QuickReference.PrayerAttachmentImageTemp + @"\" + user.Id.ToString() + @"\" + att.GUID + extension;
                        string fileTo = QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString() + @"\" + att.GUID + extension;
                        if(!System.IO.Directory.Exists(QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString()))
                            System.IO.Directory.CreateDirectory(QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString());
                        if (System.IO.File.Exists(fileFrom))
                        {
                            System.IO.File.Move(fileFrom, fileTo);
                            db.usp_AddNewPrayerAttachment(prayerid, att.GUID + extension, att.OriginalFilePath, (long?)user.Id, ref attachmentID);
                        }
                        else
                        {
                            
                        }
                    }
                }
                
                
            }
            return Request.CreateResponse(HttpStatusCode.Created, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.Created, Description = prayerid.ToString() });
        }
        
        [HttpGet]
        [Route("UpdatePublicView")]
        public HttpResponseMessage UpdatePublicView(string QueueActionGUID, string PrayerID, bool PublicView)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                db.usp_UpdatePrayerPublicView(QueueActionGUID, (long?)user.Id, long.Parse(PrayerID), PublicView, ref res);
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }

        [Route("UpdateTagFriends")]
        public HttpResponseMessage UpdateTagFriends(string QueueActionGUID, string PrayerID, List<Friends> selectedFriends)
        {

            XElement t = new XElement("Friends");
            if (selectedFriends != null)
            {
                foreach (Friends friend in selectedFriends)
                {
                    XElement x = new XElement("Friend");
                    x.Add(new XElement("UserID", friend.UserID));
                    t.Add(x);
                }
            }

            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                db.usp_UpdatePrayerTagFriends(QueueActionGUID, (long?)user.Id, long.Parse(PrayerID), t, ref res);
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }

        [Route("AddNewPrayerComment")]
        public HttpResponseMessage AddNewPrayerComment(string QueueActionGUID, string PrayerID, PrayerComment p)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                long? CommentID = -1;
                db.usp_AddNewPrayerComment(QueueActionGUID, (long?)user.Id, long.Parse(p.OwnerPrayerID), p.Comment, p.CreatedWhen.ToUniversalTime(), p.TouchedWhen.ToUniversalTime(), ref res, ref CommentID);
                if (res.ToUpper() == "OK")
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = "OK-" + CommentID.ToString() });
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }

        [HttpGet]
        [Route("DeletePrayerComment")]
        public HttpResponseMessage DeletePrayerComment(string QueueActionGUID, string CommentID)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                try
                {
                    String res = "";
                    db.usp_DeletePrayerComment(QueueActionGUID, (long?)user.Id, long.Parse(CommentID), ref res);
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = "NOTEXISTS" });
                }
            }
        }

        [Route("UpdatePrayerComment")]
        public HttpResponseMessage UpdatePrayerComment(string QueueActionGUID, PrayerComment p)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";               
                db.usp_UpdatePrayerComment(QueueActionGUID, (long?)user.Id, long.Parse(p.CommentID), p.Comment, p.TouchedWhen.ToUniversalTime(), ref res);
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }
    }
}
