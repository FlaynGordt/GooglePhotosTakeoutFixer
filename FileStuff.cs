using Newtonsoft.Json.Linq;

namespace GooglePhotosTakeoutFixer
{
    public class FileStuff
    {
        /// <summary>
        /// Defines where the file are store in the destination
        /// </summary>
        public enum DestinationDirectoryMode
        {
            // directly in the destination directory, will rename all files
            SingleDirectory,

            // in subdirectories in the destination directory
            // subdirectories are <destination>/<year>/<month>
            SubDirectories,
        }

        public enum DestinationFilenameMode
        {
            Rename,
            DontRename,
        }



        public enum CopyAndRenameMetaFile
        {
            Yes,
            No
        }

        public enum WriteMetaInfoOptions
        {
            Yes,
            No
        }

        public static FileInfo RenameAndCopy(FileInfo sourceFile,
                                             DirectoryInfo destinationRootDirectory,
                                             DestinationDirectoryMode destinationMode,
                                             DestinationFilenameMode destinationFilenameMode)
        {


            if (sourceFile.Extension.ToLower() == ".json")
            {
                throw new NotAnImageException(sourceFile.FullName);
            }

            var jsonFile = sourceFile.FullName + ".json";

            if (!File.Exists(jsonFile))
            {
                throw new NoMetaFileFoundException(sourceFile.FullName);
            }

            DirectoryInfo targetDirectory = destinationRootDirectory;

            var jObjectRoot = JObject.Parse(File.ReadAllText(jsonFile));
            
            if (jObjectRoot.GetValue("creationTime") is not JObject creationTime)
            {
                throw new MetaDataNotFoundException(sourceFile.FullName, "creationTime");
            }
           
            var value = creationTime.GetValue("timestamp") ?? throw new MetaDataNotFoundException(sourceFile.FullName, "timestamp");

            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(value.ToString())).DateTime;

            if (destinationMode == DestinationDirectoryMode.SubDirectories)
            {
                targetDirectory = new DirectoryInfo(Path.Combine(destinationRootDirectory.FullName, dateTime.Year.ToString(), dateTime.Month.ToString("00")));
                targetDirectory.Create();
            }

            string newFileName = Path.Combine(targetDirectory.FullName, sourceFile.Name);

            if (destinationFilenameMode == DestinationFilenameMode.Rename)
            {
                newFileName = Path.Combine(targetDirectory.FullName, $"{dateTime.Year}_{dateTime.Month.ToString("00")}_{dateTime.Day.ToString("00")}_{dateTime.ToString("HHmmss")}_{sourceFile.Name}");
            }

            if (!File.Exists(newFileName))
            {
                File.Copy(sourceFile.FullName, newFileName);
                Console.WriteLine(sourceFile.Name + " -> " + newFileName.Remove(0, destinationRootDirectory.FullName.Length + 1));
            }

            return new FileInfo(newFileName);
        }
    }
}
