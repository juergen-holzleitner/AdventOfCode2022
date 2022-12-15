using FluentAssertions;

namespace _15_BeaconExclusionZone
{
  public class BeaconTest
  {
    [Fact]
    public void Can_parse_sensor()
    {
      var line = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15";

      var sensor = Zone.ParseLine(line);

      sensor.Position.Should().Be(new Pos(2, 18));
      sensor.BeaconPosition.Should().Be(new Pos(-2, 15));
    }

    [Fact]
    public void Can_parse_all_sensors()
    {
      var lines = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15\r\nSensor at x=9, y=16: closest beacon is at x=10, y=16\r\nSensor at x=13, y=2: closest beacon is at x=15, y=3\r\nSensor at x=12, y=14: closest beacon is at x=10, y=16\r\nSensor at x=10, y=20: closest beacon is at x=10, y=16\r\nSensor at x=14, y=17: closest beacon is at x=10, y=16\r\nSensor at x=8, y=7: closest beacon is at x=2, y=10\r\nSensor at x=2, y=0: closest beacon is at x=2, y=10\r\nSensor at x=0, y=11: closest beacon is at x=2, y=10\r\nSensor at x=20, y=14: closest beacon is at x=25, y=17\r\nSensor at x=17, y=20: closest beacon is at x=21, y=22\r\nSensor at x=16, y=7: closest beacon is at x=15, y=3\r\nSensor at x=14, y=3: closest beacon is at x=15, y=3\r\nSensor at x=20, y=1: closest beacon is at x=15, y=3\r\n";

      var sensors = Zone.ParseInput(lines);

      sensors.Should().HaveCount(14);
    }

    [Fact]
    public void Can_get_radious_of_sensor()
    {
      var line = "Sensor at x=8, y=7: closest beacon is at x=2, y=10";
      var sensor = Zone.ParseLine(line);

      var radius = sensor.GetRadius();

      radius.Should().Be(9);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 5)]
    [InlineData(9, 181)]
    public void Can_get_area_of_Sensor(int radius, int expectedArea)
    {
      var sensor = new Sensor(new Pos(0, 0), new Pos(radius, 0));

      var area = sensor.GetArea();

      area.Should().Be(expectedArea);
    }

    [Fact]
    public void Can_get_horizontal_slice_near_sensor()
    {
      var sensor = new Sensor(new Pos(0, 0), new Pos(0, 0));

      var slice = sensor.GetHorizontalSlice(0);

      slice.Should().Be(new Slice(new Pos(0, 0), new Pos(0, 0)));
    }

    [Fact]
    public void Can_get_horizontal_slice_outside_of_sensor()
    {
      var sensor = new Sensor(new Pos(0, 0), new Pos(0, 0));

      var slice = sensor.GetHorizontalSlice(1);

      slice.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Can_get_horizontal_slice_with_some_size()
    {
      var sensor = new Sensor(new Pos(8, 7), new Pos(2, 10));

      var slice = sensor.GetHorizontalSlice(10);

      slice.HasValue.Should().BeTrue();
      slice.Should().Be(new Slice(new Pos(2, 10), new Pos(14, 10)));
    }

    [Fact]
    public void Can_get_horizontal_slices_from_sample()
    {
      var lines = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15\r\nSensor at x=9, y=16: closest beacon is at x=10, y=16\r\nSensor at x=13, y=2: closest beacon is at x=15, y=3\r\nSensor at x=12, y=14: closest beacon is at x=10, y=16\r\nSensor at x=10, y=20: closest beacon is at x=10, y=16\r\nSensor at x=14, y=17: closest beacon is at x=10, y=16\r\nSensor at x=8, y=7: closest beacon is at x=2, y=10\r\nSensor at x=2, y=0: closest beacon is at x=2, y=10\r\nSensor at x=0, y=11: closest beacon is at x=2, y=10\r\nSensor at x=20, y=14: closest beacon is at x=25, y=17\r\nSensor at x=17, y=20: closest beacon is at x=21, y=22\r\nSensor at x=16, y=7: closest beacon is at x=15, y=3\r\nSensor at x=14, y=3: closest beacon is at x=15, y=3\r\nSensor at x=20, y=1: closest beacon is at x=15, y=3\r\n";

      var notPositions = Zone.GetNotPositions(lines, 10);

      notPositions.Should().Be(26);
    }
  }
}