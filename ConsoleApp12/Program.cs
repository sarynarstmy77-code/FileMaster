using System;
using System.IO;
using System.Linq;

class Program
{
    static readonly string[] Images =
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"
    };

    static readonly string[] Documents =
    {
        ".pdf", ".doc", ".docx", ".txt",
        ".xls", ".xlsx", ".ppt", ".pptx"
    };

    static readonly string[] Videos =
    {
        ".mp4", ".mkv", ".avi", ".mov", ".wmv"
    };

    static readonly string[] Music =
    {
        ".mp3", ".wav", ".flac", ".aac", ".ogg"
    };

    static readonly string[] Archives =
    {
        ".zip", ".rar", ".7z", ".tar", ".gz"
    };

    static void Main()
    {
        Console.Title = "Smart File Organizer";

        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("=================================");
            Console.WriteLine("       SMART FILE ORGANIZER      ");
            Console.WriteLine("=================================");

            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("1. Organize a folder");
            Console.WriteLine("2. Scan a folder");
            Console.WriteLine("3. Exit");

            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    OrganizeFolder();
                    break;

                case "2":
                    ScanFolder();
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("\nInvalid option.");
                    Pause();
                    break;
            }
        }
    }

    static string GetFolder()
    {
        Console.Write("\nEnter folder path: ");

        string path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Path cannot be empty.");
            Pause();
            return null;
        }

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Folder does not exist.");
            Pause();
            return null;
        }

        return path;
    }

    static void ScanFolder()
    {
        string folder = GetFolder();

        if (folder == null)
            return;

        string[] files = Directory.GetFiles(folder);

        int images = 0;
        int documents = 0;
        int videos = 0;
        int music = 0;
        int archives = 0;
        int other = 0;

        foreach (string file in files)
        {
            string extension =
                Path.GetExtension(file).ToLowerInvariant();

            if (Images.Contains(extension))
                images++;

            else if (Documents.Contains(extension))
                documents++;

            else if (Videos.Contains(extension))
                videos++;

            else if (Music.Contains(extension))
                music++;

            else if (Archives.Contains(extension))
                archives++;

            else
                other++;
        }

        Console.WriteLine("\n========== SCAN RESULT ==========");

        Console.WriteLine("Images     : " + images);
        Console.WriteLine("Documents  : " + documents);
        Console.WriteLine("Videos     : " + videos);
        Console.WriteLine("Music      : " + music);
        Console.WriteLine("Archives   : " + archives);
        Console.WriteLine("Other      : " + other);

        Console.WriteLine("\nTotal files: " + files.Length);

        Pause();
    }

    static void OrganizeFolder()
    {
        string folder = GetFolder();

        if (folder == null)
            return;

        string[] files = Directory.GetFiles(folder);

        if (files.Length == 0)
        {
            Console.WriteLine("\nNo files found.");
            Pause();
            return;
        }

        Console.WriteLine("\nFound " + files.Length + " files.");

        Console.Write(
            "Are you sure you want to organize them? (y/n): ");

        string answer = Console.ReadLine();

        if (answer == null || answer.ToLower() != "y")
        {
            Console.WriteLine("Operation cancelled.");
            Pause();
            return;
        }

        int movedFiles = 0;

        foreach (string file in files)
        {
            try
            {
                string extension =
                    Path.GetExtension(file).ToLowerInvariant();

                string folderName = GetCategory(extension);

                string destinationFolder =
                    Path.Combine(folder, folderName);

                Directory.CreateDirectory(destinationFolder);

                string fileName =
                    Path.GetFileName(file);

                string destination =
                    Path.Combine(destinationFolder, fileName);

                // جلوگیری از جایگزین شدن فایل موجود
                destination =
                    GetUniqueFileName(destination);

                File.Move(file, destination);

                movedFiles++;

                Console.ForegroundColor =
                    ConsoleColor.Green;

                Console.WriteLine(
                    "Moved: " + fileName +
                    " -> " + folderName);

                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor =
                    ConsoleColor.Red;

                Console.WriteLine(
                    "Error: " + Path.GetFileName(file));

                Console.WriteLine(ex.Message);

                Console.ResetColor();
            }
        }

        Console.WriteLine();

        Console.ForegroundColor =
            ConsoleColor.Green;

        Console.WriteLine(
            "Done! " + movedFiles +
            " files organized.");

        Console.ResetColor();

        Pause();
    }

    static string GetCategory(string extension)
    {
        if (Images.Contains(extension))
            return "Images";

        if (Documents.Contains(extension))
            return "Documents";

        if (Videos.Contains(extension))
            return "Videos";

        if (Music.Contains(extension))
            return "Music";

        if (Archives.Contains(extension))
            return "Archives";

        return "Other";
    }

    static string GetUniqueFileName(string path)
    {
        if (!File.Exists(path))
            return path;

        string directory =
            Path.GetDirectoryName(path);

        string fileName =
            Path.GetFileNameWithoutExtension(path);

        string extension =
            Path.GetExtension(path);

        int counter = 1;

        while (true)
        {
            string newPath = Path.Combine(
                directory,
                fileName + " (" + counter + ")" + extension
            );

            if (!File.Exists(newPath))
                return newPath;

            counter++;
        }
    }

    static void Pause()
    {
        Console.WriteLine(
            "\nPress Enter to continue...");

        Console.ReadLine();
    }
}
