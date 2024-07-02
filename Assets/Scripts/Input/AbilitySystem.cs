using System;
using UnityEngine;

public class AbilitySystem
{
    private readonly PlayerInput _playerInput;

    public AbilitySystem(PlayerInput playerInput)
    {
        _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));

        _playerInput.AbilityInputReceived += OnInputReceived;
    }

    ~AbilitySystem()
    {
        _playerInput.AbilityInputReceived -= OnInputReceived;
    }

    private void OnInputReceived()
    {
        Debug.Log("Not implemented yet");
    }
}
