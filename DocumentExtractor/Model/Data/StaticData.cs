using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DocumentExtractor.Model.Data
{
    public class StaticData
    {
        private static Lazy<StaticData> _lazy;

        public static StaticData Instance => _lazy.Value;

        public static List<Credential>? Credentials { get; private set; }

        public StaticData(string path)
        {
            try
            {
                Credentials = JsonConvert.DeserializeObject<List<Credential>>(File.ReadAllText(path));

            }
            catch (Exception e)
            {
                Credentials = new List<Credential>();
            }

            _lazy = new Lazy<StaticData>(() => new StaticData(path));
        }
    }

  
}