using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfAppHoliday.Logic
{
    class ClassTextFiles
    {
        private string path;
        private DirectoryInfo d;
        private bool isCreate;

        public string PathFile { private set; get; }

        public ClassTextFiles()
        {
            path = changePath(2,Directory.GetCurrentDirectory());
        }

        public string changePath(int number, string path)
        {
            for(int i=0, index = 0; i < number; i++)
            {
                index = path.LastIndexOf("\\");
                if (index == -1)
                    break;
                path = path.Substring(0, index);
            }
            return path;
        }

        public Task CreateFileTask(string text, string name)
        {
            Action action = new Action(
                () =>
                {
                    CreateFile(text, name);
                });
            Task task = new Task(action);
            task.Start();
            return task;
        }

        private void DeleteDirectory(string pathDir)
        {
            string[] files = Directory.GetFiles(pathDir);
            string[] dirs = Directory.GetDirectories(pathDir);
            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
            Directory.Delete(pathDir, false);
        }

        private void createDirectory()
        {
            string directory = path + "\\Files\\" +DateTime.Now.ToString("dd.MM.yyyy");
            try
            {
                if (Directory.Exists(directory))
                    DeleteDirectory(directory);
                d = Directory.CreateDirectory(directory);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateFile(string text, string name)
        {
            try
            {
                if (!isCreate)
                {
                    createDirectory();
                    isCreate = true;
                }
                int numer = d.GetFiles().Length;
                PathFile = d.FullName + "\\" + name + (++numer) + ".txt";
                if (File.Exists(path))
                    File.Delete(PathFile);
                using (StreamWriter sw = File.CreateText(PathFile))
                {
                    sw.WriteLine(text);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
