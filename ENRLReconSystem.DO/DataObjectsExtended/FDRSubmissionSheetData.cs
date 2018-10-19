using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class FDRSubmissionSheetData
    {
        public FDRSubmissionSheetData()
        {
            LstRowData = new List<FDRSubmissionRow>();
        }
        public List<FDRSubmissionRow> LstRowData { get; set; }
        public int ISheetNumber { get; set; }
        public int IStartRow { get; set; }
}
}
