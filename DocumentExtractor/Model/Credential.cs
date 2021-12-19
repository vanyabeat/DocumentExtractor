using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentExtractor.Model
{
    public class Credential
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string DataBase { get; set; }
    }
}
