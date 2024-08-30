using Agava.YandexGames;

namespace UI
{
    public class LeaderboardScoreSaver
    {
        public void SaveScore(int score)
        {
            const string LeaderboardName = "Leaderboard1";

            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(LeaderboardName, result =>
            {
                if (result == null || result.score < score)
                    Leaderboard.SetScore(LeaderboardName, score);
            });
        }
    }
}