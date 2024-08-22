using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "/?" || args[0] == "-?")
        {
            ShowUsage();
            return;
        }

        bool whatIf = args.Length > 1 && args[1] == "-whatif";
        string startingDirectory = args[0];

        if (Directory.Exists(startingDirectory) && (File.GetAttributes(startingDirectory) & FileAttributes.Directory) == FileAttributes.Directory)
        {
            RemoveEmptyFolders(startingDirectory, whatIf);
        }
        else
        {
            Console.WriteLine($"The path '{startingDirectory}' is not a valid directory.");
        }
    }

    static void ShowUsage()
    {
        Console.WriteLine("Usage: DelEmptyFolders.exe <startingDirectory> [-whatif]");
        Console.WriteLine("");
        Console.WriteLine("Examples:");
        Console.WriteLine(@"  DelEmptyFolders.exe C:\path\to\starting\directory");
        Console.WriteLine(@"  DelEmptyFolders.exe C:\path\to\starting\directory -whatif");
        Console.WriteLine("");
        Console.WriteLine("Help:");
        Console.WriteLine(@"  The program will remove all empty folders in the starting directory and all subdirectories.");
        Console.WriteLine(@"  DelEmptyFolders.exe /?");
        Console.WriteLine(@"  DelEmptyFolders.exe -?");
        Console.WriteLine("");
        Console.WriteLine("Author: Jan Tiedemann");
    }

    static void RemoveEmptyFolders(string directory, bool whatIf)
    {
        // Get all subdirectories
        string[] subdirectories = Directory.GetDirectories(directory);

        // Recursively remove empty folders in subdirectories
        foreach (string subdirectory in subdirectories)
        {
            RemoveEmptyFolders(subdirectory, whatIf);
        }

        // Check if the current directory is empty
        if (Directory.GetFiles(directory).Length == 0 && Directory.GetDirectories(directory).Length == 0)
        {
            if (whatIf)
            {
                Console.WriteLine($"[WhatIf] Would delete empty folder: {directory}");
            }
            else
            {
                try
                {
                    Directory.Delete(directory);
                    Console.WriteLine($"Deleted empty folder: {directory}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete folder: {directory}. Error: {ex.Message}");
                }
            }
        }
    }
}