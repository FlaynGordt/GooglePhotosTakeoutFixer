namespace GooglePhotosTakeoutFixer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Run: <sourceDirectory> <destionationDirectory> flags");
                Console.WriteLine("Flags:");
                Console.WriteLine("silent: no verbose output");
                return;
            }

            var sourceDir = args[0];
            var targetDir = args[1];

            GlobalSettings.Silent = args.Any(a => a == "silent");

            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine(sourceDir + " not found or is not a directory");
                return;
            }

            if (!Directory.Exists(sourceDir))
            {
                Console.WriteLine(targetDir + " not found or is not a directory");
                return;
            }


            var files = Directory.EnumerateFiles(sourceDir).Select(f => new FileInfo(f));
            var targetDiretory = new DirectoryInfo(targetDir);

            string[] invalidExtensions = { ".json" };

            foreach (var file in files)
            {
                if (!invalidExtensions.Contains(file.Extension.ToLower()))
                {
                    FileStuff.RenameAndCopy(file, targetDiretory, FileStuff.DestinationDirectoryMode.SubDirectories, FileStuff.DestinationFilenameMode.Rename);
                }
            }
        }
    }
}
