using FluentAssertions;
using FluentAssertions.Common;

namespace _19_Minerals
{
  public class MineralsTest
  {
    [Fact]
    public void Can_parse_blueprint()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";

      var blueprint = Factory.ParseBlueprint(input);

      blueprint.Id.Should().Be(1);
      blueprint.RoboterCosts[Mineral.Ore][Mineral.Ore].Should().Be(4);
      blueprint.RoboterCosts[Mineral.Clay][Mineral.Ore].Should().Be(2);
      blueprint.RoboterCosts[Mineral.Obsidian][Mineral.Ore].Should().Be(3);
      blueprint.RoboterCosts[Mineral.Obsidian][Mineral.Clay].Should().Be(14);
      blueprint.RoboterCosts[Mineral.Geode][Mineral.Ore].Should().Be(2);
      blueprint.RoboterCosts[Mineral.Geode][Mineral.Obsidian].Should().Be(7);
    }

    [Fact]
    public void Can_parse_all_blueprints()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.\r\nBlueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.\r\n";
      var blueprints = Factory.ParseAllBlueprints(input);

      blueprints.Should().HaveCount(2);
    }

    [Fact]
    public void Can_create_initial_configuration()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
      var blueprint = Factory.ParseBlueprint(input);

      var configuration = new Configuration(blueprint);

      configuration.TimeLeft.Should().Be(24);
      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        configuration.Stock[mineral].Should().Be(0);
      }

      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        configuration.Roboter[mineral].Should().Be(mineral == Mineral.Ore ? 1: 0);
      }
    }

    [Fact]
    public void Can_get_state_of_minute_1()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
      var blueprint = Factory.ParseBlueprint(input);
      var configuration = new Configuration(blueprint);
      var maxUsefulRoboters = configuration.GetMaxUsefulRoboterPerMineral();

      var newConfigurations = configuration.GetNextConfigurations(maxUsefulRoboters);

      newConfigurations.Should().HaveCount(1);
      newConfigurations.Single().TimeLeft.Should().Be(23);
      newConfigurations.Single().Stock[Mineral.Ore].Should().Be(1);
    }

    [Fact]
    public void Can_get_state_of_minute_2()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
      var blueprint = Factory.ParseBlueprint(input);
      var configuration = new Configuration(blueprint);

      var newConfigurations = configuration.GetNextConfigurationsAfter(2);

      newConfigurations.Should().HaveCount(1);
      newConfigurations.Single().TimeLeft.Should().Be(22);
      newConfigurations.Single().Stock[Mineral.Ore].Should().Be(2);
    }

    [Fact]
    public void Can_get_state_of_minute_3()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
      var blueprint = Factory.ParseBlueprint(input);
      var configuration = new Configuration(blueprint);

      var newConfigurations = configuration.GetNextConfigurationsAfter(3);

      newConfigurations.Should().HaveCount(2);
      newConfigurations.Should().OnlyContain(c => c.TimeLeft == 21);
      newConfigurations.Should().Contain(c => c.Roboter[Mineral.Clay] == 1 && c.Roboter[Mineral.Ore] == 1 && c.Stock[Mineral.Ore] == 1);
    }

    [Fact]
    public void Can_calc_max_useful_roboter_per_mineral()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
      var blueprint = Factory.ParseBlueprint(input);
      var configuration = new Configuration(blueprint);

      var maxUsefulRoboterPerMineral = configuration.GetMaxUsefulRoboterPerMineral();
      maxUsefulRoboterPerMineral[Mineral.Ore].Should().Be(4);
      maxUsefulRoboterPerMineral[Mineral.Clay].Should().Be(14);
      maxUsefulRoboterPerMineral[Mineral.Obsidian].Should().Be(7);
      maxUsefulRoboterPerMineral[Mineral.Geode].Should().Be(0);
    }

    [Fact]
    public void Can_get_state_of_first_blueprint()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.";
      var blueprint = Factory.ParseBlueprint(input);
      var configuration = new Configuration(blueprint);

      var numGeodes = configuration.GetMaxGeodesAfter(24);

      numGeodes.Should().Be(9);
    }

    [Fact]
    public void Can_get_quality_level()
    {
      var input = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.\r\nBlueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.\r\n";
      var qualityLevel = Factory.GetQualityLevel(input);
      qualityLevel.Should().Be(33);
    }
  }
}