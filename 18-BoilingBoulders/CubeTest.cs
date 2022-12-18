using FluentAssertions;
using System.Security.Cryptography;

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

    [Fact]
    public void Can_get_bounding_box()
    {
      var input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5\r\n";
      var cubes = Cube.ParseInput(input);

      var boundingBox = Cube.GetBoundingBox(cubes);

      boundingBox.Should().Be(new BoundingBox(new Cube(1, 1, 1), new Cube(3, 3, 6)));
    }

    [Fact]
    public void Can_get_block_with_empty_cubes()
    {
      var input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5\r\n";
      var cubes = Cube.ParseInput(input);

      var blockWithCubeStats = Cube.GenerateBlockWithCubeStats(cubes);

      var numFreeItems = (from CubeStats x in blockWithCubeStats.Block where x.Type == Type.Free select x).Count();
      var expectedNumFreeItems = blockWithCubeStats.Block.GetLength(0) * blockWithCubeStats.Block.GetLength(1) * blockWithCubeStats.Block.GetLength(2);
      expectedNumFreeItems -= cubes.Count;
      numFreeItems.Should().Be(expectedNumFreeItems);
    }

    [Fact]
    public void Can_mark_each_block_as_inner_or_outer_free()
    {
      var input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5\r\n";
      var cubes = Cube.ParseInput(input);
      
      var innerSurface = Cube.GetInnerSurface(cubes);

      innerSurface.Should().Be(6);
    }

    [Fact]
    public void Can_get_total_surface_part_2()
    {
      var input = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5\r\n";

      var totalSurface = Cube.GetTotalSurfacePart2(input);

      totalSurface.Should().Be(58);
    }
  }
}