using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayerHelpers
{
    public class CircleDrawer
    {
        private const int Angle = 360;
        private const int AngleStep = 10;
        private const float MoveOffset = 0.1f;

        private readonly float _radius;
        private readonly Transform _drawPoint;
        private readonly LineRenderer _line;

        private CancellationTokenSource _cancellation;
        private Vector3 _lastPosition;

        public CircleDrawer(float radius, Transform drawPoint, LineRenderer line)
        {
            _radius = radius;
            _drawPoint = drawPoint;
            _line = line;
        }

        public void StartDraw()
        {
            _cancellation = new CancellationTokenSource();
            Draw().Forget();
        }

        public void StopDraw()
        {
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
    }
}