using System;
using UnityEngine;

namespace Assets.Source.Player.Input
{
    [Serializable]
    public class PidRegulator
    {
        [SerializeField] private float _ki = 1;
        [SerializeField] private float _kp = 1;
        [SerializeField] private float _kd = 1;
        [SerializeField] private float _maxOut = 1000;
        [SerializeField] private float _minOut = -1000;

        private float _integral, _previousError, _error, _currentDelta, _targetPoint;

        public PidRegulator()
        {
        }

        public PidRegulator(float ki, float kp, float kd)
        {
            _ki = ki;
            _kp = kp;
            _kd = kd;
        }

        public float Tick(float input, float time) => ComputePID(time, input);

        public void SetTarget(float target) => _targetPoint = target;

        public void SetMinMax(float min, float max)
        {
            _minOut = min;
            _maxOut = max;
        }

        public float Tick(float input, float target, float time)
        {
            SetTarget(target);
            return ComputePID(time, input);
        }

        private float ComputePID(float time, float input)
        {
            _error = _targetPoint - input;
            _integral = Mathf.Clamp(_integral + _error * time * _ki, _minOut, _maxOut);
            _currentDelta = (_error - _previousError) / time;
            _previousError = _error;
            return Mathf.Clamp(_error * _kp + _integral + _currentDelta * _kd, _minOut, _maxOut);
        }
    }
}