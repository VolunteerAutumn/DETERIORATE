using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Explorer
    {
        string DefPath { get; set; }
        // constructor opens the defaultpath and opens it in console, showing things in such format:
        // folders: 📁 [foldername]
        // files: 📝 [filename] - only possible to open text-based files
        // 🔙 back
        public Explorer()
        {
            DefPath = Directory.GetCurrentDirectory();

            string[] directories = Directory.GetDirectories(DefPath);
            string[] files = Directory.GetFiles(DefPath);

            foreach (var dir in directories)
            {
                Console.WriteLine($"folders: 📁 {Path.GetFileName(dir)}");
            }

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".txt" || ext == ".md" || ext == ".cs" || ext == ".json")
                {
                    Console.WriteLine($"files: 📝 {Path.GetFileName(file)}");
                }
            }

            Console.WriteLine("🔙 back");
        }


        // now, we need a function that'd get what the person's input and open such folder or file, previously cleaning the console.
        public void ChoiceNext()
        {
            Console.WriteLine("Enter the directory name or the file name you want to open >>> ");
            string input = Console.ReadLine();
            Console.Clear();

            if (input == "back" || input == "🔙")
            {
                string parent = Directory.GetParent(DefPath)?.FullName;
                if (parent != null)
                {
                    DefPath = parent;
                }
                else
                {
                    Console.WriteLine("Already at the root directory.");
                }
            }
            else
            {
                string dirPath = Path.Combine(DefPath, input);
                string filePath = Path.Combine(DefPath, input);

                if (Directory.Exists(dirPath))
                {
                    DefPath = dirPath;
                }
                else if (File.Exists(filePath))
                {
                    string ext = Path.GetExtension(filePath).ToLower();
                    if (ext == ".txt" || ext == ".md" || ext == ".cs" || ext == ".json")
                    {
                        Console.WriteLine(File.ReadAllText(filePath));
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Cannot open this file type.");
                    }
                }
                else
                {
                    Console.WriteLine("Directory or file not found.");
                }
            }

            // Redisplay contents after action
            string[] directories = Directory.GetDirectories(DefPath);
            string[] files = Directory.GetFiles(DefPath);

            foreach (var dir in directories)
            {
                Console.WriteLine($"folders: 📁 {Path.GetFileName(dir)}");
            }

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".txt" || ext == ".md" || ext == ".cs" || ext == ".json")
                {
                    Console.WriteLine($"files: 📝 {Path.GetFileName(file)}");
                }
            }

            Console.WriteLine("🔙 back");
        }



        // finally we need functions that allow us to delete, rename and add files/folders.
        public void DeleteItem(string itemname)
        {
            // Determine if it's a file or folder
            string targetPath = Path.Combine(DefPath, itemname);

            if (string.IsNullOrWhiteSpace(Path.GetExtension(itemname)))
            {
                // No extension: treat as folder
                if (Directory.Exists(targetPath))
                {
                    try
                    {
                        Directory.Delete(targetPath, true); // true = recursive delete
                        Console.WriteLine($"Folder '{itemname}' deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting folder: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Folder not found.");
                }
            }
            else
            {
                // Has extension: treat as file
                if (File.Exists(targetPath))
                {
                    try
                    {
                        File.Delete(targetPath);
                        Console.WriteLine($"File '{itemname}' deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
        }



        public void AddItem(string itemname)
        {
            // Pseudocode:
            // 1. Check if itemname has an extension.
            // 2. If no extension, treat as folder: create directory if it doesn't exist.
            // 3. If has extension, treat as file: create file if it doesn't exist.
            // 4. For files, optionally prompt for initial content.
            // 5. Handle exceptions and print appropriate messages.

            string targetPath = Path.Combine(DefPath, itemname);

            if (string.IsNullOrWhiteSpace(Path.GetExtension(itemname)))
            {
                // No extension: treat as folder
                if (!Directory.Exists(targetPath))
                {
                    try
                    {
                        Directory.CreateDirectory(targetPath);
                        Console.WriteLine($"Folder '{itemname}' created.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating folder: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Folder already exists.");
                }
            }
            else
            {
                // Has extension: treat as file
                if (!File.Exists(targetPath))
                {
                    try
                    {
                        Console.WriteLine("Enter initial content for the file (leave empty for blank file):");
                        string content = Console.ReadLine();
                        File.WriteAllText(targetPath, content ?? "");
                        Console.WriteLine($"File '{itemname}' created.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating file: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("File already exists.");
                }
            }
        }



        public void RenItem(string item, string newname)
        {
            string sourcePath = Path.Combine(DefPath, item);
            string destPath = Path.Combine(DefPath, newname);

            if (string.IsNullOrWhiteSpace(Path.GetExtension(item)))
            {
                // Folder
                if (Directory.Exists(sourcePath))
                {
                    if (!Directory.Exists(destPath) && !File.Exists(destPath))
                    {
                        try
                        {
                            Directory.Move(sourcePath, destPath);
                            Console.WriteLine($"Folder '{item}' renamed to '{newname}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error renaming folder: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("A file or folder with the new name already exists.");
                    }
                }
                else
                {
                    Console.WriteLine("Folder not found.");
                }
            }
            else
            {
                // File
                if (File.Exists(sourcePath))
                {
                    if (!File.Exists(destPath) && !Directory.Exists(destPath))
                    {
                        try
                        {
                            File.Move(sourcePath, destPath);
                            Console.WriteLine($"File '{item}' renamed to '{newname}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error renaming file: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("A file or folder with the new name already exists.");
                    }
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
        }



        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Explorer Menu ---");
                Console.WriteLine("1. Navigate (open folder/file)");
                Console.WriteLine("2. Add file/folder");
                Console.WriteLine("3. Delete file/folder");
                Console.WriteLine("4. Rename file/folder");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ChoiceNext();
                        break;
                    case "2":
                        Console.Write("Enter new file/folder name: ");
                        string addName = Console.ReadLine();
                        AddItem(addName);
                        break;
                    case "3":
                        Console.Write("Enter file/folder name to delete: ");
                        string delName = Console.ReadLine();
                        DeleteItem(delName);
                        break;
                    case "4":
                        Console.Write("Enter current file/folder name: ");
                        string oldName = Console.ReadLine();
                        Console.Write("Enter new name: ");
                        string newName = Console.ReadLine();
                        RenItem(oldName, newName);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
    class Program
    {
        static void Main()
        {
            // Ensure the console can display emojis
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Tip: For best emoji support, use a font like 'Segoe UI Emoji' in your console settings.");

            Explorer exp = new Explorer();
            exp.Run();
        }
    }
}



// easily one of the hardest assignments ngl TwT
