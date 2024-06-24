using System;
using Agava.WebUtility;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Silencer : MonoBehaviour
    {
        private readonly float _minValue = 0f;
        private readonly float _maxValue = 1f;
        private GameState _gameState;
        private bool _inApp = true;

        private void OnEnable()
        {
            _gameState = new (_maxValue, _maxValue, false);
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
            Application.focusChanged += OnFocusChanged;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
            Application.focusChanged -= OnFocusChanged;
        }

        public void SetGameState(float timeScale, float volume, bool isPausing)
        {
            if (timeScale < _minValue && timeScale > _maxValue)
                throw new ArgumentOutOfRangeException(nameof(timeScale));

            if (volume < _minValue && volume > _maxValue)
                throw new ArgumentOutOfRangeException(nameof(volume));

            _gameState = new (timeScale, volume, isPausing);
        }

        private void OnFocusChanged(bool isFocused)
        {
            if (isFocused == false)
                PauseGame();
            else
                UnpauseGame();
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            if (inBackground)
                PauseGame();
        }

        private void UnpauseGame()
        {
            AudioListener.pause = _gameState.IsPausing;
            AudioListener.volume = _gameState.Volume;
            Time.timeScale = _gameState.TimeScale;
            _inApp = true;
        }

        private void PauseGame()
        {
            if (_inApp)
            {
                _gameState = new (Time.timeScale, AudioListener.volume, AudioListener.pause);
                _inApp = false;
            }

            AudioListener.pause = true;
            AudioListener.volume = _minValue;
            Time.timeScale = _minValue;
        }

        private readonly struct GameState
        {
            public readonly float TimeScale;
            public readonly float Volume;
            public readonly bool IsPausing;

            public GameState(float timeScale, float volume, bool isPausing)
            {
                float minValue = 0f;
                float maxValue = 1f;
                TimeScale = timeScale >= minValue && timeScale <= maxValue ? timeScale : throw new ArgumentOutOfRangeException(nameof(timeScale));
                Volume = volume >= minValue && volume <= maxValue ? volume : throw new ArgumentOutOfRangeException(nameof(volume));
                IsPausing = isPausing;
            }
        }
    }
}