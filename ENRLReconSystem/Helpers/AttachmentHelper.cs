using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ENRLReconSystem.Utility;

namespace ENRLReconSystem.Helpers
{
    public static class AttachmentHelper
    {
        static string _tempFolder = HttpContext.Current.Server.MapPath(ConstantTexts.TempCaseAttachmentPath);
        public static bool SaveFileTemp(HttpPostedFileBase attachment, string strFileName)
        {
            try
            {
                
                string tempPath = Path.Combine(_tempFolder, strFileName);
                attachment.SaveAs(tempPath);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SaveFilePerm(HttpPostedFileBase attachment,string strPermPath, string strFileName)
        {
            try
            {
                string tempPath = Path.Combine(_tempFolder, strFileName);
                File.Delete(tempPath);

                Directory.CreateDirectory(strPermPath);
                string permPath = Path.Combine(strPermPath, strFileName);
                attachment.SaveAs(permPath);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}