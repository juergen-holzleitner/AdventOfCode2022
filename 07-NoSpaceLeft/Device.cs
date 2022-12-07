namespace _07_NoSpaceLeft
{
  internal record DirectoryEntry(string Name, List<FileSystemItem> ChildItems) : Directory(Name);

  internal class Device
  {
    private readonly DirectoryEntry root = new("/", new List<FileSystemItem>());
    private List<string> currentPath = new();

    internal void AddFolder(string folder)
    {
      var currentDirectoryEntry = GetCurrentDirectoryEntry();
      currentDirectoryEntry.ChildItems.Add(new DirectoryEntry(folder, new()));
    }

    private DirectoryEntry GetCurrentDirectoryEntry()
    {
      var current = root;
      foreach (var item in currentPath)
      {
        current = current!.ChildItems.Where(c => c is DirectoryEntry && c.Name == item).Single() as DirectoryEntry;
      }
      return current;
    }

    internal void ChangeDirectory(string directory)
    {
      if (directory == "/")
      {
        currentPath = new List<string>();
        return;
      }

      if (directory == "..")
      {
        currentPath.RemoveAt(currentPath.Count - 1);
        return;
      }

      if (!GetCurrentDirectoryEntry().ChildItems.Any(f => f is Directory && f.Name == directory))
        throw new ArgumentException($"Directory '{directory}' not available");

      currentPath.Add(directory);
    }

    internal IEnumerable<string> GetCurrentDirectory()
    {
      return currentPath;
    }

    internal IEnumerable<FileSystemItem> GetFileSystemItems()
    {
      return GetCurrentDirectoryEntry().ChildItems;
    }

    internal void ProcessLine(string line)
    {
      if (string.IsNullOrWhiteSpace(line))
        return;

      if (Parser.IsCommand(line))
      {
        var command = Parser.ParseCommand(line);
        if (command is CommandCd cd)
        {
          ChangeDirectory(cd.Folder);
        }
      }
      else
      {
        var fileSystemItem = Parser.ParseFileSystemItem(line);
        if (fileSystemItem is Directory)
          GetCurrentDirectoryEntry().ChildItems.Add(new DirectoryEntry(fileSystemItem.Name, new()));
        else
          GetCurrentDirectoryEntry().ChildItems.Add(fileSystemItem);
      }
    }

    internal ulong GetCurrentFolderSize()
    {
      return GetFolderSize(GetCurrentDirectoryEntry());
    }

    private ulong GetFolderSize(DirectoryEntry currentFolder)
    {
      ulong totalSize = 0;
      foreach (var f in currentFolder.ChildItems)
      {
        if (f is File file)
        {
          totalSize += file.Size;
        }
        else if (f is DirectoryEntry d)
        {
          totalSize += GetFolderSize(d);
        }
      }
      return totalSize;
    }

    internal ulong GetSumOfFoldersAtMost(ulong size)
    {
      return GetSumOfFoldersAtMost(root, size);
    }

    private ulong GetSumOfFoldersAtMost(DirectoryEntry current, ulong size)
    {
      ulong totalSize = 0;

      var currentSize = GetFolderSize(current);
      if (currentSize <= size)
        totalSize += currentSize;

      foreach (var x in current.ChildItems)
        if (x is DirectoryEntry d)
          totalSize += GetSumOfFoldersAtMost(d, size);

      return totalSize;
    }

    internal ulong GetRootFolderSize()
    {
      return GetFolderSize(root);
    }

    internal ulong GetSmallestDirectoryWithAtLeast(ulong sizeRequired)
    {
      return GetSmallestDirectoryWithAtLeast(sizeRequired, null, root)!.Value;
    }

    private ulong? GetSmallestDirectoryWithAtLeast(ulong sizeRequired, ulong? currentBest, DirectoryEntry current)
    {
      var size = GetFolderSize(current);
      if (size >= sizeRequired)
      {
        if (!currentBest.HasValue || currentBest.Value > size)
          currentBest = size;

        foreach (var x in current.ChildItems)
          if (x is DirectoryEntry d)
            currentBest = GetSmallestDirectoryWithAtLeast(sizeRequired, currentBest, d);
      }

      return currentBest;
    }
  }
}
