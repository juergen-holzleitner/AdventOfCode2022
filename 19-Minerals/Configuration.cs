using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace _19_Minerals
{
  internal class Configuration : IComparable<Configuration>, IEquatable<Configuration>
  {
    public int TimeLeft { get; private set; } = 24;
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
      if (TimeLeft <= 0)
        yield break;

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
          if (roboter.Value.ContainsKey(mineral) && roboter.Value[mineral] > maxCost)
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
      
      if (TimeLeft < 1)
        throw new ApplicationException("TimeLeft should be greater than 0 here");

      --TimeLeft;
    }

    private Configuration Clone()
    {
      var config = new Configuration(blueprint)
      {
        TimeLeft = TimeLeft
      };
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
        configurations = newConfigurations;
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

      if (TimeLeft != other.TimeLeft)
        return TimeLeft.CompareTo(other.TimeLeft);

      foreach (var mineral in Enum.GetValues<Mineral>())
      {
        if (Stock[mineral] != other.Stock[mineral])
          return Stock[mineral].CompareTo(other.Stock[mineral]);
      }

      foreach(var mineral in Enum.GetValues<Mineral>())
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


  }

  internal class ConfigurationComperer : IEqualityComparer<Configuration>
  {
    public bool Equals(Configuration? x, Configuration? y)
    {
      return x!.Equals(y);
    }

    public int GetHashCode([DisallowNull] Configuration obj)
    {
      var val = obj.TimeLeft;
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
