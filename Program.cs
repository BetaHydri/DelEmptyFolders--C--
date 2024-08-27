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
        string? logFilePath = args.Length > 2 ? args[2] : null;

        if (Directory.Exists(startingDirectory) && (File.GetAttributes(startingDirectory) & FileAttributes.Directory) == FileAttributes.Directory)
        {
            RemoveEmptyFolders(directory: startingDirectory, whatIf: whatIf, logFilePath: logFilePath ?? string.Empty);
        }
        else
        {
            string message = $"The path '{startingDirectory}' is not a valid directory.";
            Console.WriteLine(message);
            if (!string.IsNullOrEmpty(logFilePath))
            {
                LogMessage(logFilePath: logFilePath, message: message);
            }
        }
    }

    static void LogMessage(string logFilePath, string message)
    {
        if (!string.IsNullOrEmpty(logFilePath))
        {
            try
            {
                File.AppendAllText(logFilePath, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
            }
        }
    }

    static void ShowUsage()
    {
        Console.WriteLine("Usage: DelEmptyFolders.exe <startingDirectory> [-logFilePath C:\\Temp\\output.log] [-whatif]");
        Console.WriteLine("");
        Console.WriteLine("Examples:");
        Console.WriteLine("");
        Console.WriteLine(@"  Delete empty folders:");
        Console.WriteLine(@"  DelEmptyFolders.exe C:\path\to\starting\directory");
        Console.WriteLine("");
        Console.WriteLine(@"  Delete empty folders and log to file:");
        Console.WriteLine(@"  DelEmptyFolders.exe C:\path\to\starting\directory -LogFilePath C:\path\to\output.log");
        Console.WriteLine("");
        Console.WriteLine(@"  Simulate deletion:");
        Console.WriteLine(@"  DelEmptyFolders.exe C:\path\to\starting\directory -whatif");
        Console.WriteLine("");
        Console.WriteLine("Help:");
        Console.WriteLine(@"  The program will remove all empty folders in the starting directory and all subdirectories.");
        Console.WriteLine(@"  DelEmptyFolders.exe /?");
        Console.WriteLine(@"  DelEmptyFolders.exe -?");
        Console.WriteLine("");
        Console.WriteLine("Author: Jan Tiedemann");
    }

    static void RemoveEmptyFolders(string directory, bool whatIf, string logFilePath)
    {
        try
        {
            // Get all subdirectories
            string[] subdirectories = Directory.GetDirectories(directory);

            // Recursively remove empty folders in subdirectories
            foreach (string subdirectory in subdirectories)
            {
                RemoveEmptyFolders(directory: subdirectory, whatIf: whatIf, logFilePath: logFilePath);
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
                        LogMessage(logFilePath: logFilePath, message: $"Deleted empty folder: {directory}");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Access denied to folder: {directory}. Skipping deletion.");
                        Console.ResetColor();
                        LogMessage(logFilePath: logFilePath, message: $"Access denied to folder: {directory}. Skipping deletion.");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed to delete folder: {directory}. Error: {ex.Message}");
                        Console.ResetColor();
                        LogMessage(logFilePath: logFilePath, message: $"Failed to delete folder: {directory}. Error: {ex.Message}");
                    }
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Access denied to folder: {directory}. Skipping.");
            Console.ResetColor();
            LogMessage(logFilePath: logFilePath, message: $"Access denied to folder: {directory}. Skipping.");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to process folder: {directory}. Error: {ex.Message}");
            Console.ResetColor();
            LogMessage(logFilePath: logFilePath, message: $"Failed to process folder: {directory}. Error: {ex.Message}");
        }
    }
}