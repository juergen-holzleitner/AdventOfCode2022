using System.Diagnostics.CodeAnalysis;

namespace _19_Minerals
{
  internal class Configuration : IComparable<Configuration>, IEquatable<Configuration>
  {
    public Dictionary<Mineral, int> Stock { get; private set; } = new();
    public Dictionary<Mineral, int> Roboter { get; private set; } = new();

    private readonly Blueprint blueprint;

    public Configuration(Blueprint blueprint)
    {
      this.blueprint = blueprint;

      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        Stock.Add(mineral, 0);
        Roboter.Add(mineral, mineral == Mineral.Ore ? 1 : 0);
      }
    }

    internal IEnumerable<Configuration> GetNextConfigurations(Dictionary<Mineral, int> maxUsefulRoboters)
    {
      if (CanProduceMineralRobot(Mineral.Geode))
      {
        var newConfigWithRoboter = Clone();
        newConfigWithRoboter.ProduceMineral();
        newConfigWithRoboter.ProduceMineralRobot(Mineral.Geode);
        yield return newConfigWithRoboter;
        yield break;
      }

      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        if (CanProduceMineralRobot(mineral) && ItMakesSenseToProduceMineralRobot(maxUsefulRoboters, mineral))
        {
          var newConfigWithRoboter = Clone();
          newConfigWithRoboter.ProduceMineral();
          newConfigWithRoboter.ProduceMineralRobot(mineral);
          yield return newConfigWithRoboter;
        }
      }

      var newConfigWithoutRoboter = Clone();
      newConfigWithoutRoboter.ProduceMineral();
      yield return newConfigWithoutRoboter;
    }

    private bool ItMakesSenseToProduceMineralRobot(Dictionary<Mineral, int> maxUsefulRoboters, Mineral mineral)
    {
      if (mineral == Mineral.Geode)
        return true;

      return Roboter[mineral] < maxUsefulRoboters[mineral];
    }

    internal Dictionary<Mineral, int> GetMaxUsefulRoboterPerMineral()
    {
      var dictionary = new Dictionary<Mineral, int>();
      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        int maxCost = 0;
        foreach (var roboter in blueprint.RoboterCosts)
        {
          if (roboter.Value.TryGetValue(mineral, out int value) && value > maxCost)
            maxCost = roboter.Value[mineral];
        }
        dictionary.Add(mineral, maxCost);
      }

      return dictionary;
    }

    private void ProduceMineralRobot(Mineral mineral)
    {
      foreach (var requiredMineral in blueprint.RoboterCosts[mineral])
      {
        if (Stock[requiredMineral.Key] < requiredMineral.Value)
          throw new ApplicationException("minerals are expected to be available");

        Stock[requiredMineral.Key] -= requiredMineral.Value;
      }

      ++Roboter[mineral];
    }

    private bool CanProduceMineralRobot(Mineral mineral)
    {
      foreach (var requiredMineral in blueprint.RoboterCosts[mineral])
      {
        if (Stock[requiredMineral.Key] < requiredMineral.Value)
          return false;
      }
      return true;
    }

    private void ProduceMineral()
    {
      foreach (var mineral in Enum.GetValues<Mineral>())
        Stock[mineral] += Roboter[mineral];
    }

    private Configuration Clone()
    {
      var config = new Configuration(blueprint);
      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        config.Roboter[mineral] = Roboter[mineral];
        config.Stock[mineral] = Stock[mineral];
      }
      return config;
    }

    internal IEnumerable<Configuration> GetNextConfigurationsAfter(int minutes)
    {
      var configurations = new List<Configuration>
      {
        this
      };

      var maxUsefulRoboters = GetMaxUsefulRoboterPerMineral();

      var totalConfigurations = new HashSet<Configuration>(new ConfigurationComperer());

      for (int n = 0; n < minutes; ++n)
      {
        var newConfigurations = new List<Configuration>();

        int numDuplicatesRemoved = 0;
        foreach (var config in configurations)
        {
          foreach (var c in config.GetNextConfigurations(maxUsefulRoboters))
          {
            if (totalConfigurations.Contains(c))
            {
              ++numDuplicatesRemoved;
            }
            else
            {
              newConfigurations.Add(c);
              totalConfigurations.Add(c);
            }
          }
        }

        var maxGeods = newConfigurations.Select(x => x.Stock[Mineral.Geode]).Max();
        var maxGeodRoboter = newConfigurations.Select(x => x.Roboter[Mineral.Geode]).Max();

        configurations = newConfigurations.Where(x => x.Roboter[Mineral.Geode] >= maxGeodRoboter || x.Stock[Mineral.Geode] >= maxGeods - 1).ToList();
      }

      return configurations;
    }

    internal int GetMaxGeodesAfter(int minutes)
    {
      return GetNextConfigurationsAfter(minutes).Select(c => c.Stock[Mineral.Geode]).Max();
    }

    public int CompareTo(Configuration? other)
    {
      if (other == null)
        return 1;

      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        if (Stock[mineral] != other.Stock[mineral])
          return Stock[mineral].CompareTo(other.Stock[mineral]);
      }

      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        if (Roboter[mineral] != other.Roboter[mineral])
          return Roboter[mineral].CompareTo(other.Roboter[mineral]);
      }

      return 0;
    }

    public bool Equals(Configuration? other)
    {
      return CompareTo(other) == 0;
    }

    public override bool Equals(object? obj)
    {
      return Equals(obj as Configuration);
    }

    public override int GetHashCode()
    {
      return new ConfigurationComperer().GetHashCode(this);
    }
  }

  internal class ConfigurationComperer : IEqualityComparer<Configuration>
  {
    public bool Equals(Configuration? x, Configuration? y)
    {
      return x!.Equals(y);
    }

    public int GetHashCode([DisallowNull] Configuration obj)
    {
      var val = 0;
      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        val <<= 4;
        val ^= obj.Stock[mineral];
        val <<= 4;
        val ^= obj.Roboter[mineral];
      }
      return val;
    }
  }
}
