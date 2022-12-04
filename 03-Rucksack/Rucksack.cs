namespace _03_Rucksack
{
  internal class Rucksack
  {
    internal record struct Compartment(IEnumerable<char> CompartmentOne, IEnumerable<char> CompartmentTwo);

    internal static Compartment GetCompartmentItems(string items)
    {
      var halfSize = items.Length / 2;
      return new Compartment(items[..halfSize].ToCharArray(), items[halfSize..].ToCharArray());
    }

    internal static char GetCommonItem(string items)
    {
      var compartments = GetCompartmentItems(items);
      return compartments.CompartmentOne.Intersect(compartments.CompartmentTwo).Single();
    }

    internal static int GetItemPriority(char item)
    {
      if (item >= 'A' && item <= 'Z')
        return item - 'A' + 27;

      return item - 'a' + 1;
    }

    internal static int GetSumOfPriorities(IEnumerable<string> rucksacks)
    {
      return rucksacks.Sum(x => GetItemPriority(GetCommonItem(x)));
    }
  }
}
