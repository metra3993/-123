using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;

        while (true)
        {
            DisplayDriveMenu();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.Clear();

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                break;
            }
            else if (char.IsLetter(keyInfo.KeyChar))
            {
                string driveLetter = keyInfo.KeyChar.ToString().ToUpper();
                ExploreDrive(driveLetter);
            }
        }
    }

    static void DisplayDriveMenu()
    {
        Console.WriteLine("Выберите диск:");

        DriveInfo[] drives = DriveInfo.GetDrives();
        int selectedIndex = 0;

        while (true)
        {
            for (int i = 0; i < drives.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"[{drives[i].Name}] {drives[i].DriveType} - Свободно: {drives[i].AvailableFreeSpace / (1024 * 1024 * 1024)} GB / Всего: {drives[i].TotalSize / (1024 * 1024 * 1024)} GB");

                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex == 0) ? drives.Length - 1 : selectedIndex - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex == drives.Length - 1) ? 0 : selectedIndex + 1;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                string driveLetter = drives[selectedIndex].Name.Substring(0, 1);
                ExploreDrive(driveLetter);
                break;
            }

            Console.Clear();
        }
    }

    static void ExploreDrive(string driveLetter)
    {
        Console.Clear();
        Console.WriteLine($"Информация о диске {driveLetter}:");

        DriveInfo drive = new DriveInfo(driveLetter);
        Console.WriteLine($"Тип: {drive.DriveType}");
        Console.WriteLine($"Файловая система: {drive.DriveFormat}");
        Console.WriteLine($"Свободно: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} GB / Всего: {drive.TotalSize / (1024 * 1024 * 1024)} GB\n");

        Console.WriteLine("Файлы и папки:");

        string[] directories = Directory.GetDirectories(driveLetter);
        string[] files = Directory.GetFiles(driveLetter);

        foreach (string directory in directories)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            Console.WriteLine($"[Папка] {directoryInfo.Name} - Создана: {directoryInfo.CreationTime}");
        }

        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);
            Console.WriteLine($"[Файл] {fileInfo.Name} - Создан: {fileInfo.CreationTime} - Формат: {fileInfo.Extension}");
        }

        Console.WriteLine("\nНажмите Escape для возврата к выбору диска.");
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Escape)
        {
            Console.Clear();
        }
        else if (Directory.Exists(Path.Combine(driveLetter, keyInfo.KeyChar.ToString())))
        {
            ExploreFolder(Path.Combine(driveLetter, keyInfo.KeyChar.ToString()));
        }
        else if (File.Exists(Path.Combine(driveLetter, keyInfo.KeyChar.ToString())))
        {
            RunFile(Path.Combine(driveLetter, keyInfo.KeyChar.ToString()));
        }
    }

    static void ExploreFolder(string path)
    {
        Console.Clear();
        Console.WriteLine($"Информация о папке {path}:");

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        Console.WriteLine($"Создана: {directoryInfo.CreationTime}\n");

        Console.WriteLine("Файлы и папки:");

        string[] directories = Directory.GetDirectories(path);
        string[] files = Directory.GetFiles(path);

        foreach (string directory in directories)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            Console.WriteLine($"[Папка] {dirInfo.Name} - Создана: {dirInfo.CreationTime}");
        }

        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);
            Console.WriteLine($"[Файл] {fileInfo.Name} - Создан: {fileInfo.CreationTime} - Формат: {fileInfo.Extension}");
        }

        Console.WriteLine("\nНажмите Escape для возврата к предыдущей папке.");
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Escape)
        {
            Console.Clear();
            ExploreDrive(directoryInfo.Root.FullName);
        }
        else if (Directory.Exists(Path.Combine(path, keyInfo.KeyChar.ToString())))
        {
            ExploreFolder(Path.Combine(path, keyInfo.KeyChar.ToString()));
        }
        else if (File.Exists(Path.Combine(path, keyInfo.KeyChar.ToString())))
        {
            RunFile(Path.Combine(path, keyInfo.KeyChar.ToString()));
        }
    }

    static void RunFile(string filePath)
    {
        Console.Clear();
        Console.WriteLine($"Запуск файла: {filePath}");

        Console.WriteLine("\nНажмите любую клавишу для возврата.");
        Console.ReadKey(true);
        Console.Clear();
    }
}
