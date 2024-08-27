# DelEmptyFolders

`DelEmptyFolders` is a simple .NET 8 console application written in C# that recursively deletes empty folders starting from a specified directory. It also provides a \"what-if\" mode to simulate the deletion process without actually deleting any folders.

## Usage
```sh
DelEmptyFolders.exe <startingDirectory> [-logFilePath C:\\Temp\\output.log] [-whatif]
```
## Examples:
```sh
DelEmptyFolders.exe C:\path\to\starting\directory

DelEmptyFolders.exe C:\path\to\starting\directory -LogFilePath C:\path\to\output.log

DelEmptyFolders.exe C:\path\to\starting\directory -whatif
```
