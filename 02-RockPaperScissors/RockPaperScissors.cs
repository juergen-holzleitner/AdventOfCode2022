namespace _02_RockPaperScissors
{
  public static class RockPaperScissors
  {
    public enum Shape { Rock, Paper, Scissors };

    public enum Outcome { Lose, Draw, Win };

    public record struct GameResult(Shape OpponentShape, Shape PlayerShape);

    public record struct OutcomeInput(Shape OpponentShape, Outcome Outcome);

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

    public static Outcome ParseOutcome(char outcomeInput)
    {
      return outcomeInput switch
      {
        'X' => Outcome.Lose,
        'Y' => Outcome.Draw,
        'Z' => Outcome.Win,
        _ => throw new ArgumentException($"{nameof(outcomeInput)} has invalid value {outcomeInput}"),
      };
    }

    internal static GameResult ParseGame(string gameInput)
    {
      var shapeInputs = gameInput.Split(' ');
      return new GameResult(ParseOpponent(shapeInputs[0].Single()), ParsePlayer(shapeInputs[1].Single()));
    }

    internal static OutcomeInput ParseOutcomeInput(string outcomeInput)
    {
      var inputs = outcomeInput.Split(' ');
      return new OutcomeInput(ParseOpponent(inputs[0].Single()), ParseOutcome(inputs[1].Single()));
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

    internal static Outcome GetRoundOutcome(GameResult gameInput)
    {
      if (gameInput.OpponentShape == gameInput.PlayerShape)
        return Outcome.Draw;

      if (gameInput.OpponentShape == Shape.Rock)
        return gameInput.PlayerShape == Shape.Paper ? Outcome.Win : Outcome.Lose;

      if (gameInput.OpponentShape == Shape.Paper)
        return gameInput.PlayerShape == Shape.Scissors ? Outcome.Win : Outcome.Lose;

      return gameInput.PlayerShape == Shape.Rock ? Outcome.Win : Outcome.Lose;
    }

    internal static int GetRoundScore(GameResult gameInput)
    {
      return GetScoreFromOutcome(GetRoundOutcome(gameInput));
    }

    internal static int GetOutcomeRoundScore(string gameInput)
    {
      var outcomeInput = ParseOutcomeInput(gameInput);
      var playerShape = GetPlayerShapeFromOutcomeInput(outcomeInput);
      return GetShapeScore(playerShape) + GetScoreFromOutcome(outcomeInput.Outcome);

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

    internal static int GetTotalInputScore(IEnumerable<string> gameInputs)
    {
      return gameInputs.Sum(x => GetOutcomeRoundScore(x.Trim()));
    }

    internal static int GetScoreFromOutcome(Outcome gameOutcome)
    {
      return gameOutcome switch 
      {
        Outcome.Lose => 0,
        Outcome.Draw => 3,
        Outcome.Win => 6,
        _ => throw new ArgumentException($"{nameof(gameOutcome)} has invalid value {gameOutcome}"),
      };
    }

    internal static Shape GetPlayerShapeFromOutcomeInput(OutcomeInput outcomeInput)
    {
      if (outcomeInput.Outcome == Outcome.Draw)
        return outcomeInput.OpponentShape;

      if (outcomeInput.OpponentShape == Shape.Rock)
        return outcomeInput.Outcome == Outcome.Win ? Shape.Paper : Shape.Scissors;

      if (outcomeInput.OpponentShape == Shape.Paper)
        return outcomeInput.Outcome == Outcome.Win ? Shape.Scissors : Shape.Rock;

      return outcomeInput.Outcome == Outcome.Win ? Shape.Rock : Shape.Paper;
    }

  }
}
