using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LaboratorySystem
{
    public class HtmlRendering
    {
        public static string ReplaceParameterOfHtmlTemplate(string HtmlTemplateFile, Dictionary<string, string> values)
        {

            try
            {
                string templateHtml = File.ReadAllText(HtmlTemplateFile);
                // Populate your dictionary here
                if (!String.IsNullOrEmpty(templateHtml))
                {
                    if (values != null)
                    {
                        foreach (KeyValuePair<string, string> kvp in values)
                        {
                            templateHtml = templateHtml.Replace(kvp.Key, kvp.Value);
                        }
                    }
                }
                return templateHtml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}