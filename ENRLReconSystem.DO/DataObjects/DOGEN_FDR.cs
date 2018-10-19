using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_FDR
    {
        public string SubmissionID { get; set; }
        public string TransactionTypeLValue { get; set; }
        public string ExcelFileName { get; set; }
        public string ExcelFilePath { get; set; }
        public long TotalRecordCount { get; set; }

        public long FDRBulkImportID { get; set; }

        public string ContractNumber { get; set; }

        public DataTable dtExcelData;

    }
}
