using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Reflection;

namespace KronosCameraTestApp.Helpers
{
    public static class SendOutlookMail
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(
             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        public enum BodyType
        {
            PlainText,
            RTF,
            HTML
        }

        public static async Task<bool> sendEmailViaOutlook(string sFromAddress, string sToAddress, string sCc, string sSubject, string sBody, BodyType bodyType, List<string> arrAttachments = null, string sBcc = null)
        {
            //Send email via Office Outlook 2010
            //'sFromAddress' = email address sending from (ex: "me@somewhere.com") -- this account must exist in Outlook. Only one email address is allowed!
            //'sToAddress' = email address sending to. Can be multiple. In that case separate with semicolons or commas. (ex: "recipient@gmail.com", or "recipient1@gmail.com; recipient2@gmail.com")
            //'sCc' = email address sending to as Carbon Copy option. Can be multiple. In that case separate with semicolons or commas. (ex: "recipient@gmail.com", or "recipient1@gmail.com; recipient2@gmail.com")
            //'sSubject' = email subject as plain text
            //'sBody' = email body. Type of data depends on 'bodyType'
            //'bodyType' = type of text in 'sBody': plain text, HTML or RTF
            //'arrAttachments' = if not null, must be a list of absolute file paths to attach to the email
            //'sBcc' = single email address to use as a Blind Carbon Copy, or null not to use
            //RETURN:
            //      = true if success
            bool bRes = false;

            try
            {
                await Task.Run(() =>
                {
                    log.Info("SendOutlookMail Begin Sending Outlook email");
                    //Get Outlook COM objects
                    Outlook.Application app = new Outlook.Application();
                    Outlook.MailItem newMail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);

                    //Parse 'sToAddress'
                    if (!string.IsNullOrWhiteSpace(sToAddress))
                    {
                        string[] arrAddTos = sToAddress.Split(new char[] { ';', ',' });
                        foreach (string strAddr in arrAddTos)
                        {
                            if (!string.IsNullOrWhiteSpace(strAddr) &&
                                strAddr.IndexOf('@') != -1)
                            {
                                newMail.Recipients.Add(strAddr.Trim());
                            }
                            else
                            {
                                log.Error("SendOutlookMail Bad to-address while sending email" + sToAddress);
                                throw new Exception("Bad to-address: " + sToAddress);
                            }

                        }
                    }
                    else
                    {
                        log.Error("SendOutlookMail Must specify to-address while sending email" + sToAddress);
                        throw new Exception("Must specify to-address");
                    }


                    //Parse 'sCc'
                    if (!string.IsNullOrWhiteSpace(sCc))
                    {
                        string[] arrAddTos = sCc.Split(new char[] { ';', ',' });
                        foreach (string strAddr in arrAddTos)
                        {
                            if (!string.IsNullOrWhiteSpace(strAddr) &&
                                strAddr.IndexOf('@') != -1)
                            {
                                newMail.Recipients.Add(strAddr.Trim());
                            }
                            else
                            {
                                log.Error("SendOutlookMail Bad CC-address:" + sCc);
                                throw new Exception("Bad CC-address: " + sCc);
                            }

                        }
                    }

                    //Is BCC empty?
                    if (!string.IsNullOrWhiteSpace(sBcc))
                    {
                        newMail.BCC = sBcc.Trim();
                    }
                    //for(int i =1)
                    //Resolve all recepients
                    if (!newMail.Recipients.ResolveAll())
                    {
                        log.Error("SendOutlookMail Failed to resolve all recipients: " + sToAddress + ";" + sCc);
                        throw new Exception("Failed to resolve all recipients: " + sToAddress + ";" + sCc);
                    }


                    //Set type of message
                    switch (bodyType)
                    {
                        case BodyType.HTML:
                            newMail.HTMLBody = sBody;
                            break;
                        case BodyType.RTF:
                            newMail.RTFBody = sBody;
                            break;
                        case BodyType.PlainText:
                            newMail.Body = sBody;
                            break;
                        default:
                            {
                                log.Error("SendOutlookMail Bad email body type: " + bodyType);
                                throw new Exception("Bad email body type: " + bodyType);
                            }

                    }


                    if (arrAttachments != null)
                    {
                        //Add attachments
                        foreach (string strPath in arrAttachments)
                        {
                            if (File.Exists(strPath))
                            {
                                newMail.Attachments.Add(strPath);
                            }
                            else
                            {
                                log.Error("SendOutlookMail Attachment file is not found: \"" + strPath + "\"");
                                throw new Exception("Attachment file is not found: \"" + strPath + "\"");
                            }

                        }
                    }

                    //Add subject
                    if (!string.IsNullOrWhiteSpace(sSubject))
                        newMail.Subject = sSubject;

                    Outlook.Accounts accounts = app.Session.Accounts;
                    Outlook.Account acc = null;

                    //Look for our account in the Outlook
                    foreach (Outlook.Account account in accounts)
                    {
                        if (account.SmtpAddress.Equals(sFromAddress, StringComparison.CurrentCultureIgnoreCase))
                        {
                            //Use it
                            acc = account;
                            break;
                        }
                    }

                    //Did we get the account
                    if (acc != null)
                    {
                        //Use this account to send the e-mail. 
                        newMail.SendUsingAccount = acc;

                        //And send it
                        ((Outlook._MailItem)newMail).Send();

                        //Done
                        bRes = true;
                    }
                    else
                    {
                        log.Error("Account does not exist in Outlook:" + sFromAddress);
                        throw new Exception("Account does not exist in Outlook: " + sFromAddress);
                    }
                });
                return bRes;
            }
            catch (Exception ex)
            {
                log.Error("ERROR: Failed to send mail: " + ex.Message);
                //Console.WriteLine("ERROR: Failed to send mail: " + ex.Message);
            }

            return bRes;
        }
        public static async Task<bool> SendMail_SMTP(string sFromAddress, string sToAddress, string sCc, string sSubject, string sBody,string emailPassword, List<string> arrAttachments = null)
        {
            bool bRes = false;
            try
            {
                await Task.Run(() =>
                {
                    log.Info("SendOutlookMail Begin Sending Outlook email");
                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    var mail = new MailMessage();
                    mail.From = new MailAddress(sFromAddress);
                    mail.Subject = sSubject;
                    mail.Body = sBody;
                    mail.IsBodyHtml = false;
                    string[] CCMuliId =sCc.Split(',');
                    foreach (string ToEMailId in CCMuliId)
                    {
                        if (!string.IsNullOrWhiteSpace(ToEMailId))
                            mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                    }
                    string[] ToMuliId = sToAddress.Split(',');
                    foreach (string ToEMailId in ToMuliId)
                    {
                        mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                    }
                    foreach(string attachment in arrAttachments) 
                    {
                        mail.Attachments.Add(new Attachment(attachment));
                    }

                    System.Net.NetworkCredential credentials =
                      new System.Net.NetworkCredential(sFromAddress, emailPassword);
                    client.EnableSsl = true;
                    client.Credentials = credentials;

                    client.Send(mail);
                    bRes= true;
                });
                return bRes;
            }
            catch (Exception ex)
            {
                log.Error("Exception in" + MethodBase.GetCurrentMethod(), ex);
                return bRes;
            }

        }
    }
}
