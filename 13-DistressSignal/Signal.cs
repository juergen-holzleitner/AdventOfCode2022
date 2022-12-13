namespace _13_DistressSignal
{
  internal record Item();
  internal record ValueItem(int Value) : Item;
  internal record ListItem(List<Item> Items) : Item;
  internal record Packet(ListItem Item);

  internal record PacketPair(Packet Left, Packet Right);

  internal class Signal
  {
    internal static bool CheckPair(PacketPair packetPair)
    {
      return CheckList(packetPair.Left.Item, packetPair.Right.Item)!.Value;
    }

    private static bool? CheckList(ListItem left, ListItem right)
    {
      var leftEnumerator = left.Items.GetEnumerator();
      var rightEnumerator = right.Items.GetEnumerator();

      for (; ; )
      {
        if (!leftEnumerator.MoveNext())
        {
          if (rightEnumerator.MoveNext())
            return true;

          return null;
        }

        if (!rightEnumerator.MoveNext())
          return false;

        if (leftEnumerator.Current is ValueItem leftValue)
        {
          if (rightEnumerator.Current is ValueItem rightValue)
          {
            var comp = CheckValue(leftValue, rightValue);
            if (comp.HasValue)
              return comp.Value;
          }
          else
          {
            var comp = CheckList(new ListItem(new List<Item>() { leftValue }), (ListItem)rightEnumerator.Current);
            if (comp.HasValue)
              return comp.Value;
          }
        }
        else
        {
          if (rightEnumerator.Current is ValueItem rightValue)
          {
            var comp = CheckList((ListItem)leftEnumerator.Current, new ListItem(new List<Item>() { rightValue }));
            if (comp.HasValue)
              return comp.Value;
          }
          else
          {
            var comp = CheckList((ListItem)leftEnumerator.Current, (ListItem)rightEnumerator.Current);
            if (comp.HasValue)
              return comp.Value;
          }
        }
      }
    }

    private static bool? CheckValue(ValueItem left, ValueItem right)
    {
      if (left.Value < right.Value)
        return true;
      if (left.Value > right.Value)
        return false;

      return null;
    }

    internal static Packet Parse(string line)
    {
      var enumerator = line.GetEnumerator();
      if (!enumerator.MoveNext())
        throw new ApplicationException("no items in enumerator");

      var list = ParseList(enumerator);

      return new Packet(list);
    }

    internal static IEnumerable<PacketPair> ParseInput(IEnumerable<string> input)
    {
      var inputLines = input.GetEnumerator();
      for (; ; )
      {
        if (!inputLines.MoveNext())
          yield break;

        while (string.IsNullOrWhiteSpace(inputLines.Current))
          if (!inputLines.MoveNext())
            yield break;

        var left = Parse(inputLines.Current);
        if (!inputLines.MoveNext())
          throw new ApplicationException("missing right package");

        var right = Parse(inputLines.Current);
        yield return new PacketPair(left, right);
      }
    }

    private static ListItem ParseList(CharEnumerator enumerator)
    {
      if (enumerator.Current != '[')
        throw new ApplicationException($"expected '[' instead of '{enumerator.Current}'");

      var listItem = new ListItem(new List<Item>());

      while (enumerator.MoveNext())
      {
        if (char.IsDigit(enumerator.Current))
        {
          var value = ParseValue(enumerator);
          listItem.Items.Add(new ValueItem(value));
        }

        if (enumerator.Current == '[')
        {
          var list = ParseList(enumerator);
          listItem.Items.Add(list);
        }

        if (enumerator.Current == ']')
        {
          enumerator.MoveNext();
          return listItem;
        }

        if (enumerator.Current != ',')
          throw new ApplicationException($"expected ',' instead of {enumerator.Current}");
      }
      throw new ApplicationException("invalid state");
    }

    private static int ParseValue(CharEnumerator enumerator)
    {
      if (!char.IsDigit(enumerator.Current))
        throw new ApplicationException($"invalid char {enumerator.Current}");
      int val = 0;
      while (char.IsDigit(enumerator.Current))
      {
        val *= 10;
        val += int.Parse(enumerator.Current.ToString());
        if (!enumerator.MoveNext())
          throw new ApplicationException("not expected");
      }
      return val;
    }

    internal static int GetSumOfIndices(IEnumerable<string> lines)
    {
      var pairs = ParseInput(lines);
      int index = 1;
      int sum = 0;
      foreach (var pair in pairs)
      {
        if (CheckPair(pair))
          sum += index;

        ++index;
      }

      return sum;
    }
  }
}