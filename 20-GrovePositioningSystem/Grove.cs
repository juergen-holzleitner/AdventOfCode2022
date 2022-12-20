namespace _20_GrovePositioningSystem
{
  internal class Grove
  {
    internal record struct Element(int Value, int Position);
    internal record Input(List<Element> Elements);

    internal static Input ParseInput(string input)
    {
      var numbers = new List<Element>();
      int index = 0;
      foreach (var line in input.Split('\n'))
        if (!string.IsNullOrWhiteSpace(line))
        {

          var number = int.Parse(line.Trim());
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
        for (int n = 0; n < element.Value; ++n)
        {
          SwapForward(input.Elements, currentIndex + n);
        }
      }
      else
      {
        for (int n = 0; n < Math.Abs(element.Value); ++n)
        {
          SwapBackward(input.Elements, currentIndex - n);
        }
      }

      //var newIndex = (currentIndex + element.Value) % (input.Elements.Count - 1);
      //if (newIndex < 0)
      //{
      //  newIndex += input.Elements.Count;
      //}

      //input.Elements.RemoveAt(currentIndex);
      //input.Elements.Insert(newIndex, element);
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

    internal static List<Element> MoveAllElements(string inputData)
    {
      var input = ParseInput(inputData);
      for (int n = 0; n < input.Elements.Count; n++)
        MoveElement(input, n);
      return input.Elements;
    }

    internal static int GetGroveCoordinate(string inputData)
    {
      var elements = MoveAllElements(inputData);

      var zeroIndex = elements.FindIndex(e => e.Value == 0);

      var sum = 0;
      for (int n = 0; n < 3; ++n)
      {
        var currentIndex = zeroIndex + (n + 1) * 1000;
        currentIndex %= elements.Count;

        sum += elements[currentIndex].Value;
      }

      return sum;
    }

    internal static List<Element> MoveAllElementsForNSteps(string inputData, int numSteps)
    {
      var input = ParseInput(inputData);
      for (int n = 0; n < numSteps; n++)
        MoveElement(input, n);
      return input.Elements;
    }
  }
}