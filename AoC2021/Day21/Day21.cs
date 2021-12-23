namespace AoC2021.Day21;

public class Day21 : IMDay
{
    // Outcomes after 3 dice roles with the number of universes per outcome
    private static readonly Dictionary<int, int> _diracDiceOutcomes = new()
    {
        { 3, 1 },
        { 4, 3 },
        { 5, 6 },
        { 6, 7 },
        { 7, 6 },
        { 8, 3 },
        { 9, 1 },
    };
    private readonly Dictionary<DiracGameState, (long, long)> _gameStateCache = new();

    public string FilePath { private get; init; } = "Day21\\input.txt";

    public async Task<string> GetAnswerPart1()
    {
        var (player1, player2) = await GetPlayers();
        Dice100 dice = new();

        while ((player1 = player1.PlayRound(dice.RollDiceThrice())).Score < 1000 &&
               (player2 = player2.PlayRound(dice.RollDiceThrice())).Score < 1000);

        var result = Math.Min(player1.Score, player2.Score) * dice.TotalRolls;
        return result.ToString();
    }

    public async Task<string> GetAnswerPart2()
    {
        var (player1, player2) = await GetPlayers();
        var (player1Wins, player2Wins) = PlayDiracRound(player1, player2);

        return Math.Max(player1Wins, player2Wins).ToString();
    }

    private (long, long) PlayDiracRound(Player player1, Player player2)
    {
        long player1Wins = 0;
        long player2Wins = 0;

        var gameState = new DiracGameState(player1, player2);
        if (_gameStateCache.ContainsKey(gameState))
        {
            return _gameStateCache[gameState];
        }

        foreach (var outcome in _diracDiceOutcomes.Keys)
        {
            var newPlayer1 = player1.PlayRound(outcome);
            if (newPlayer1.Score >= 21)
            {
                player1Wins += _diracDiceOutcomes[outcome];
            }
            else
            {
                var (wins2, wins1) = PlayDiracRound(player2, newPlayer1);
                player1Wins += wins1 * _diracDiceOutcomes[outcome];
                player2Wins += wins2 * _diracDiceOutcomes[outcome];
            }
        }

        _gameStateCache.Add(gameState, (player1Wins, player2Wins));

        return (player1Wins, player2Wins);
    }

    private async Task<(Player, Player)> GetPlayers()
    {
        var players = (await File.ReadAllLinesAsync(FilePath))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => l.Split(' ').Last())
            .Select(p => new Player(int.Parse(p), 0))
            .ToArray();

        return (players.First(), players.Last());
    }

    private class Dice100
    {
        private int _currentState = 0;

        public int TotalRolls { get; private set; } = 0;

        public int RollDiceThrice() => RollDice() + RollDice() + RollDice();

        public int RollDice()
        {
            _currentState = (++_currentState - 1) % 100 + 1;
            TotalRolls++;
            return _currentState;
        }
    }

    private record struct Player(int Position, int Score)
    {
        public Player PlayRound(int rolledNumber)
        {
            var newPosition = (Position + rolledNumber - 1) % 10 + 1;
            var newScore = Score + newPosition;
            return new Player(newPosition, newScore);
        }
    }

    private record struct DiracGameState(Player Player1, Player Player2);
}