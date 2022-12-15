using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace _15_BeaconExclusionZone
{
  internal record struct Pos(long X, long Y);

  internal record struct Slice(Pos Start, Pos End);

  internal record struct Sensor(Pos Position, Pos BeaconPosition)
  {
    internal long GetArea()
    {
      var r = GetRadius();
      return r * r + (r + 1) * (r + 1);
    }

    internal Slice? GetHorizontalSlice(long posY)
    {
      var r = GetRadius();
      var diff = Math.Abs(Position.Y - posY);
      if (diff > r)
        return null;

      var offset = r - diff;
      return new Slice(new Pos(Position.X - offset, posY), new Pos(Position.X + offset, posY));
    }

    internal long GetRadius()
    {
      return Math.Abs(Position.X - BeaconPosition.X) + Math.Abs(Position.Y - BeaconPosition.Y);
    }
  }

  internal partial class Zone
  {
    internal static List<Pos> GetFreePositionWithin(string lines, long maxCoordinate)
    {
      var positions = new List<Pos>();
      var sensors = ParseInput(lines);

      for (long y = 0; y <= maxCoordinate; ++y)
      {
        long x = 0;
        while (x <= maxCoordinate)
        {
          bool found = true;

          foreach (var s in sensors)
          {
            var slice = s.GetHorizontalSlice(y);
            if (slice.HasValue && slice.Value.Start.X <= x && slice.Value.End.X >= x)
            {
              x = slice.Value.End.X + 1;
              found = false;
              break;
            }
          }

          if (found)
          {
            positions.Add(new Pos(x, y));
            ++x;
          }
        }
      }

      return positions;
    }

    internal static long GetNotPositions(string lines, long verticalPosition)
    {
      var sensors = ParseInput(lines);
      var positions = new HashSet<Pos>();

      foreach (var sensor in sensors)
      {
        var slice = sensor.GetHorizontalSlice(verticalPosition);
        if (!slice.HasValue) continue;

        for (long n = slice.Value.Start.X; n <= slice.Value.End.X; ++n)
          positions.Add(new Pos(n, verticalPosition));
      }

      foreach (var sensor in sensors)
      {
        positions.Remove(sensor.Position);
        positions.Remove(sensor.BeaconPosition);
      }

      return positions.Count;
    }

    internal static long GetTuningFrequency(string lines, long maxCoordinate)
    {
      var freePositions = GetFreePositionWithin(lines, maxCoordinate);
      var pos = freePositions.Single();
      return pos.X * 4000000 + pos.Y;
    }

    internal static IEnumerable<Sensor> ParseInput(string lines)
    {
      foreach (var line in lines.Split('\n'))
      {
        if (!string.IsNullOrWhiteSpace(line))
        {
          var sensor = ParseLine(line);
          yield return sensor;
        }
      }
    }

    internal static Sensor ParseLine(string line)
    {
      var regex = RegExSensor();
      var match = regex.Match(line);

      if (match.Success)
      {
        var sensorX = long.Parse(match.Groups["SensorX"].Value);
        var sensorY = long.Parse(match.Groups["SensorY"].Value);
        var beaconX = long.Parse(match.Groups["BeaconX"].Value);
        var beaconY = long.Parse(match.Groups["BeaconY"].Value);

        return new Sensor(new Pos(sensorX, sensorY), new Pos(beaconX, beaconY));
      }

      throw new ApplicationException("not expected input: " + line);
    }

    [GeneratedRegex("Sensor at x=(?<SensorX>-?\\d+), y=(?<SensorY>-?\\d+): closest beacon is at x=(?<BeaconX>-?\\d+), y=(?<BeaconY>-?\\d+)")]
    private static partial Regex RegExSensor();
  }
}