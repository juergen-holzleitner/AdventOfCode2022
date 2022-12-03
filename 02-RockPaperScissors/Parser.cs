namespace _02_RockPaperScissors
{
  public static class Parser
  {
    public enum Shape { Rock, Paper, Scissors };

    public static Shape ParseOpponent(char shapeInput)
    {
      if (shapeInput == 'A')
        return Shape.Rock;
      if (shapeInput == 'B')
        return Shape.Paper;
      if (shapeInput == 'C')
        return Shape.Scissors;

      throw new ArgumentException($"{nameof(shapeInput)} has invalid value {shapeInput}");
    }

    public static Shape ParsePlayer(char shapeInput)
    {
      if (shapeInput == 'X')
        return Shape.Rock;
      if (shapeInput == 'Y')
        return Shape.Paper;
      if (shapeInput == 'Z')
        return Shape.Scissors;

      throw new ArgumentException($"{nameof(shapeInput)} has invalid value {shapeInput}");
    }
  }
}
