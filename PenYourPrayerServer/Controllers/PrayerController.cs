﻿using Newtonsoft.Json.Serialization;
using PenYourPrayerServer.Models;
using PenYourPrayerServer.Models.Struct;
using PenYourPrayerServer.Supporting;
using PenYourPrayerServer.Supporting.HMAC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;

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
        [Route("GetLatestPrayers")]
        public HttpResponseMessage GetLatestPrayers(string PrayerID)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                List<Prayer> latestprayer = new List<Prayer>();
                List<usp_GetLatestPrayersResult> p = db.usp_GetLatestPrayers((long?)user.Id, long.Parse(PrayerID)).ToList();
                foreach (usp_GetLatestPrayersResult t in p)
                {
                    Prayer prayer = new Prayer();
                    prayer.PrayerID = t.PrayerID;
                    var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                    
                    prayer.TouchedWhen = new DateTime(t.TouchedWhen.Ticks, DateTimeKind.Local).AddTicks(offset.Ticks);
                    prayer.CreatedWhen = new DateTime(t.CreatedWhen.Ticks, DateTimeKind.Local).AddTicks(offset.Ticks);
                    prayer.Content = t.PrayerContent;
                    prayer.publicView = t.PublicView;
                    prayer.IfExecutedGUID = t.QueueActionGUID;
                    if (t.TagFriends != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AllFriends));
                        TextReader reader = new StringReader(t.TagFriends.ToString());

                        prayer.selectedFriends = ((AllFriends)serializer.Deserialize(reader)).friends;
                        reader.Close();                       
                    }
                    if (t.Attachments != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AllPrayerAttachment));
                        TextReader reader = new StringReader(t.Attachments.ToString());

                        prayer.attachments = ((AllPrayerAttachment)serializer.Deserialize(reader)).attachments;
                        reader.Close();
                    }
                    if (t.Comment != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AllPrayerComments));
                        TextReader reader = new StringReader(t.Comment.ToString());

                        prayer.comments = ((AllPrayerComments)serializer.Deserialize(reader)).comments;
                        reader.Close();
                    }
                    if (t.Answers != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AllPrayerAnswered));
                        TextReader reader = new StringReader(t.Answers.ToString());

                        prayer.answers = ((AllPrayerAnswered)serializer.Deserialize(reader)).answers;
                        reader.Close();
                    }
                    if (t.Amen != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AllPrayerAmen));
                        TextReader reader = new StringReader(t.Amen.ToString());

                        prayer.amen = ((AllPrayerAmen)serializer.Deserialize(reader)).amen;
                        reader.Close();
                    }

                    latestprayer.Add(prayer);
                }

                var formatter = new JsonMediaTypeFormatter();
                var json = formatter.SerializerSettings;
                json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind;

                return Request.CreateResponse(HttpStatusCode.OK, latestprayer, formatter);
                //return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }

        [HttpGet]
        [Route("DeletePrayer")]
        public HttpResponseMessage DeletePrayer(string QueueActionGUID, string PrayerID)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                db.usp_DeletePrayer(QueueActionGUID, (long?)user.Id, long.Parse(PrayerID));
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
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
    
        [HttpGet]
        [Route("AddNewPrayerAmen")]
        public HttpResponseMessage AddNewPrayerAmen(string QueueActionGUID, string PrayerID){
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                long? AmenID = -1;
                String res = "";       
                db.usp_AddNewPrayerAmen(QueueActionGUID, (long?)user.Id, long.Parse(PrayerID), ref res, ref AmenID);
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }

        [HttpGet]
        [Route("DeletePrayerAmen")]
        public HttpResponseMessage DeletePrayerAmen(string QueueActionGUID, string PrayerID)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                long? AmenID = -1;
                String res = "";
                db.usp_DeletePrayerAmen(QueueActionGUID, (long?)user.Id, long.Parse(PrayerID));
                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = "OK" });
            }
        }

        [Route("AddNewPrayerAnswered")]
        public HttpResponseMessage AddNewPrayerAnswered(string QueueActionGUID, string PrayerID, PrayerAnswered p)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                long? AnsweredID = -1;
                db.usp_AddNewPrayerAnswered(QueueActionGUID, (long?)user.Id, long.Parse(p.OwnerPrayerID), p.Answered, p.CreatedWhen.ToUniversalTime(), p.TouchedWhen.ToUniversalTime(), ref res, ref AnsweredID);
                if (res.ToUpper() == "OK")
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = "OK-" + AnsweredID.ToString() });
                else
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });
            }
        }

        [Route("AddNewPrayerRequest")]
        public HttpResponseMessage AddNewPrayerRequest(string QueueActionGUID, PrayerRequest p)
        {
            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            using (DBDataContext db = new DBDataContext())
            {
                String res = "";
                long? PrayerRequestID = -1;
                db.usp_AddNewPrayerRequest(QueueActionGUID, (long?)user.Id, p.Subject, p.Description, p.CreatedWhen.ToUniversalTime(), p.TouchedWhen.ToUniversalTime(), ref res, ref PrayerRequestID);
                if (res.ToUpper() != "OK")                    
                    return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = res });


                if (p.attachments != null)
                {
                    foreach (PrayerRequestAttachment att in p.attachments)
                    {
                        long? attachmentID = -1;
                        string extension = att.FileName.Substring(att.FileName.LastIndexOf('.'));
                        string fileFrom = QuickReference.PrayerAttachmentImageTemp + @"\" + user.Id.ToString() + @"\" + att.GUID + extension;
                        string fileTo = QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString() + @"\" + att.GUID + extension;
                        if (!System.IO.Directory.Exists(QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString()))
                            System.IO.Directory.CreateDirectory(QuickReference.PrayerAttachmentImage + @"\" + user.Id.ToString());
                        if (System.IO.File.Exists(fileFrom))
                        {
                            System.IO.File.Move(fileFrom, fileTo);
                            db.usp_AddNewPrayerRequestAttachment(PrayerRequestID, att.GUID + extension, att.OriginalFilePath, (long?)user.Id, ref attachmentID);
                        }
                        else
                        {

                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new CustomResponseMessage() { StatusCode = (int)HttpStatusCode.OK, Description = "OK-" + PrayerRequestID.ToString() });
            
            }
        }

        [HttpPost]
        [Route("GetLatestPrayerRequest")]
        public HttpResponseMessage GetLatestPrayerRequest(string Useless, List<PrayerRequest> pr)
        {
            String id = "";
            foreach (PrayerRequest prayerRequest in pr)
            {
                id += prayerRequest.PrayerRequestID + ";";
            }

            PenYourPrayerIdentity user = (PenYourPrayerIdentity)User.Identity;
            List<PrayerRequest> newPrayerRequest = new List<PrayerRequest>();
            using (DBDataContext db = new DBDataContext())
            {
                List<usp_GetLatestPrayerRequestResult> res = db.usp_GetLatestPrayerRequest(user.Id, id).ToList();
                foreach (usp_GetLatestPrayerRequestResult f in res)
                {

                    PrayerRequest temp = (PrayerRequest)f;

                    if (f.Attachments != null)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AllPrayerRequestAttachment));
                        TextReader reader = new StringReader(f.Attachments.ToString());

                        temp.attachments = ((AllPrayerRequestAttachment)serializer.Deserialize(reader)).attachments;
                        reader.Close();
                    }

                    newPrayerRequest.Add(temp);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, newPrayerRequest);
        }
    }
}