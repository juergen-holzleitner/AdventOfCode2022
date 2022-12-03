using FluentAssertions;

namespace _02_RockPaperScissors
{
  public class RockPaperScissorsTest
  {
    [Theory]
    [InlineData('A', RockPaperScissors.Shape.Rock)]
    [InlineData('B', RockPaperScissors.Shape.Paper)]
    [InlineData('C', RockPaperScissors.Shape.Scissors)]
    public void Can_parse_Opponent(char inputShape, RockPaperScissors.Shape result)
    {
      var shape = RockPaperScissors.ParseOpponent(inputShape);

      shape.Should().Be(result);
    }

    [Theory]
    [InlineData('X', RockPaperScissors.Shape.Rock)]
    [InlineData('Y', RockPaperScissors.Shape.Paper)]
    [InlineData('Z', RockPaperScissors.Shape.Scissors)]
    public void Can_parse_Player(char inputShape, RockPaperScissors.Shape expectedShape)
    {
      var shape = RockPaperScissors.ParsePlayer(inputShape);

      shape.Should().Be(expectedShape);
    }

    [Theory]
    [InlineData('X', RockPaperScissors.Outcome.Lose)]
    [InlineData('Y', RockPaperScissors.Outcome.Draw)]
    [InlineData('Z', RockPaperScissors.Outcome.Win)]
    public void Can_parse_Outcome(char input, RockPaperScissors.Outcome expectedOutcome)
    {
      var shape = RockPaperScissors.ParseOutcome(input);

      shape.Should().Be(expectedOutcome);
    }

    [Fact]
    public void Can_parse_single_game()
    {
      var gameInput = "A Y";

      var gameResult = RockPaperScissors.ParseGame(gameInput);

      gameResult.Should().Be(new RockPaperScissors.GameResult(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Paper));
    }

    [Fact]
    public void Can_parse_single_input()
    {
      var outomeInput = "A Y";

      var outcomeInput = RockPaperScissors.ParseOutcomeInput(outomeInput);

      outcomeInput.Should().Be(new RockPaperScissors.OutcomeInput(RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Draw));
    }

    [Theory]
    [InlineData(RockPaperScissors.Shape.Rock, 1)]
    [InlineData(RockPaperScissors.Shape.Paper, 2)]
    [InlineData(RockPaperScissors.Shape.Scissors, 3)]
    public void Can_get_shape_score(RockPaperScissors.Shape shape, int expectedScore)
    {
      var score = RockPaperScissors.GetShapeScore(shape);

      score.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Paper, RockPaperScissors.Outcome.Win)]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Draw)]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Scissors, RockPaperScissors.Outcome.Lose)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Lose)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Shape.Scissors, RockPaperScissors.Outcome.Win)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Shape.Paper, RockPaperScissors.Outcome.Lose)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Win)]
    public void Can_get_round_outcome(RockPaperScissors.Shape opponent, RockPaperScissors.Shape player, RockPaperScissors.Outcome expectedOutcome)
    {
      var gameInput = new RockPaperScissors.GameResult(opponent, player);

      var score = RockPaperScissors.GetRoundOutcome(gameInput);

      score.Should().Be(expectedOutcome);
    }

    [Theory]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Paper, 6)]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Rock, 3)]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Scissors, 0)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Shape.Rock, 0)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Shape.Scissors, 6)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Shape.Paper, 0)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Shape.Rock, 6)]
    public void Can_get_result_score(RockPaperScissors.Shape opponent, RockPaperScissors.Shape player, int expectedScore)
    {
      var gameInput = new RockPaperScissors.GameResult(opponent, player);

      var score = RockPaperScissors.GetRoundScore(gameInput);

      score.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Shape.Paper, 8)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Shape.Rock, 1)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Shape.Scissors, 6)]
    public void Can_get_game_score(RockPaperScissors.Shape opponent, RockPaperScissors.Shape player, int expectedScore)
    {
      var gameInput = new RockPaperScissors.GameResult(opponent, player);

      var score = RockPaperScissors.GetGameScore(gameInput);

      score.Should().Be(expectedScore);
    }

    [Fact]
    public void Can_get_game_score_from_input()
    {
      var gameInput = "A Y";

      var score = RockPaperScissors.GetRoundScore(gameInput);
      
      score.Should().Be(8);
    }

    [Fact]
    public void Can_get_game_score_from_outcome_input()
    {
      var gameInput = "A Y";

      var score = RockPaperScissors.GetOutcomeRoundScore(gameInput);

      score.Should().Be(4);
    }

    [Fact]
    public void Can_get_total_game_score()
    {
      var gameInput = "A Y\r\nB X\r\nC Z";

      var totalScore = RockPaperScissors.GetTotalScore(gameInput.Split('\n'));

      totalScore.Should().Be(15);
    }

    [Fact]
    public void Can_get_total_game_input_score()
    {
      var gameInput = "A Y\r\nB X\r\nC Z";

      var totalScore = RockPaperScissors.GetTotalInputScore(gameInput.Split('\n'));

      totalScore.Should().Be(12);
    }

    [Theory]
    [InlineData(RockPaperScissors.Outcome.Lose, 0)]
    [InlineData(RockPaperScissors.Outcome.Draw, 3)]
    [InlineData(RockPaperScissors.Outcome.Win, 6)]
    public void Can_get_score_from_game_outcome(RockPaperScissors.Outcome gameOutcome, int expectedScore)
    {
      var score = RockPaperScissors.GetScoreFromOutcome(gameOutcome);

      score.Should().Be(expectedScore);
    }

    [Theory]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Draw, RockPaperScissors.Shape.Rock)]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Win, RockPaperScissors.Shape.Paper)]
    [InlineData(RockPaperScissors.Shape.Rock, RockPaperScissors.Outcome.Lose, RockPaperScissors.Shape.Scissors)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Outcome.Win, RockPaperScissors.Shape.Scissors)]
    [InlineData(RockPaperScissors.Shape.Paper, RockPaperScissors.Outcome.Lose, RockPaperScissors.Shape.Rock)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Outcome.Win, RockPaperScissors.Shape.Rock)]
    [InlineData(RockPaperScissors.Shape.Scissors, RockPaperScissors.Outcome.Lose, RockPaperScissors.Shape.Paper)]
    public void Can_get_player_shape_from_outcome(RockPaperScissors.Shape opponentShape, RockPaperScissors.Outcome outcome, RockPaperScissors.Shape expectedPlayerShape)
    {
      var outcomeInput = new RockPaperScissors.OutcomeInput(opponentShape, outcome);
      var playerShape = RockPaperScissors.GetPlayerShapeFromOutcomeInput(outcomeInput);
      playerShape.Should().Be(expectedPlayerShape);
    }

  }
}