using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace LaboratorySystem
{
    public class EmailManager
    {

        public static bool SendEMail(string configKey,string to, string cc,string bcc, string subject, string body, List<FileManager> attachmentsFile)
        {
            try
            {
                SmtpClient SmtpClient = null;

                string config = WebConfigurationManager.AppSettings[configKey];
                string[] parameters = config.Split(';');

                SmtpClient = new SmtpClient
                {
                    Host = parameters[0],
                    Port = Convert.ToInt32(parameters[1]),
                    EnableSsl = Convert.ToBoolean(parameters[2]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(parameters[3]),
                    Credentials = new NetworkCredential(parameters[4], parameters[5])
                };
                    //create the MailMessage object
                    MailMessage mMailMessage = new MailMessage();

                    //set the sender address of the mail message
                    if (!string.IsNullOrEmpty(parameters[6]))
                    {
                        mMailMessage.From = new MailAddress(parameters[6]);
                    }

                    //set the recipient address of the mail message
                    string[] splittedstrings;
                    if (!string.IsNullOrEmpty(to))
                    {
                        splittedstrings = to.Split(',');
                        foreach (string str in splittedstrings)
                        {
                            mMailMessage.To.Add(new MailAddress(str.Trim()));
                        }
                    }

                    //set the blind carbon copy address
                    if (!string.IsNullOrEmpty(bcc))
                    {
                        splittedstrings = bcc.Split(',');
                        foreach (string str in splittedstrings)
                        {
                            mMailMessage.Bcc.Add(new MailAddress(str));
                        }
                    }

                    //set the carbon copy address
                    if (!string.IsNullOrEmpty(cc))
                    {
                        splittedstrings = cc.Split(',');
                        foreach (string str in splittedstrings)
                        {
                            mMailMessage.CC.Add(new MailAddress(str));
                        }
                    }

                    //set the subject of the mail message
                    if (!string.IsNullOrEmpty(subject))
                    {
                        mMailMessage.Subject = subject;
                    }
                    else
                    {
                        mMailMessage.Subject = "Blank";
                    }

                    //set the body of the mail message
                    mMailMessage.Body = body;

                    //set the format of the mail message body
                    mMailMessage.IsBodyHtml = true;

                    //set the priority
                    mMailMessage.Priority = MailPriority.Normal;

                    //add any attachments from the filesystem
                    if (attachmentsFile != null)
                    {
                        foreach (var attachmentPath in attachmentsFile)
                        {
                            Attachment mailAttachment = new Attachment(new MemoryStream(attachmentPath.FileBytes),attachmentPath.Name+attachmentPath.Extension);
                            mMailMessage.Attachments.Add(mailAttachment);
                        }
                    }

                    //create the SmtpClient instance


                    //send the mail message
                    SmtpClient.Send(mMailMessage);
                    return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }



        /// <summary>
        /// Determines whether an email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>
        /// 	<c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            // An empty or null string is not valid
            if (String.IsNullOrEmpty(emailAddress))
            {
                return (false);
            }

            // Regular expression to match valid email address
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            // Match the email address using a regular expression
            Regex re = new Regex(emailRegex);
            if (re.IsMatch(emailAddress))
                return (true);
            else
                return (false);
        }

    }
}
