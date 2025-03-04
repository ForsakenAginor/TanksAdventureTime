using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

namespace UI
{
    public class YandexLeaderboard : MonoBehaviour
    {
        private const string LeaderboardName = "Leaderboard1";
        private const string AnonymousName = "Anonymous";

        private readonly List<LeaderboardPlayer> _leaderboardPlayers = new ();

        [SerializeField] private LeaderboardView _leaderboardView;

        public void SetPlayerScore(int score, Action callback = null)
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            Leaderboard.GetPlayerEntry(
                LeaderboardName,
                result =>
                {
                    if (result == null || result.score < score)
                        Leaderboard.SetScore(LeaderboardName, score, callback);
                    else
                        callback?.Invoke();
                });
        }

        public void Fill()
        {
            if (PlayerAccount.IsAuthorized == false)
                return;

            _leaderboardPlayers.Clear();

            Leaderboard.GetEntries(
                LeaderboardName,
                result =>
                {
                    foreach (var entry in result.entries)
                    {
                        int rank = entry.rank;
                        int score = entry.score;
                        string name = entry.player.publicName;

                        if (string.IsNullOrEmpty(name))
                            name = AnonymousName;

                        _leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
                    }

                    _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
                });
        }
    }
}