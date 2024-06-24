namespace Assets.Scripts.UI.Menu.Leaderboard
{
    internal class LeaderboardPlayer
    {
        internal LeaderboardPlayer(int rank, string name, int score)
        {
            Rank = rank;
            Name = name;
            Score = score;
        }

        internal int Rank { get; }

        internal string Name { get; }

        internal int Score { get; }
    }
}
