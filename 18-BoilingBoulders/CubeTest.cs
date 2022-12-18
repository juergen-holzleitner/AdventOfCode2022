using FluentAssertions;

namespace _18_BoilingBoulders
{
  public class CubeTest
  {
    [Fact]
    public void Can_parse_single_cube()
    {
      var line = "2,2,2";
      var cube = Cube.ParseLine(line);

      cube.Should().Be(new Cube(2, 2, 2));
    }

    [Fact]
    public void Can_parse_all_input()
    {
      var input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5\r\n";

      var cubes = Cube.ParseInput(input);

      cubes.Should().HaveCount(13);
    }

    [Fact]
    public void Can_get_adjacent_positions()
    {
      var cube = new Cube(1, 2, 3);
      var adjacent = cube.GetAdjacentPositions();
      adjacent.Should().BeEquivalentTo(new[] { new Cube(0, 2, 3), new Cube(2, 2, 3), new Cube(1, 1, 3), new Cube(1, 3, 3), new Cube(1, 2, 2), new Cube(1, 2, 4) });
    }

    [Fact]
    public void Can_get_additional_surfaces()
    {
      var processed = new List<Cube>() { new Cube(1, 0, 0) };

      var cube = new Cube(0, 0, 0);

      var additionalSurfaces = cube.GetAdditionalSurfaces(processed);

      additionalSurfaces.Should().Be(4);
    }


    [Fact]
    public void Can_get_total_surface()
    {
      var input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5\r\n";

      var totalSurface = Cube.GetTotalSurface(input);

      totalSurface.Should().Be(64);
    }


  }
}