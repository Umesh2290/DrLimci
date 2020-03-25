using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LaboratorySystem
{
    //to dwonload file use this
    // Response.AddHeader("Content-Disposition", "attachment; filename=download.zip");
    // return File(fileBytes, "application/zip");

    public class ZipManager
    {
        public static FileManager ListOfFilesToZipFile(string ZipName,List<FileManager> sourceFiles)
        {

            // ...

            // the output bytes of the zip
            byte[] fileBytes = null;

            // create a working memory stream
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                // create a zip
                using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {
                    // interate through the source files
                    foreach (FileManager f in sourceFiles)
                    {
                        // add the item name to the zip
                        System.IO.Compression.ZipArchiveEntry zipItem = zip.CreateEntry(f.Name + f.Extension);
                        // add the item bytes to the zip entry by opening the original file and copying the bytes 
                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(f.FileBytes))
                        {
                            using (System.IO.Stream entryStream = zipItem.Open())
                            {
                                originalFileMemoryStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
                fileBytes = memoryStream.ToArray();
            }

            FileManager fm = new FileManager();
            fm.Name = ZipName;
            fm.Extension = ".zip";
            fm.MimeType = "application/zip";
            fm.FileBytes = fileBytes;

            // download the constructed zip
            return fm;
        }
    }
}