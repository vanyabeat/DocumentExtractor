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

        public static SortedSet<Credential> Credentials { get; private set; }

        public StaticData(string path)
        {
            try
            {
                Credentials = JsonConvert.DeserializeObject<SortedSet<Credential>>(File.ReadAllText(path));

            }
            catch (Exception e)
            {
                Credentials = new SortedSet<Credential>();
            }

            _lazy = new Lazy<StaticData>(() => new StaticData(path));
        }
    }

  
}