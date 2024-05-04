using ProjectRenamer.Models;
using System.IO;

namespace ProjectRenamer.Services
{
    public class RephraseService
    {
        private string oldWord = string.Empty;
        private string newWord = string.Empty;
        private string[] typesToRead = ["cs", "csproj", "sln"];
        
        public void Rephrase(RephraseSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Path) || string.IsNullOrWhiteSpace(settings.OldWord) || string.IsNullOrWhiteSpace(settings.NewWord))
            {
                throw new ArgumentException("Invalid input data.");
            }

            if (!Directory.Exists(settings.Path))
            {
                throw new ArgumentException("Invalid path.");
            }

            oldWord = settings.OldWord;
            newWord = settings.NewWord;
            if (settings.TypesToOpen != null && settings.TypesToOpen.Length > 0)
            {
                typesToRead = settings.TypesToOpen;
            }
            GetDirectories(settings.Path);
        }
            
        private void GetDirectories(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (var filePath in files)
            {
                var fileName = Path.GetFileName(filePath);
                try
                {
                    if (typesToRead.Any(x => fileName.Contains(x)))
                    {
                        string fileContent = File.ReadAllText(filePath);
                        fileContent = fileContent.Replace(oldWord, newWord);
                        File.WriteAllText(filePath, fileContent);
                    }
                    if (fileName.Contains(oldWord))
                    {
                        var newName = fileName.Replace(oldWord, newWord);
                        File.Move(filePath, Path.Combine(path, Path.Combine(path, newName)));
                    }
                }
                catch 
                {
                    throw new InvalidOperationException($"Error while processing file {fileName} in {path}.");
                }
            }

            var directories = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly)
                .Where(dir => (File.GetAttributes(dir) & FileAttributes.Hidden) == 0)
                .ToArray();
            foreach (var directory in directories)
            {
                GetDirectories(Path.Combine(path, directory));
            }

            var dirName = Path.GetFileName(path);
            if (dirName!.Contains(oldWord))
            {
                var newDirName = dirName.Replace(oldWord, newWord);
                var parentPath = Path.GetDirectoryName(path)!;
                var newPath = Path.Combine(parentPath, newDirName);
                Directory.Move(path, newPath);
            }
        }
    }
}
