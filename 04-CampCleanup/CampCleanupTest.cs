using FluentAssertions;

namespace _04_CampCleanup
{
  public class CampCleanupTest
  {
    [Fact]
    public void Can_parse_assignment()
    {
      var input = "2-4";
      var result = CampCleanup.ParseAssignment(input);
      result.Should().Be(new CampCleanup.Assignment(2, 4));
    }

    [Fact]
    public void Can_parse_assignment_pair()
    {
      var input = "2-4,6-8";
      var result = CampCleanup.ParseAssignmentPair(input);

      result.First.Should().Be(new CampCleanup.Assignment(2, 4));
      result.Second.Should().Be(new CampCleanup.Assignment(6, 8));
    }

    [Fact]
    public void Can_check_if_one_assignment_is_within_another()
    {
      var assignment1 = new CampCleanup.Assignment(2, 8);
      var assignment2 = new CampCleanup.Assignment(3, 7);
      
      var res = assignment1.Contains(assignment2);

      res.Should().BeTrue();
    }

    [Theory]
    [InlineData("2-8,3-7", true)]
    [InlineData("6-6,4-6", true)]
    [InlineData("2-4,6-8", false)]
    public void Can_check_if_assignment_pair_contains(string input, bool expectedResult)
    {
      var res = CampCleanup.AssignmentIsContained(input);

      res.Should().Be(expectedResult);
    }

    [Fact]
    public void Can_get_count_fully_overlapping()
    {
      var input = "2-4,6-8\r\n2-3,4-5\r\n5-7,7-9\r\n2-8,3-7\r\n6-6,4-6\r\n2-6,4-8";
      var numFullyOverlapping = CampCleanup.CountFullyOverlapping(input.Split('\n'));
      numFullyOverlapping.Should().Be(2);
    }
  }
}