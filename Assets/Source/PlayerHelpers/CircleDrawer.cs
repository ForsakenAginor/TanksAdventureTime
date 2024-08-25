using System.Collections.Generic;
using System.Threading;
using Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayerHelpers
{
    public class CircleDrawer : ISwitchable<ITarget>
    {
        private const int Angle = 360;
        private const int AngleStep = 10;
        private const float MoveOffset = 0.1f;

        private readonly float _radius;
        private readonly Transform _drawPoint;
        private readonly LineRenderer _line;
        private readonly Color _available;
        private readonly Color _standard;

        private CancellationTokenSource _cancellation;
        private Vector3 _lastPosition;

        public CircleDrawer(float radius, Transform drawPoint, LineRenderer line)
        {
            _radius = radius;
            _drawPoint = drawPoint;
            _line = line;
            _available = Color.green;
            _standard = Color.white;
        }

        public void StartDraw()
        {
            _cancellation = new CancellationTokenSource();
            Draw().Forget();
        }

        public void StopDraw()
        {
            if (_cancellation == null)
                return;

            if (_cancellation.IsCancellationRequested == true)
                return;

            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid Draw()
        {
            while (_cancellation.IsCancellationRequested == false)
            {
                if (Vector3.Distance(_lastPosition, _drawPoint.position) > MoveOffset)
                {
                    List<Vector3> points = new ();
                    _lastPosition = _drawPoint.position;

                    for (int i = 0; i < Angle; i += AngleStep)
                    {
                        points.Add(
                            new Vector3(
                                _lastPosition.x + _radius * Mathf.Cos(i * Mathf.Deg2Rad),
                                _lastPosition.y,
                                _lastPosition.z + _radius * Mathf.Sin(i * Mathf.Deg2Rad)));
                    }

                    _line.positionCount = points.Count;
                    _line.SetPositions(points.ToArray());
                }

                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, _cancellation.Token);
            }
        }

        public void Switch(ITarget target)
        {
            if (target == null)
            {
                _line.startColor = _standard;
                _line.endColor = _standard;
                return;
            }

            _line.startColor = _available;
            _line.endColor = _available;
        }
    }
}