namespace _07_NoSpaceLeft
{
  internal class Parser
  {
    internal static bool IsCommand(string line)
    {
      return line.StartsWith("$ ");
    }

    internal static Command ParseCommand(string line)
    {
      if (line.StartsWith("$ ls"))
        return new CommandLs();

      const string cdToken = "$ cd ";
      if (line.StartsWith(cdToken))
        return new CommandCd(line[cdToken.Length..]);

      throw new ApplicationException($"Invalid input: {line}");
    }

    internal static FileSystemItem ParseFileSystemItem(string line)
    {
      const string directoryToken = "dir ";
      if (line.StartsWith(directoryToken))
        return new Directory(line[directoryToken.Length..]);

      var fileParts = line.Split(' ', 2);
      return new File(fileParts[1], ulong.Parse(fileParts[0]));
    }
  }

  internal record Command;

  internal record CommandCd(string Folder) : Command;

  internal record CommandLs() : Command;

  internal record FileSystemItem(string Name);

  internal record Directory(string Name) : FileSystemItem(Name);

  internal record File(string Name, ulong Size) : FileSystemItem(Name);
}
