using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class ExcelCreationConfig
    {
        public string PdfFileslocation { get; set; }
        public string TemplateFilelocation { get; set; }
        public string TemplateFileName { get; set; }
        public string NewFilesLocation { get; set; }
        public string NewExcelFileNamingConvention { get; set; }
        public List<int> StartingRows { get; set; }
        public List<FileInfo> LstPdffiles { get; set; }
        public long LbgpMasterId { get; set; }
        public FDRSubmissionCategory SubmissionCategory { get; set; }
        public DateTime FilterEndDate { get; set; }
        public DateTime FilterStartDate { get; set; }
    }
}
