namespace _03_Rucksack
{
  internal static class Rucksack
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

    internal static char GetCommonGroupItem(IEnumerable<string> rucksacks)
    {
      return rucksacks.Select(r => r.Trim()).Aggregate<IEnumerable<char>>((prev, next) => prev.Intersect(next)).Single();
    }

    internal static int GetSumOfGroupItems(IEnumerable<string> rucksacks)
    {
      return rucksacks.Batch(3).Sum(b => GetItemPriority(GetCommonGroupItem(b)));
    }

    private static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> enumerator, int batchsize)
    {
      T[]? batch = null;
      var count = 0;
      foreach (var item in enumerator)
      {
        batch ??= new T[batchsize];

        batch[count++] = item;
        if (count < batchsize)
          continue;

        yield return batch;
        batch = null;
        count = 0;
      }

      if (batch is not null)
        yield return batch.Take(count).ToArray();
    }
  }
}
