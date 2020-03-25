using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace LaboratorySystem
{
    public class PdfManager
    {

        public static FileManager HtmlToPdf(string name,string html)
        {
            //"<body>Hello world: {0}</body>"
            try
            {
                var htmlContent = html;
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);

                FileManager fm = new FileManager();
                fm.Name = name;
                fm.FileBytes = pdfBytes;
                fm.Extension = ".pdf";
                fm.MimeType = "application/pdf";

                return fm;
            }
            catch (Exception e)
            {

            }
            return null;

        }



    }
}