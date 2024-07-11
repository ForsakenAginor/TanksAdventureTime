﻿using Assets.Source.Player.Weapons;
using System;

namespace Assets.Source.Player.Input
{
    public class FireInputHandler
    {
        private readonly PlayerWeapon _playerWeapon;
        private readonly PlayerInput _playerInput;

        public FireInputHandler(PlayerInput playerInput, PlayerWeapon playerWeapon)
        {
            _playerInput = playerInput != null ? playerInput : throw new ArgumentNullException(nameof(playerInput));
            _playerWeapon = playerWeapon != null ? playerWeapon : throw new ArgumentNullException(nameof(playerWeapon));

            _playerInput.FireInputReceived += OnInputReceived;
        }

        ~FireInputHandler()
        {
            _playerInput.FireInputReceived -= OnInputReceived;
        }

        public event Action ShotFired;

        private void OnInputReceived()
        {
            _playerWeapon.Shoot();
            ShotFired?.Invoke();
        }
    }
}