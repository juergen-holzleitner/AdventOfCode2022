using System.Text.RegularExpressions;

namespace _19_Minerals
{
  enum Mineral { Ore, Clay, Obsidian, Geode };

  internal record Blueprint(int Id, Dictionary<Mineral, Dictionary<Mineral, int>> RoboterCosts);

  internal class Factory
  {
    internal static int GetQualityLevel(string input)
    {
      var sum = 0;
      var blueprints = ParseAllBlueprints(input);
      foreach (var blueprint in blueprints)
      {
        var configuration = new Configuration(blueprint);
        var numGeodes = configuration.GetMaxGeodesAfter(24);
        sum += numGeodes * blueprint.Id;
      }

      return sum;
    }

    internal static IEnumerable<Blueprint> ParseAllBlueprints(string inputs)
    {
      foreach (var input in inputs.Split('\n'))
        if (!string.IsNullOrWhiteSpace(input))
          yield return ParseBlueprint(input);
    }

    internal static Blueprint ParseBlueprint(string input)
    {
      var regex = new Regex(@"Blueprint (?<Id>\d+): Each ore robot costs (?<Ore_Ore>\d+) ore. Each clay robot costs (?<Clay_Ore>\d+) ore. Each obsidian robot costs (?<Obsidian_Ore>\d+) ore and (?<Obsidian_Clay>\d+) clay. Each geode robot costs (?<Geode_Ore>\d+) ore and (?<Geode_Obsidian>\d+) obsidian.");
      var match = regex.Match(input);
      if (!match.Success)
        throw new ApplicationException("not expected");

      var id = int.Parse(match.Groups["Id"].Value);
      var ore_ore = int.Parse(match.Groups["Ore_Ore"].Value);
      var clay_ore = int.Parse(match.Groups["Clay_Ore"].Value);
      var obsidian_ore = int.Parse(match.Groups["Obsidian_Ore"].Value);
      var obsidian_clay = int.Parse(match.Groups["Obsidian_Clay"].Value);
      var geode_ore = int.Parse(match.Groups["Geode_Ore"].Value);
      var geode_obsidian = int.Parse(match.Groups["Geode_Obsidian"].Value);

      var roboterCost = new Dictionary<Mineral, Dictionary<Mineral, int>>()
      {
        {Mineral.Ore, new Dictionary<Mineral, int>() {{ Mineral.Ore, ore_ore } } },
        {Mineral.Clay, new Dictionary<Mineral, int>() {{ Mineral.Ore, clay_ore } } },
        {Mineral.Obsidian, new Dictionary<Mineral, int>() {{ Mineral.Ore, obsidian_ore }, {Mineral.Clay, obsidian_clay } } },
        {Mineral.Geode, new Dictionary<Mineral, int>() {{ Mineral.Ore, geode_ore }, {Mineral.Obsidian, geode_obsidian} } },
      };

      return new Blueprint(id, roboterCost);
    }
  }
}