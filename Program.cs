using System;
using System.IO;

/// <summary>
/// This program removes empty folders from a specified starting directory.
/// Usage: Program <startingDirectory> [-whatif]
/// - <startingDirectory>: The directory to start the search for empty folders.
/// - [-whatif]: Optional flag to indicate a dry run, where no folders are actually deleted.
/// </summary>
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

        if (Directory.Exists(startingDirectory))
        {
            RemoveEmptyFolders(startingDirectory, whatIf);
        }
        else
        {
            Console.WriteLine($"The directory '{startingDirectory}' does not exist.");
        }
    }

    static void ShowUsage()
    {
        Console.WriteLine("Usage: Program <startingDirectory> [-whatif]");
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