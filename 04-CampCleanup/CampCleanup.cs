namespace _04_CampCleanup
{
  internal static class CampCleanup
  {
    internal record struct Assignment(int From, int To)
    {
      internal bool IsOverlapping(Assignment second)
      {
        return !(second.From > To || second.To < From);
      }
    }

    internal static Assignment ParseAssignment(string input)
    {
      var parts = input.Split('-');
      return new Assignment(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    internal static (Assignment First, Assignment Second) ParseAssignmentPair(string input)
    {
      var parts = input.Split(",");
      return new(ParseAssignment(parts[0]), ParseAssignment(parts[1]));
    }

    internal static bool Contains(this Assignment assignment, Assignment other)
    {
      return other.From >= assignment.From && other.To <= assignment.To;
    }

    internal static bool AssignmentIsContained(string input)
    {
      var assignementPair = ParseAssignmentPair(input);
      return assignementPair.First.Contains(assignementPair.Second) || assignementPair.Second.Contains(assignementPair.First);
    }

    internal static int CountFullyOverlapping(IEnumerable<string> assignmentPairs)
    {
      return assignmentPairs.Count(a => AssignmentIsContained(a));
    }

    internal static bool AssignmentIsOverlapping(string input)
    {
      var assignmentPair = ParseAssignmentPair(input);
      return assignmentPair.First.IsOverlapping(assignmentPair.Second);
    }

    internal static int CountOverlapping(IEnumerable<string> assignmentPairs)
    {
      return assignmentPairs.Count(a => AssignmentIsOverlapping(a));
    }
  }
}
