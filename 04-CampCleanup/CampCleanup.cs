using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_CampCleanup
{
  internal static class CampCleanup
  {
    internal record struct Assignment(int From, int To);

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
  }
}
