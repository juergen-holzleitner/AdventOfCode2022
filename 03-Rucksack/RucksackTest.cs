using FluentAssertions;

namespace _03_Rucksack
{
  public class RucksackTest
  {
    [Fact]
    public void Can_get_items_of_each_compartment()
    {
      var items = "vJrwpWtwJgWrhcsFMMfFFhFp";

      var result = Rucksack.GetCompartmentItems(items);

      result.CompartmentOne.Should().BeEquivalentTo("vJrwpWtwJgWr");
      result.CompartmentTwo.Should().BeEquivalentTo("hcsFMMfFFhFp");
    }

    [Theory]
    [InlineData("vJrwpWtwJgWrhcsFMMfFFhFp", 'p')]
    [InlineData("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL", 'L')]
    [InlineData("PmmdzqPrVvPwwTWBwg", 'P')]
    [InlineData("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn", 'v')]
    [InlineData("ttgJtRGJQctTZtZT", 't')]
    [InlineData("CrZsJsPPZsGzwwsLwLmpwMDw", 's')]
    public void Can_get_common_item(string items, char expectedResult)
    {
      var result = Rucksack.GetCommonItem(items);

      result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData('a', 1)]
    [InlineData('b', 2)]
    [InlineData('z', 26)]
    [InlineData('A', 27)]
    [InlineData('Z', 52)]
    public void Can_get_item_priority(char item, int expectedPriority)
    {
      var priority = Rucksack.GetItemPriority(item);
      priority.Should().Be(expectedPriority);
    }

    [Fact]
    public void Can_get_sum_of_priorities()
    {
      var input = "vJrwpWtwJgWrhcsFMMfFFhFp\r\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\r\nPmmdzqPrVvPwwTWBwg\r\nwMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\r\nttgJtRGJQctTZtZT\r\nCrZsJsPPZsGzwwsLwLmpwMDw";
      var sum = Rucksack.GetSumOfPriorities(input.Split('\n'));
      sum.Should().Be(157);
    }

    [Theory]
    [InlineData("vJrwpWtwJgWrhcsFMMfFFhFp\r\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\r\nPmmdzqPrVvPwwTWBwg", 'r')]
    [InlineData("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\r\nttgJtRGJQctTZtZT\r\nCrZsJsPPZsGzwwsLwLmpwMDw", 'Z')]
    public void Can_get_common_item_of_group(string input, char expectedItem)
    {
      var item = Rucksack.GetCommonGroupItem(input.Split('\n'));
      item.Should().Be(expectedItem);
    }

    [Fact]
    public void Can_get_sum_of_group_items()
    {
      var input = "vJrwpWtwJgWrhcsFMMfFFhFp\r\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\r\nPmmdzqPrVvPwwTWBwg\r\nwMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\r\nttgJtRGJQctTZtZT\r\nCrZsJsPPZsGzwwsLwLmpwMDw";
      var sum = Rucksack.GetSumOfGroupItems(input.Split('\n'));
      sum.Should().Be(70);
    }
  }
}