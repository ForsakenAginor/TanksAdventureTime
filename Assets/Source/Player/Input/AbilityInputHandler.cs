using System;
using UnityEngine;

namespace Assets.Source.Player.Input
{
    public class AbilityInputHandler
    {
        private readonly PlayerInput _playerInput;

        public AbilityInputHandler(PlayerInput playerInput)
        {
            _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));

            _playerInput.AbilityInputReceived += OnInputReceived;
        }

        ~AbilityInputHandler()
        {
            _playerInput.AbilityInputReceived -= OnInputReceived;
        }

        private void OnInputReceived()
        {
            Debug.Log("Not implemented yet");
        }
    }
}