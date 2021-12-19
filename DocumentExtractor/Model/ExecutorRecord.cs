using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DocumentExtractor.Model
{
    public class ExecutorRecord
    {
        [Key]
        public string Guid { get; set; }
        public int? RecordDataId { get; set; }

        public string Info { get; set; }

        public string OutputNumber { get; set; }

        public int OutputDivisionId { get; set; }

        public string IdentifiersJson { get; set; }

        public long OutputNumberDate { get; set; }

        public int IsEmpty { get; set; }

        public int HasCd { get; set; }

        public int IsExported { get; set; }
        
    }
}
