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

            Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
            {
                if (result == null || result.score < score)
                    Agava.YandexGames.Leaderboard.SetScore(LeaderboardName, score);
            });
        }
    }
}