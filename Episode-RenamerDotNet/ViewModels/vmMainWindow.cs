using Episode_RenamerDotNet.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Episode_RenamerDotNet.ViewModels
{
    internal class vmMainWindow : INotifyPropertyChanged
    {
        #region Private Variables
        private int _seasonValue = 1;
        private int _episodeValue = 1;
        private string _FolderSource = "";
        private string _FolderDestination = "";
        private string _ReplacePattern = "S{SeasonValue}_E{EpisodeValue}";
        private List<string> _RenameLog = new List<string>();
        private List<string> _Files = new List<string>();
        #endregion

        #region Public Variables
        public string FolderSource
        {
            get
            {
                return _FolderSource;
            }
            set
            {
                _FolderSource = value;
                NotifyPropertyChanged(nameof(FolderSource));
            }
        }
        public string FolderDestination
        {
            get
            {
                return _FolderDestination;
            }
            set
            {
                _FolderDestination = value;
                NotifyPropertyChanged(nameof(FolderDestination));
            }
        }
        public string ReplacePattern
        {
            get
            {
                return _ReplacePattern;
            }
            set
            {
                _ReplacePattern = value;
                NotifyPropertyChanged(nameof(ReplacePattern));
            }
        }
        public string SeasonValue
        {
            get
            {
                return _seasonValue.ToString();
            }
            set
            {
                if (int.TryParse(value, out int result))
                {
                    _seasonValue = result;
                }
                else
                {
                    _seasonValue = 1;
                }
                NotifyPropertyChanged(nameof(SeasonValue));
            }
        }
        public string EpisodeValue
        {
            get
            {
                return _episodeValue.ToString();
            }
            set
            {
                if (int.TryParse(value, out int result))
                {
                    _episodeValue = result;
                }
                else
                {
                    _episodeValue = 1;
                }
                NotifyPropertyChanged(nameof(EpisodeValue));
            }
        }

        public ObservableCollection<string> RenameLog
        {
            get
            {
                return new ObservableCollection<string>(_RenameLog);
            }
            set
            {
                _RenameLog = value.ToList();
                NotifyPropertyChanged(nameof(RenameLog));
            }
        }
        public ObservableCollection<string> Files
        {
            get
            {
                return new ObservableCollection<string>(_Files);
            }
            set
            {
                _Files = value.ToList();
                NotifyPropertyChanged(nameof(Files));
            }
        }
        #endregion

        #region Delegates

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Commands
        public ICommand Command_SelectFolderSource { get; set; }
        public ICommand Command_Rename { get; set; }
        public ICommand Command_SelectFolderDestination { get; set; }
        #endregion

        public void NotifyPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public vmMainWindow()
        {
            Command_SelectFolderSource = new CommandAction(SelectFolderSource);
            Command_Rename = new CommandAction(Rename);
            Command_SelectFolderDestination = new CommandAction(SelectFolderDestination);
        }

        public void SelectFolderSource()
        {
            try
            {
                OpenFolderDialog ofd = new OpenFolderDialog();
                if (ofd.ShowDialog().Value)
                {
                    FolderSource = ofd.FolderName;
                    _Files.Clear();
                    foreach (string FilePath in System.IO.Directory.GetFiles(FolderSource))
                    {
                        string FileName = System.IO.Path.GetFileName(FilePath);
                        _Files.Add(FileName);
                    }
                    NotifyPropertyChanged(nameof(Files));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int i = 0;
        }
        public void Rename()
        {
            try
            {
                List<string> files = (from obj in System.IO.Directory.GetFiles(FolderSource) orderby obj ascending select obj).ToList();
                foreach (string filePath in System.IO.Directory.GetFiles(FolderSource))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        string fileName = System.IO.Path.GetFileName(filePath);
                        string FileExtension = System.IO.Path.GetExtension(filePath);
                        string newFileName = ReplacePattern.Replace("{SeasonValue}", SeasonValue).Replace("{EpisodeValue}", EpisodeValue) + FileExtension;
                        string CombinedNewFileName = System.IO.Path.Combine(FolderDestination, newFileName);
                        System.IO.File.Move(filePath, CombinedNewFileName);
                        _RenameLog.Add($"{fileName} verschoben nach {newFileName}");
                    }
                    else
                    {
                        MessageBox.Show($"Datei\r\n{filePath}\r\nnicht gefunden");
                    }
                    EpisodeValue = (int.Parse(EpisodeValue) + 1).ToString();
                }
                NotifyPropertyChanged(nameof(RenameLog));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SelectFolderDestination()
        {
            try
            {
                OpenFolderDialog ofd = new OpenFolderDialog();
                if (ofd.ShowDialog().Value)
                {
                    FolderDestination = ofd.FolderName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
