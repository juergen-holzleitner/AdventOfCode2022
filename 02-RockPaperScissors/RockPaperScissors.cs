namespace _02_RockPaperScissors
{
  public static class RockPaperScissors
  {
    public enum Shape { Rock, Paper, Scissors };

    public record struct GameResult(Shape OpponentShape, Shape PlayerShape);

    public static Shape ParseOpponent(char shapeInput)
    {
      return shapeInput switch
      {
        'A' => Shape.Rock,
        'B' => Shape.Paper,
        'C' => Shape.Scissors,
        _ => throw new ArgumentException($"{nameof(shapeInput)} has invalid value {shapeInput}"),
      };
    }

    public static Shape ParsePlayer(char shapeInput)
    {
      return shapeInput switch
      {
        'X' => Shape.Rock,
        'Y' => Shape.Paper,
        'Z' => Shape.Scissors,
        _ => throw new ArgumentException($"{nameof(shapeInput)} has invalid value {shapeInput}"),
      };
    }

    internal static GameResult ParseGame(string gameInput)
    {
      var shapeInputs = gameInput.Split(' ');
      return new GameResult(ParseOpponent(shapeInputs[0].Single()), ParsePlayer(shapeInputs[1].Single()));
    }

    internal static int GetShapeScore(Shape shape)
    {
      return shape switch
      {
        Shape.Rock => 1,
        Shape.Paper => 2,
        Shape.Scissors => 3,
        _ => throw new ArgumentException($"{nameof(shape)} has invalid value {shape}"),
      };
    }

    internal static int GetRoundScore(GameResult gameInput)
    {
      if (gameInput.OpponentShape == gameInput.PlayerShape)
        return 3;

      if (gameInput.OpponentShape == Shape.Rock)
        return gameInput.PlayerShape == Shape.Paper ? 6 : 0;

      if (gameInput.OpponentShape == Shape.Paper)
        return gameInput.PlayerShape == Shape.Scissors ? 6 : 0;

      return gameInput.PlayerShape == Shape.Rock ? 6 : 0;
    }

    internal static int GetGameScore(GameResult gameInput)
    {
      return GetShapeScore(gameInput.PlayerShape) + GetRoundScore(gameInput);
    }

    internal static int GetRoundScore(string gameInput)
    {
      return GetGameScore(ParseGame(gameInput));
    }

    internal static int GetTotalScore(IEnumerable<string> gameInputs)
    {
      return gameInputs.Sum(x => GetRoundScore(x.Trim()));
    }
  }
}
