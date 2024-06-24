using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Menu.Leaderboard
{
    internal class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private Transform _holder;
        [SerializeField] private LeaderboardElement _prefab;

        private List<LeaderboardElement> _spawnedElements = new ();

        internal void ConstructLeaderboard(List<LeaderboardPlayer> leaderboardPlayers)
        {
            ClearLeaderboard();

            foreach (LeaderboardPlayer player in leaderboardPlayers)
            {
                LeaderboardElement spawnedElement = Instantiate(_prefab, _holder);
                spawnedElement.Init(player.Name, player.Rank, player.Score);

                _spawnedElements.Add(spawnedElement);
            }
        }

        private void ClearLeaderboard()
        {
            foreach (var element in _spawnedElements)
                Destroy(element.gameObject);

            _spawnedElements = new ();
        }
    }
}
