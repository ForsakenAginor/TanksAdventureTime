using Assets.Source.Player.Input;
using Lean.Localization;
using System;
using TMPro;
using UnityEngine;

public class ShootingCooldownView : MonoBehaviour
{
    private const string ReadyStatus = "CHARGED";

    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private LeanLocalizedTextMeshProUGUI _localizedText;
    private float _cooldown;
    private float _remainingTime;
    private FireInputHandler _inputHandler;

    private void Update()
    {
        if (_inputHandler == null)
            return;

        if(_remainingTime > 0 )
        {
            _remainingTime -= Time.deltaTime;
            _textField.text = _remainingTime.ToString("0.00");
        }
        else if (_remainingTime <= 0 && _textField.text != ReadyStatus)
        {
            DisplayChargeStatus();
        }
    }

    private void OnDestroy()
    {
        _inputHandler.ShotFired -= OnShotFired;        
    }

    public void Init(FireInputHandler inputHandler, float cooldown)
    {
        _inputHandler = inputHandler != null ? inputHandler : throw new ArgumentNullException(nameof(inputHandler));
        _cooldown = cooldown > 0 ? cooldown : throw new ArgumentOutOfRangeException(nameof(_cooldown));
        DisplayChargeStatus();

        _inputHandler.ShotFired += OnShotFired;
    }

    private void OnShotFired()
    {
        _remainingTime = _cooldown;
    }
    private void DisplayChargeStatus()
    {
        _localizedText.TranslationName = ReadyStatus;
        _localizedText.UpdateLocalization();
    }
}
