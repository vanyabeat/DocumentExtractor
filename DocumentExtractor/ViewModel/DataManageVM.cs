using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DocumentExtractor.Annotations;
using DocumentExtractor.Model;
using DocumentExtractor.Model.Data;
using DocumentExtractor.View;
using DocumentVisor.Infrastructure;
using Microsoft.Win32;
using Syncfusion.Data.Extensions;

namespace DocumentExtractor.ViewModel
{
	public class DataManageVm : INotifyPropertyChanged
	{
		private static readonly ResourceDictionary Settings = new ResourceDictionary
		{
			Source = new Uri(@"pack://application:,,,/settings.xaml")
		};
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary()
        {
            Source = new Uri(@"pack://application:,,,/Resources/StringResource.xaml")
        };

		#region DataBase

		public static string SelectedUserName { get; set; } = Settings["User"].ToString();
		public static string SelectedPassword { get; set; } = Settings["Password"].ToString();
		public static string SelectedHost { get; set; } = Settings["Host"].ToString();
		public static string SelectedDatabase { get; set; } = Settings["DataBase"].ToString();
		public static string SelectedPort { get; set; } = Settings["Port"].ToString();
		public static string SelectedCredentials { get; set; } = Settings["PathDataTypes"].ToString();
		
		// private static readonly ResourceDictionary Dictionary = new ResourceDictionary
		// {
		//     Source = new Uri(@"pack://application:,,,/Resources/StringResource.xaml")
		// };

		public static StaticData StaticTextData;
		#endregion
        #region Login

	

		private readonly RelayCommand<object> _openPathWindow = null;

		public RelayCommand<object> OpenPathWindow
		{
			get
			{
				return _openPathWindow ?? new RelayCommand<object>(obj =>
					{
						var window = obj as Window;
						var folderBrowser = new OpenFileDialog
						{
							ValidateNames = false,
							CheckFileExists = true,
							CheckPathExists = true,
							FileName = "Select file Selection."
						};
						if ((bool) folderBrowser.ShowDialog())
						{
							SelectedCredentials = Path.GetFullPath(folderBrowser.FileName);
							if (window?.FindName("PathTextBox") is TextBox textBox) textBox.Text = SelectedCredentials;
						}
					}
				);
			}
		}

		#endregion
		#region MVVM

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

        private readonly AsyncRelayCommand<object> _exportJson = null;

		public AsyncRelayCommand<object> ExportJson
        {
            get
            {
                return _exportJson ?? new AsyncRelayCommand<object>(obj =>
                    {
                        var window = obj as Window;
                        var textBox = window.FindName("LogTextBox") as TextBox;
                        var p = new StaticData(SelectedCredentials);
                        var credentials = StaticData.Credentials.ToArray();
                        var result = new List<object>();
                        foreach (var cred in credentials)
                        {
                            result.AddRange(DataWorker.GetExecutorRecordPlainObject(cred.DataBase, cred.Host, cred.User, cred.Password, cred.Port));
                            textBox.Text += $"Connect to {cred}\n";
                        }
						SaveFileDialog dialog = new SaveFileDialog()
                        {
							FileName = $"{DateTime.Now:dd_MM_yyyy}_exportdata",
                            Filter = "Text Files(*.json)|*.json|All(*.*)|*"
                        };

                        if (dialog.ShowDialog() == true)
                        {
                            File.WriteAllText(dialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(result));
                        }
						return Task.CompletedTask;
                    }
                );
            }
        }

		private void SetCenterPositionAndOpenDataBaseWindow(Window window)
		{
			Application.Current.MainWindow = window;
			window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			window.Show();
		}


		private void SetCenterPositionAndOpen(Window window)
		{
			window.Owner = Application.Current.MainWindow;
			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			window.ShowDialog();
		}




		



		private void SetRedBlockControl(Window window, string blockName)
		{
			var block = window.FindName(blockName) as Control;
			block.BorderBrush = Brushes.Crimson;
		}

		private void ShowMessageToUser(string message)
		{
			var wnd = new MessageView(message);
			SetCenterPositionAndOpen(wnd);
		}

		private static async Task<byte[]> GetZipBytesFromFolder(string path)
		{
			byte[] archiveFile;
			await using (var archiveStream = new MemoryStream())
			{
				using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
				{
					foreach (var file in Directory.GetFiles(path,
								 "*.*",
								 SearchOption.AllDirectories))
					{
						var zipArchiveEntry = archive.CreateEntry(file, CompressionLevel.Fastest);

						await using var zipStream = zipArchiveEntry.Open();
						var fileContent = await File.ReadAllBytesAsync(file);
						await zipStream.WriteAsync(fileContent, 0, fileContent.Length);
					}
				}
				archiveFile = archiveStream.ToArray();
			}
			return await Task.FromResult<byte[]>(archiveFile); 
		}
	}
}