using ConvertAllCharactersRules;
using Fluent;
using StringAction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Windows.Shapes;
using System.Xml;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        private void NewFile_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            FileTab.Items.Refresh();
            FolderTab.Items.Refresh();
            FileInFolderTab.Items.Refresh();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            var helpWindows = new help();
            helpWindows.ShowDialog();

        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            String information = "Thành viên nhóm: \n";
            information += "1. 19120622 - Nguyễn Minh Phụng\n";
            information += "2. 19120629 - Lê Hồng Quân\n";
            information += "3. 19120728 - Trương Quốc Vương\n";
            information += "4. 19120729 - Bùi Ngọc Thảo Vy";
            System.Windows.MessageBox.Show(information, "Giới thiệu", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            DialogResult choose = System.Windows.Forms.MessageBox.Show("Are you sure to batch rename", "Batch rename", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (choose == System.Windows.Forms.DialogResult.OK)
            {
                bool isError = false;
                string exception = "";
                //check input from users;
                if (methodListBox.Items.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Add Method Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                else if (FileTab.Items.Count == 0 && FolderTab.Items.Count == 0 && FileInFolderTab.Items.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Choose File Or Folder Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                else
                {
                    //ObservableCollection<FileChoose> FileList = new ObservableCollection<FileChoose>();
                    //ObservableCollection<FolderChoose> FolderList = new ObservableCollection<FolderChoose>();
                    int i = 0;
                    //file process
                    foreach (FileChoose file in FileTab.Items)
                    {
                        string result = file.Filename;

                        foreach (IStringAction action in methodListBox.Items)
                        {
                            result = action.Processor.Invoke(result, i);
                        }

                        var path = System.IO.Path.GetDirectoryName(file.Path);
                        try
                        {
                            var tempfile = new FileInfo(file.Path);
                            tempfile.MoveTo(path + "\\" + result);

                            file.Newfilename = result;
                            file.Filename = result;
                            file.Path = path + "\\" + result;
                        }
                        catch (IOException)
                        {
                            var tempfile = new FileInfo(file.Path);
                            string[] parts = result.Split(".");
                            file.Newfilename = parts[0];
                            int temp = 2;
                            file.Path = path + "\\" + result;
                            while (File.Exists(file.Path))
                            {
                                result = file.Newfilename + " (" + temp.ToString() + ")" + "." + parts[1];
                                file.Path = path + "\\" + result;
                                temp++;
                            }
                            tempfile.MoveTo(file.Path);
                            file.Newfilename = result;
                            file.Filename = result;

                        }
                        catch (Exception k)
                        {
                            isError = true;
                            exception = k.ToString();
                        }
                        ++i;
                    }


                    //file process
                    i = 0;
                    foreach (FileChoose file in FileInFolderTab.Items)
                    {
                        string result = file.Filename;
                        bool caseCheck = true;

                        foreach (IStringAction action in methodListBox.Items)
                        {
                            result = action.Processor.Invoke(result, i);
                            var myArgs = action.Args;

                        }

                        var path = System.IO.Path.GetDirectoryName(file.Path);
                        try
                        {
                            var tempfile = new FileInfo(file.Path);
                            tempfile.MoveTo(path + "\\" + result);

                            file.Newfilename = result;
                            file.Filename = result;
                            file.Path = path + "\\" + result;
                        }
                        catch (IOException)
                        {
                            var tempfile = new FileInfo(file.Path);
                            string[] parts = result.Split(".");
                            file.Newfilename = parts[0];
                            int temp = 2;
                            file.Path = path + "\\" + result;
                            while (File.Exists(file.Path))
                            {
                                result = file.Newfilename + " (" + temp.ToString() + ")" + "." + parts[1];
                                file.Path = path + "\\" + result;
                                temp++;
                            }
                            tempfile.MoveTo(file.Path);
                            file.Newfilename = result;
                            file.Filename = result;
                        }
                        catch (Exception k)
                        {
                            isError = true;
                            exception = k.ToString();
                        }
                        i++;
                    }

                    //folder process
                    i = 0;
                    foreach (FolderChoose folder in FolderTab.Items)
                    {
                        string result = folder.Foldername;

                        foreach (IStringAction action in methodListBox.Items)
                        {
                            result = action.Processor.Invoke(result, i);
                        }
                        string path = System.IO.Path.GetDirectoryName(folder.Path);
                        string newfolderpath = path + "\\" + result;
                        string tempFolderName = "\\Temp";
                        string tempFolderPath = path + tempFolderName;
                        CopyAll(folder.Path, tempFolderPath, true);

                        if (folder.Path.Equals(newfolderpath) == false)
                        {
                            RemoveDirectory(folder.Path);
                            Directory.Delete(folder.Path);

                            folder.Newfolder = result;
                            int temp = 2;
                            while (Directory.Exists(newfolderpath))
                            {
                                result = folder.Newfolder + " (" + temp.ToString() + ")";
                                newfolderpath = path + "\\" + result;
                                temp++;
                            }
                            try
                            {
                                Directory.Move(tempFolderPath, newfolderpath);
                                folder.Newfolder = result;
                                folder.Foldername = result;
                                folder.Path = newfolderpath;
                            }
                            catch (IOException)
                            {
                                Directory.Move(tempFolderPath, newfolderpath);
                                folder.Newfolder = result;
                                folder.Foldername = result;
                                folder.Path = newfolderpath;
                            }
                            catch (Exception k)
                            {
                                isError = true;
                                exception = k.ToString();
                            }
                        }
                        else
                        {
                            RemoveDirectory(tempFolderPath);
                            Directory.Delete(tempFolderPath);
                        }
                        i++;
                    }

                    if (isError)
                    {
                        System.Windows.Forms.MessageBox.Show(exception, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }

                    FolderTab.Items.Refresh();
                    FileTab.Items.Refresh();
                }
            }
        }

        public static void RemoveDirectory(string sourcePath)
        {
            DirectoryInfo src = new DirectoryInfo(sourcePath);

            foreach (var file in src.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in src.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public static void CopyAll(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                System.Windows.MessageBox.Show("Source Directory does not exist or could not be found !");
            }

            if (!Directory.Exists(destDirName))
            {
                DirectoryInfo tempFolder = Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    CopyAll(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void btnAddMethod_Click(object sender, RoutedEventArgs e)
        {
            var prototype = methodComboBox.SelectedItem as IStringAction;
            if (prototype == null)
            {
                System.Windows.Forms.MessageBox.Show("You Have To Choose Method!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return;
            }
            var instance = prototype.Clone();
            methodListBox.Items.Add(instance);
        }

        private void btnEditMethod_Click(object sender, RoutedEventArgs e)
        {
            var action = methodListBox.SelectedItem as IStringAction;
            action.ShowEditDialog();
        }

        private void btnDeleteMethod_Click(object sender, RoutedEventArgs e)
        {
            var item = methodListBox.SelectedItem;
            methodListBox.Items.Remove(item);
        }

        private void AddFileButtons_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect = true;
            if (screen.ShowDialog() == true)
            {
                foreach (var file in screen.FileNames)
                {
                    bool check = true;
                    foreach (FileChoose fileChoose in FileTab.Items)
                    {
                        if (file == fileChoose.Path)
                        {
                            System.Windows.Forms.MessageBox.Show("File was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            check = false;
                            break;
                        }
                    }

                    if (check)
                    {
                        FileTab.Items.Add(new FileChoose()
                        {
                            Filename = System.IO.Path.GetFileName(file),
                            Path = file
                        });
                    }
                }
            }
        }

        List<IStringAction> stringActions;
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                stringActions = new List<IStringAction>();

                string exePath = Assembly.GetExecutingAssembly().Location;
                string folder = System.IO.Path.GetDirectoryName(exePath);
                var dlls = new DirectoryInfo(folder).GetFiles("*.dll");

                foreach (var dll in dlls)
                {
                    //if(dll.FullName== @$"{folder}\ControlzEx.dll")
                    //{
                    //    //var domain = AppDomain.CurrentDomain;
                    //    //Assembly assembly1 = domain.Load(AssemblyName.GetAssemblyName(dll.FullName));
                    //    continue;
                    //}

                    //var domain = AppDomain.CurrentDomain;
                    //Assembly assembly = domain.Load(AssemblyName.GetAssemblyName(dll.FullName));
                    //var types = assembly.GetTypes();

                    var assembly = Assembly.LoadFrom(dll.FullName);
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        if (type.IsClass)
                        {
                            if (typeof(IStringAction).IsAssignableFrom(type))
                            {
                                var action = Activator.CreateInstance(type) as IStringAction;
                                stringActions.Add(action);
                            }
                        }
                    }

                    methodComboBox.ItemsSource = stringActions;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void AddFolderButtons_Click(object sender, RoutedEventArgs e)
        {
            string directory;
            var screen = new FolderBrowserDialog();
            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Add current directory to tab
                directory = screen.SelectedPath;
                bool check = true;
                foreach (FolderChoose folderChoose in FolderTab.Items)
                {
                    if (directory == folderChoose.Path)
                    {
                        System.Windows.Forms.MessageBox.Show("Folder was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    FolderTab.Items.Add(new FolderChoose()
                    {
                        //Foldername = directory.Substring(directory.Length + 1),
                        Foldername = directory.Split('\\').Last(),
                        Path = directory
                    });
                }

                // Add all file in all subdirectories to file tab
                string[] Files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
                foreach (string fileName in Files)
                {
                    bool fileCheck = true;
                    foreach (FileChoose fileChoose in FileTab.Items)
                    {
                        if (fileName == fileChoose.Path)
                        {
                            System.Windows.Forms.MessageBox.Show("File was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            fileCheck = false;
                            break;
                        }
                    }
                    if (fileCheck)
                    {
                        FileTab.Items.Add(new FileChoose()
                        {
                            Filename = System.IO.Path.GetFileName(fileName),
                            Path = fileName
                        });
                    }
                }


                // Add all subdirectories to tab
                string[] subs = Directory.GetDirectories(directory, "", SearchOption.AllDirectories);
                foreach (string sub in subs)
                {
                    bool folderCheck = true;
                    foreach (FolderChoose folder in FolderTab.Items)
                    {
                        if (sub == folder.Path)
                        {
                            System.Windows.Forms.MessageBox.Show("Folder was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            folderCheck = false;
                            break;
                        }
                    }

                    if (folderCheck)
                    {
                        FolderTab.Items.Add(new FolderChoose()
                        {
                            Foldername = sub.Split('\\').Last(),
                            Path = sub
                        });
                    }
                }
            }
        }

        private void btnClearAllFileFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FileTab.SelectedItems.Count != 0 || FolderTab.SelectedItems.Count != 0 || FileInFolderTab.SelectedItems.Count != 0)
            {
                while (FileTab.SelectedItems.Count != 0)
                    FileTab.Items.Remove(FileTab.SelectedItem);

                while (FolderTab.SelectedItems.Count != 0)
                    FolderTab.Items.Remove(FolderTab.SelectedItem);

                while (FileInFolderTab.SelectedItems.Count != 0)
                    FileInFolderTab.Items.Remove(FileInFolderTab.SelectedItem);
                return;
            }
            FileTab.ItemsSource = null;
            FileTab.Items.Clear();
            FolderTab.ItemsSource = null;
            FolderTab.Items.Clear();
            FileInFolderTab.ItemsSource = null;
            FileInFolderTab.Items.Clear();
        }

        private void btnClearAllMethod_Click(object sender, RoutedEventArgs e)
        {
            methodListBox.ItemsSource = null;
            methodListBox.Items.Clear();
        }

        private void AddListFileInFolderButtons_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.MessageBox.Show("You selected: " + dialog.SelectedPath);
                DirectoryInfo d = new DirectoryInfo(dialog.SelectedPath);

                string[] files = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.TopDirectoryOnly);

                foreach (string fileName in files)
                {
                    bool check = true;
                    foreach (FileChoose fileChoose in FileInFolderTab.Items)
                    {
                        if (fileName == fileChoose.Path)
                        {
                            System.Windows.Forms.MessageBox.Show("File was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        FileInFolderTab.Items.Add(new FileChoose()
                        {
                            Filename = System.IO.Path.GetFileName(fileName),
                            Path = fileName
                        });
                    }
                }
            }
        }

        private void SavePreset_Click(object sender, RoutedEventArgs e)
        {
            if (methodListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing To Save To Preset", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter("preset.txt"))
                {
                    foreach (IStringAction action in methodListBox.Items)
                    {
                        string actionData = $"{action.name} {action.Args.Details}";
                        writer.WriteLine(actionData);
                    }
                }
                System.Windows.Forms.MessageBox.Show("All thing done ^_^", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }
        string presetSave = "";
        private void LoadPreset_Click(object sender, RoutedEventArgs e)
        {

            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.ShowDialog();

            string presetfilename = screen.FileName;
            if (presetfilename == null || presetfilename.Length == 0)
                return;

            presetSave = presetfilename;
            using (StreamReader reader = new StreamReader(presetfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Replace action:"))
                    {
                        foreach (IStringAction i in stringActions)
                        {
                            if (line.Contains(i.name))
                            {
                                var action = i.Clone();
                                string needle = line.Substring("Replace action: Replace ".Length, line.IndexOf(" with") - "Replace action: Replace ".Length);
                                string hammer = line.Substring(line.IndexOf("with") + "with".Length + 1);
                                List<String> list = new List<string>();
                                list.Add(needle);
                                list.Add(hammer);
                                action.Args.setValueChild(list);
                                methodListBox.Items.Add(action);
                            }
                        }
                    }
                    else if (line.Contains("Convert All Character:"))
                    {
                        foreach (IStringAction i in stringActions)
                        {
                            if (line.Contains(i.name))
                            {
                                var action = i.Clone();
                                string token = line.Substring("Convert All Character: Convert all character with ".Length);
                                List<String> list = new List<string>();
                                list.Add(token);
                                action.Args.setValueChild(list);
                                methodListBox.Items.Add(action);
                            }
                        }

                    }
                    else if (line.Contains("Change extention action: "))
                    {
                        foreach (IStringAction i in stringActions)
                        {
                            if (line.Contains(i.name))
                            {
                                var action = i.Clone();
                                string needle = line.Substring("Change extention action: Change Extension ".Length, line.IndexOf(" with") - "Change extention action: Change Extension ".Length);
                                string hammer = line.Substring(line.IndexOf("with") + "with".Length + 1);
                                List<String> list = new List<string>();
                                list.Add(needle);
                                list.Add(hammer);
                                action.Args.setValueChild(list);
                                methodListBox.Items.Add(action);
                            }
                        }
                    }
                    else if (line.Contains("Add Suffix action: "))
                    {
                        foreach (IStringAction i in stringActions)
                        {
                            if (line.Contains(i.name))
                            {
                                var action = i.Clone();
                                string token = line.Substring("Add Suffix action: Add Suffix  with ".Length);
                                List<String> list = new List<string>();
                                list.Add(token);
                                action.Args.setValueChild(list);
                                methodListBox.Items.Add(action);
                            }
                        }
                    }
                    else if (line.Contains("Add Prefix action: "))
                    {
                        foreach (IStringAction i in stringActions)
                        {
                            if (line.Contains(i.name))
                            {
                                var action = i.Clone();
                                string token = line.Substring("Add Prefix action: Add Prefix  with ".Length);
                                List<String> list = new List<string>();
                                list.Add(token);
                                action.Args.setValueChild(list);
                                methodListBox.Items.Add(action);
                            }
                        }
                    }
                    else if (line.Contains("Add counter action: "))
                    {
                        foreach (IStringAction i in stringActions)
                        {
                            if (line.Contains(i.name))
                            {
                                var action = i.Clone();
                                var matches = Regex.Matches(line, @"\d+");
                                List<String> list = new List<string>();
                                foreach (var match in matches)
                                {
                                    string strNum = String.Empty;
                                    strNum += match;
                                    list.Add(strNum);
                                }
                                action.Args.setValueChild(list);
                                methodListBox.Items.Add(action);
                            }
                        }
                    }
                }
            }
        }

        private void FileTab_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            // Check if the Data format of the file(s) can be accepted
            // (we only accept file drops from Windows Explorer, etc.)
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // modify the drag drop effects to Move
                e.Effects = System.Windows.DragDropEffects.Move;
            }
            else
            {
                // no need for any drag drop effect
                e.Effects = System.Windows.DragDropEffects.None;
            }
        }



        private void FileTab_Drop(object sender, System.Windows.DragEventArgs e)
        {

            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                foreach (var filePath in files)
                {
                    if (File.Exists(filePath))
                    {
                        bool check = true;
                        foreach (FileChoose fileChoose in FileTab.Items)
                        {
                            if (filePath == fileChoose.Path)
                            {
                                System.Windows.Forms.MessageBox.Show("File was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                check = false;
                                break;
                            }
                        }
                        if (check)
                        {
                            FileTab.Items.Add(new FileChoose()
                            {
                                Filename = System.IO.Path.GetFileName(filePath),
                                Path = filePath
                            });
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("You have to choose files", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void FolderTab_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            // Check if the Data format of the file(s) can be accepted
            // (we only accept file drops from Windows Explorer, etc.)
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // modify the drag drop effects to Move
                e.Effects = System.Windows.DragDropEffects.Move;
            }
            else
            {
                // no need for any drag drop effect
                e.Effects = System.Windows.DragDropEffects.None;
            }

        }

        private void FolderTab_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                foreach (var filePath in files)
                {
                    string directory = filePath;
                    if (Directory.Exists(directory))
                    {
                        // Add current directory to tab
                        bool check = true;
                        foreach (FolderChoose folderChoose in FolderTab.Items)
                        {
                            if (directory == folderChoose.Path)
                            {
                                System.Windows.Forms.MessageBox.Show("Folder was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                check = false;
                                break;
                            }
                        }
                        if (check)
                        {
                            FolderTab.Items.Add(new FolderChoose()
                            {
                                //Foldername = directory.Substring(directory.Length + 1),
                                Foldername = directory.Split('\\').Last(),
                                Path = directory
                            });
                        }

                        // Add all file in all subdirectories to file tab
                        string[] Files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
                        foreach (string fileName in Files)
                        {
                            bool fileCheck = true;
                            foreach (FileChoose fileChoose in FileTab.Items)
                            {
                                if (fileName == fileChoose.Path)
                                {
                                    System.Windows.Forms.MessageBox.Show("File was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                    fileCheck = false;
                                    break;
                                }
                            }
                            if (fileCheck)
                            {
                                FileTab.Items.Add(new FileChoose()
                                {
                                    Filename = System.IO.Path.GetFileName(fileName),
                                    Path = fileName
                                });
                            }
                        }


                        // Add all subdirectories to tab
                        string[] subs = Directory.GetDirectories(directory, "", SearchOption.AllDirectories);
                        foreach (string sub in subs)
                        {
                            bool folderCheck = true;
                            foreach (FolderChoose folder in FolderTab.Items)
                            {
                                if (sub == folder.Path)
                                {
                                    System.Windows.Forms.MessageBox.Show("Folder was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                    folderCheck = false;
                                    break;
                                }
                            }

                            if (folderCheck)
                            {
                                FolderTab.Items.Add(new FolderChoose()
                                {
                                    Foldername = sub.Split('\\').Last(),
                                    Path = sub
                                });
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("You have to choose folders", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                }



            }
        }

        private void FileInFolderTab_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            // Check if the Data format of the file(s) can be accepted
            // (we only accept file drops from Windows Explorer, etc.)
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // modify the drag drop effects to Move
                e.Effects = System.Windows.DragDropEffects.Move;
            }
            else
            {
                // no need for any drag drop effect
                e.Effects = System.Windows.DragDropEffects.None;
            }
        }

        private void FileInFolderTab_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                foreach (var filePath in files)
                {
                    string directory = filePath;
                    if (Directory.Exists(directory))
                    {
                        string[] fileslist = Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly);
                        foreach (string fileName in fileslist)
                        {
                            bool check = true;
                            foreach (FileChoose fileChoose in FileTab.Items)
                            {
                                if (fileName == fileChoose.Path)
                                {
                                    System.Windows.Forms.MessageBox.Show("File was selected", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                                    check = false;
                                    break;
                                }
                            }
                            if (check)
                            {
                                FileInFolderTab.Items.Add(new FileChoose()
                                {
                                    Filename = System.IO.Path.GetFileName(fileName),
                                    Path = fileName
                                });
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("You have to choose folders", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen

                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Maximized = false;
            }
            Properties.Settings.Default.Preset = presetSave;
            Properties.Settings.Default.Save();

        }

        private void RibbonWindow_SourceInitialized(object sender, EventArgs e)
        {
            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;
            this.Height = Properties.Settings.Default.Height;
            this.Width = Properties.Settings.Default.Width;
            // Very quick and dirty - but it does the job
            if (Properties.Settings.Default.Maximized)
            {
                WindowState = WindowState.Maximized;
            }


            if (Properties.Settings.Default.Preset != "")
            {
                using (StreamReader reader = new StreamReader(Properties.Settings.Default.Preset))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("Replace action:"))
                        {
                            foreach (IStringAction i in stringActions)
                            {
                                if (line.Contains(i.name))
                                {
                                    var action = i.Clone();
                                    string needle = line.Substring("Replace action: Replace ".Length, line.IndexOf(" with") - "Replace action: Replace ".Length);
                                    string hammer = line.Substring(line.IndexOf("with") + "with".Length + 1);
                                    List<String> list = new List<string>();
                                    list.Add(needle);
                                    list.Add(hammer);
                                    action.Args.setValueChild(list);
                                    methodListBox.Items.Add(action);
                                }
                            }
                        }
                        else if (line.Contains("Convert All Character:"))
                        {
                            foreach (IStringAction i in stringActions)
                            {
                                if (line.Contains(i.name))
                                {
                                    var action = i.Clone();
                                    string token = line.Substring("Convert All Character: Convert all character with ".Length);
                                    List<String> list = new List<string>();
                                    list.Add(token);
                                    action.Args.setValueChild(list);
                                    methodListBox.Items.Add(action);
                                }
                            }

                        }
                        else if (line.Contains("Change extention action: "))
                        {
                            foreach (IStringAction i in stringActions)
                            {
                                if (line.Contains(i.name))
                                {
                                    var action = i.Clone();
                                    string needle = line.Substring("Change extention action: Change Extension ".Length, line.IndexOf(" with") - "Change extention action: Change Extension ".Length);
                                    string hammer = line.Substring(line.IndexOf("with") + "with".Length + 1);
                                    List<String> list = new List<string>();
                                    list.Add(needle);
                                    list.Add(hammer);
                                    action.Args.setValueChild(list);
                                    methodListBox.Items.Add(action);
                                }
                            }
                        }
                        else if (line.Contains("Add Suffix action: "))
                        {
                            foreach (IStringAction i in stringActions)
                            {
                                if (line.Contains(i.name))
                                {
                                    var action = i.Clone();
                                    string token = line.Substring("Add Suffix action: Add Suffix  with ".Length);
                                    List<String> list = new List<string>();
                                    list.Add(token);
                                    action.Args.setValueChild(list);
                                    methodListBox.Items.Add(action);
                                }
                            }
                        }
                        else if (line.Contains("Add Prefix action: "))
                        {
                            foreach (IStringAction i in stringActions)
                            {
                                if (line.Contains(i.name))
                                {
                                    var action = i.Clone();
                                    string token = line.Substring("Add Prefix action: Add Prefix  with ".Length);
                                    List<String> list = new List<string>();
                                    list.Add(token);
                                    action.Args.setValueChild(list);
                                    methodListBox.Items.Add(action);
                                }
                            }
                        }
                        else if (line.Contains("Add counter action: "))
                        {
                            foreach (IStringAction i in stringActions)
                            {
                                if (line.Contains(i.name))
                                {
                                    var action = i.Clone();
                                    var matches = Regex.Matches(line, @"\d+");
                                    List<String> list = new List<string>();
                                    foreach (var match in matches)
                                    {
                                        string strNum = String.Empty;
                                        strNum += match;
                                        list.Add(strNum);
                                    }
                                    action.Args.setValueChild(list);
                                    methodListBox.Items.Add(action);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SaveXMLPreset_Click(object sender, RoutedEventArgs e)
        {
            if (methodListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing To Save To Preset", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                XmlWriter writer = XmlWriter.Create(@"xmlPreset.xml");
                writer.WriteStartElement("Rules");

                foreach (IStringAction action in methodListBox.Items)
                {
                    writer.WriteStartElement("Rule");
                    writer.WriteElementString("Name", $"{action.name}");
                    writer.WriteElementString("Details", $"{action.Args.Details}");
                    //string xmlActionData = @$"{action.name} {action.Args.Details}";
                    writer.WriteEndElement();
                }


                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                System.Windows.Forms.MessageBox.Show("All thing done ^_^", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        class XmlStringAction
        {
            public string Name { get; set; }
            public string Details { get; set; }
        }
        private void LoadXMLPreset_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.ShowDialog();
            string xmlpresetfilename = screen.FileName;
            if (xmlpresetfilename == null || xmlpresetfilename.Length == 0)
                return;

            List<IStringAction> listActions = new List<IStringAction>();
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpresetfilename);
            XmlNodeList nodeList = doc.SelectNodes("/Rules/Rule");
            List<XmlStringAction> xmlStringActions = new List<XmlStringAction>();

            foreach (XmlNode node in nodeList)
            {
                XmlStringAction xmlStringAction = new XmlStringAction();
                // loop over child nodes to get Name and all Number elements
                foreach (XmlNode child in node.ChildNodes)
                {
                    // check node name to decide how to handle the values               
                    if (child.Name == "Name")
                    {
                        xmlStringAction.Name = child.InnerText;
                    }
                    else if (child.Name == "Details")
                    {
                        xmlStringAction.Details = child.InnerText;
                    }

                }
                xmlStringActions.Add(xmlStringAction);
            }

            foreach (XmlStringAction line in xmlStringActions)
            {
                if (line.Name.Contains("Replace action:"))
                {
                    foreach (IStringAction i in stringActions)
                    {
                        if (line.Name.Contains(i.name))
                        {
                            var action = i.Clone();
                            string needle = line.Details.Substring("Replace ".Length, line.Details.IndexOf(" with") - "Replace ".Length);
                            string hammer = line.Details.Substring(line.Details.IndexOf("with") + "with".Length + 1);
                            List<string> list = new List<string>();
                            list.Add(needle);
                            list.Add(hammer);
                            action.Args.setValueChild(list);
                            methodListBox.Items.Add(action);
                        }
                    }
                }
                else if (line.Name.Contains("Convert All Character:"))
                {
                    foreach (IStringAction i in stringActions)
                    {
                        if (line.Name.Contains(i.name))
                        {
                            var action = i.Clone();
                            string token = line.Details.Substring("Convert all character with ".Length);
                            List<String> list = new List<string>();
                            list.Add(token);
                            action.Args.setValueChild(list);
                            methodListBox.Items.Add(action);
                        }
                    }

                }
                else if (line.Name.Contains("Change extention action:"))
                {
                    foreach (IStringAction i in stringActions)
                    {
                        if (line.Name.Contains(i.name))
                        {
                            var action = i.Clone();
                            string needle = line.Details.Substring("Change Extension ".Length, line.Details.IndexOf(" with") - "Change Extension ".Length);
                            string hammer = line.Details.Substring(line.Details.IndexOf("with") + "with".Length + 1);
                            List<String> list = new List<string>();
                            list.Add(needle);
                            list.Add(hammer);
                            action.Args.setValueChild(list);
                            methodListBox.Items.Add(action);
                        }
                    }
                }
                else if (line.Name.Contains("Add Suffix action:"))
                {
                    foreach (IStringAction i in stringActions)
                    {
                        if (line.Name.Contains(i.name))
                        {
                            var action = i.Clone();
                            string token = line.Details.Substring("Add Suffix  with ".Length);
                            List<String> list = new List<string>();
                            list.Add(token);
                            action.Args.setValueChild(list);
                            methodListBox.Items.Add(action);
                        }
                    }
                }
                else if (line.Name.Contains("Add Prefix action:"))
                {
                    foreach (IStringAction i in stringActions)
                    {
                        if (line.Name.Contains(i.name))
                        {
                            var action = i.Clone();
                            string token = line.Details.Substring("Add Prefix  with ".Length);
                            List<String> list = new List<string>();
                            list.Add(token);
                            action.Args.setValueChild(list);
                            methodListBox.Items.Add(action);
                        }
                    }
                }
                else if (line.Name.Contains("Add counter action:"))
                {
                    foreach (IStringAction i in stringActions)
                    {
                        if (line.Name.Contains(i.name))
                        {
                            var action = i.Clone();
                            var matches = Regex.Matches(line.Details, @"\d+");
                            List<String> list = new List<string>();
                            foreach (var match in matches)
                            {
                                string strNum = String.Empty;
                                strNum += match;
                                list.Add(strNum);
                            }
                            action.Args.setValueChild(list);
                            methodListBox.Items.Add(action);
                        }
                    }
                }

            }
        }

        private void PreviewFileButtons_Click(object sender, RoutedEventArgs e)
        {
            if (methodListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Add Method Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (FileTab.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Choose File Or Folder Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                int i = 0;
                //file process
                foreach (FileChoose file in FileTab.Items)
                {
                    string result = file.Filename;

                    foreach (IStringAction action in methodListBox.Items)
                    {
                        result = action.Processor.Invoke(result, i);
                    }
                    file.PreviewFile = result;

                    ++i;
                }

                FileTab.Items.Refresh();
            }

        }

        private void PreviewFolderButtons_Click(object sender, RoutedEventArgs e)
        {
            if (methodListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Add Method Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (FolderTab.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Choose Folder Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                int i = 0;
                foreach (FolderChoose folder in FolderTab.Items)
                {
                    string result = folder.Foldername;

                    foreach (IStringAction action in methodListBox.Items)
                    {
                        result = action.Processor.Invoke(result, i);
                    }
                    i++;
                    folder.PreviewFolder = result;
                }


                FolderTab.Items.Refresh();
            }
        }

        private void PreviewListFileButtons_Click(object sender, RoutedEventArgs e)
        {
            if (methodListBox.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Add Method Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else if (FileInFolderTab.Items.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Choose Folder Before Batching!", "Error Detected in Input", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            else
            {
                int i = 0;
                foreach (FileChoose file in FileInFolderTab.Items)
                {
                    string result = file.Filename;

                    foreach (IStringAction action in methodListBox.Items)
                    {
                        result = action.Processor.Invoke(result, i);
                    }
                    file.PreviewFile = result;

                    i++;
                }


                FileInFolderTab.Items.Refresh();
            }
        }
    }
}
