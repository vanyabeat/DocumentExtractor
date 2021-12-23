using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using DocumentExtractor.Model.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.XlsIO.Parser.Biff_Records;

namespace DocumentExtractor.Model
{
    public class DataWorker
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary()
        {
            Source = new Uri(@"pack://application:,,,/Resources/StringResource.xaml")
        };

        private static void SetExported(string guid, string databaseName, string host, string user, string password, string port)
        {
            try
            {
                using var db = new ApplicationContext(databaseName, host, user, password, port);
                db.ExecutorRecords.FirstOrDefault(p => p.Guid == guid)!.IsExported = 1;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return;
            }
        }
        public static List<object> GetExecutorRecordPlainObject(string databaseName, string host, string user,
            string password, string port)
        {

            try
            {
                var result = new List<object>();
                using var db = new ApplicationContext(databaseName, host, user, password, port);
                var records = db.ExecutorRecords.Where(p => p.IsExported == 0).ToList();
                foreach (var record in records)
                {
                    // record.IsExported = 1;
                    // db.SaveChangesAsync();
                    SetExported(record.Guid, databaseName, host, user, password, port);

                    string blobBase64 = null;

                    if (record.RecordDataId != null)
                    {
                        var d = db.ExecutorRecordDatas.FirstOrDefault(r => r.Id == record.RecordDataId)?.Data;
                        if (d != null) blobBase64 = Convert.ToBase64String(d, 0, d.Length);
                    }
                    result.Add(new
                    {
                        Guid = record.Guid,
                        HasCd = record.HasCd,
                        IdentifiersJson = record.IdentifiersJson,
                        Info = record.Info,
                        IsEmpty = record.IsEmpty,
                        OutputDivisionId = record.OutputDivisionId,
                        OutputNumber= record.OutputNumber,
                        OutputNumberDate = record.OutputNumberDate,
                        BlobData = blobBase64
                    });
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
