namespace _20_GrovePositioningSystem
{
  internal class Grove
  {
    internal record struct Element(long Value, int Position);
    internal record Input(List<Element> Elements);

    internal static Input ParseInput(string input, long multiplier)
    {
      var numbers = new List<Element>();
      int index = 0;
      foreach (var line in input.Split('\n'))
        if (!string.IsNullOrWhiteSpace(line))
        {

          var number = long.Parse(line.Trim()) * multiplier;
          numbers.Add(new Element(number, index));
          ++index;
        }

      return new Input(numbers);
    }

    internal static void MoveElement(Input input, int position)
    {
      var currentIndex = input.Elements.FindIndex(e => e.Position == position);
      var element = input.Elements[currentIndex];
      if (element.Value == 0)
        return;

      if (element.Value > 0)
      {
        long numMoves = element.Value % (input.Elements.Count - 1);
        for (long n = 0; n < numMoves; ++n)
        {
          SwapForward(input.Elements, (int)(currentIndex + n));
        }
      }
      else
      {
        long numMoves = Math.Abs(element.Value) % (input.Elements.Count - 1);
        for (long n = 0; n < numMoves; ++n)
        {
          SwapBackward(input.Elements, (int)(currentIndex - n));
        }
      }
    }

    private static void SwapForward(List<Element> elements, int position)
    {
      int posOld = ModuloPosition(elements.Count, position);
      int posNew = ModuloPosition(elements.Count, position + 1);
      Swap(elements, posOld, posNew);
    }

    private static void Swap(List<Element> elements, int posOld, int posNew)
    {
      var tmp = elements[posOld];
      elements[posOld] = elements[posNew];
      elements[posNew] = tmp;
    }

    private static void SwapBackward(List<Element> elements, int position)
    {
      int posOld = ModuloPosition(elements.Count, position);
      int posNew = ModuloPosition(elements.Count, position - 1);
      Swap(elements, posOld, posNew);
    }

    private static int ModuloPosition(int count, int position)
    {
      var newPos = position % count;
      if (newPos < 0)
        newPos += count;
      return newPos;
    }

    internal static List<Element> MoveAllElements(string inputData, long multiplier, int numMoves)
    {
      var input = ParseInput(inputData, multiplier);
      for (int m = 0; m < numMoves; ++m)
      {
        for (int n = 0; n < input.Elements.Count; n++)
          MoveElement(input, n);
      }
      return input.Elements;
    }

    internal static long GetGroveCoordinate(string inputData, long multiplier, int numMoves)
    {
      var elements = MoveAllElements(inputData, multiplier, numMoves);

      var zeroIndex = elements.FindIndex(e => e.Value == 0);

      long sum = 0;
      for (int n = 0; n < 3; ++n)
      {
        var currentIndex = zeroIndex + (n + 1) * 1000;
        currentIndex %= elements.Count;

        sum += elements[currentIndex].Value;
      }

      return sum;
    }

    internal static List<Element> MoveAllElementsForNSteps(string inputData, long multiplier, int numSteps)
    {
      var input = ParseInput(inputData, multiplier);
      for (int n = 0; n < numSteps; n++)
        MoveElement(input, n);
      return input.Elements;
    }
  }
}