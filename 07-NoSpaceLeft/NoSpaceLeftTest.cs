using FluentAssertions;

namespace _07_NoSpaceLeft
{
  public class NoSpaceLeftTest
  {
    [Theory]
    [InlineData("$ cd /", "/")]
    [InlineData("$ cd a", "a")]
    public void Can_parse_cd(string line, string expectedArgument)
    {
      var command = Parser.ParseCommand(line);

      command.Should().Be(new CommandCd(expectedArgument));
    }

    [Fact]
    public void Can_parse_ls()
    {
      var line = "$ ls";

      var command = Parser.ParseCommand(line);

      command.Should().Be(new CommandLs());
    }

    [Theory]
    [InlineData("dir a", "a")]
    [InlineData("dir d", "d")]
    public void Can_parse_dir(string line, string expectedDirectoryName)
    {
      var item = Parser.ParseFileSystemItem(line);

      item.Should().Be(new Directory(expectedDirectoryName));
    }

    [Fact]
    public void Can_change_one_directory_up()
    {
      var sut = new Device();
      sut.AddFolder("a");
      sut.ChangeDirectory("a");
      sut.AddFolder("b");
      sut.ChangeDirectory("b");

      sut.ChangeDirectory("..");

      var currentDirectory = sut.GetCurrentDirectory();
      currentDirectory.Should().BeEquivalentTo(new[] { "a" });
    }

    [Theory]
    [InlineData("14848514 b.txt", "b.txt", 14848514)]
    [InlineData("8504156 c.dat", "c.dat", 8504156)]
    public void Can_parse_file(string line, string expectedFileName, ulong expectedSize)
    {
      var item = Parser.ParseFileSystemItem(line);

      item.Should().Be(new File(expectedFileName, expectedSize));
    }

    [Theory]
    [InlineData("$ ", true)]
    [InlineData(" ", false)]
    public void Can_check_if_line_is_command(string line, bool expectedResult)
    {
      var isCommand = Parser.IsCommand(line);
      isCommand.Should().Be(expectedResult);
    }

    [Fact]
    public void Deivce_has_initial_directory()
    {
      var sut = new Device();

      var currentDir = sut.GetCurrentDirectory();
      var items = sut.GetFileSystemItems();

      currentDir.Should().BeEmpty();
      items.Should().BeEmpty();
    }

    [Fact]
    public void Can_add_folder_to_root()
    {
      var sut = new Device();

      sut.AddFolder("a");

      var items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new[] { new Directory("a") });
    }

    [Fact]
    public void Can_change_to_root()
    {
      var sut = new Device();

      sut.ChangeDirectory("/");

      var currentDir = sut.GetCurrentDirectory();
      currentDir.Should().BeEmpty();
    }

    [Fact]
    public void Can_process_sample_with_ls()
    {
      var input = "$ ls\r\ndir a\r\n14848514 b.txt\r\n8504156 c.dat\r\ndir d\r\n";
      var sut = new Device();

      foreach (var line in input.Split('\n'))
      {
        sut.ProcessLine(line.Trim('\r'));
      }

      var items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new FileSystemItem[] { new Directory("a"), new File("b.txt", 14848514), new File("c.dat", 8504156), new Directory("d") });
    }

    [Fact]
    public void Throw_if_directory_not_available()
    {
      var sut = new Device();

      var action = () => sut.ChangeDirectory("a");

      action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Can_change_into_directory()
    {
      var sut = new Device();
      sut.AddFolder("a");

      sut.ChangeDirectory("a");

      var current = sut.GetCurrentDirectory();
      current.Should().BeEquivalentTo(new[] { "a" });
    }

    [Fact]
    public void Can_add_item_to_directory()
    {
      var sut = new Device();
      sut.AddFolder("a");

      sut.ChangeDirectory("a");
      sut.AddFolder("b");

      var items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new FileSystemItem[] { new Directory("b") });
    }

    [Fact]
    public void Can_process_sample()
    {
      var input = "$ cd /\r\n$ ls\r\ndir a\r\n14848514 b.txt\r\n8504156 c.dat\r\ndir d\r\n$ cd a\r\n$ ls\r\ndir e\r\n29116 f\r\n2557 g\r\n62596 h.lst\r\n$ cd e\r\n$ ls\r\n584 i\r\n$ cd ..\r\n$ cd ..\r\n$ cd d\r\n$ ls\r\n4060174 j\r\n8033020 d.log\r\n5626152 d.ext\r\n7214296 k";
      var sut = new Device();

      foreach (var line in input.Split('\n'))
      {
        sut.ProcessLine(line.Trim('\r'));
      }

      sut.ChangeDirectory("/");
      var items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new FileSystemItem[] { new Directory("a"), new File("b.txt", 14848514), new File("c.dat", 8504156), new Directory("d") });

      sut.ChangeDirectory("a");
      items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new FileSystemItem[] { new Directory("e"), new File("f", 29116), new File("g", 2557), new File("h.lst", 62596) });

      sut.ChangeDirectory("e");
      items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new FileSystemItem[] { new File("i", 584) });

      sut.ChangeDirectory("/");
      sut.ChangeDirectory("d");
      items = sut.GetFileSystemItems();
      items.Should().BeEquivalentTo(new FileSystemItem[] { new File("j", 4060174), new File("d.log", 8033020), new File("d.ext", 5626152), new File("k", 7214296) });
    }

    [Fact]
    public void Can_get_size_of_directory()
    {
      var sut = new Device();
      sut.AddFolder("a");
      sut.ProcessLine("123 x.file");
      sut.ChangeDirectory("a");
      sut.ProcessLine("123 x.file");
      sut.ChangeDirectory("/");

      var size = sut.GetCurrentFolderSize();

      size.Should().Be(246);
    }

    [Fact]
    public void Can_get_sum_of_folders_for_part1()
    {
      var input = "$ cd /\r\n$ ls\r\ndir a\r\n14848514 b.txt\r\n8504156 c.dat\r\ndir d\r\n$ cd a\r\n$ ls\r\ndir e\r\n29116 f\r\n2557 g\r\n62596 h.lst\r\n$ cd e\r\n$ ls\r\n584 i\r\n$ cd ..\r\n$ cd ..\r\n$ cd d\r\n$ ls\r\n4060174 j\r\n8033020 d.log\r\n5626152 d.ext\r\n7214296 k";
      var sut = new Device();

      foreach (var line in input.Split('\n'))
      {
        sut.ProcessLine(line.Trim('\r'));
      }

      var size = sut.GetSumOfFoldersAtMost(100000);
      size.Should().Be(95437);
    }
  }
}