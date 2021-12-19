using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace DocumentExtractor.Model
{
    public class ExecutorRecordData
    {
        public int Id { get; set; }
        public string RecordGuid { get; set; }
        public uint BytesSize { get; set; }
        public byte[] Data { get; set; }
    }
}
