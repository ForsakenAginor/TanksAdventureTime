using System;
using System.Collections.Generic;
using System.Threading;
using Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projectiles
{
    public class SwitchableExplosive : IExplosive, ISwitchable<IDamageableTarget>,
        ISwitchable<(ITarget projectile, Action explosionCallback)>
    {
        private readonly List<(ITarget projectile, Action explosionCallback)> _trackers;
        private readonly float _projectileRadius;

        private IDamageableTarget _target;
        private CancellationTokenSource _cancellation;

        public SwitchableExplosive(float projectileRadius)
        {
            _trackers = new List<(ITarget projectile, Action explosionCallback)>();
            _projectileRadius = projectileRadius * projectileRadius;
        }

        public void Explode(Vector3 position, float radius)
        {
            if (_target == null)
                return;

            if (Vector3.Distance(position, _target.Position) > radius)
                return;

            _target.TakeHit(HitTypes.Explosion);
        }

        public void Switch(IDamageableTarget target)
        {
            _target = target;
        }

        public void Switch((ITarget projectile, Action explosionCallback) tuple)
        {
            if (tuple.projectile == null || tuple.explosionCallback == null)
                throw new ArgumentNullException();

            _trackers.Add(tuple);
        }

        public void StartTracking()
        {
            if (_cancellation?.IsCancellationRequested == true)
                return;

            _cancellation = new CancellationTokenSource();
            Track().Forget();
        }

        public void StopTracking()
        {
            if (_cancellation.IsCancellationRequested == true)
                return;

            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid Track()
        {
            while (_cancellation.IsCancellationRequested == false)
                await ExplodeClosest();
        }

        private UniTask ExplodeClosest()
        {
            (ITarget projectile, Action explosionCallback) tracker =
                _trackers.Find(TryContact);

            if (tracker.projectile == null)
                return UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, _cancellation.Token);

            _trackers.Remove(tracker);
            tracker.explosionCallback.Invoke();
            return UniTask.CompletedTask;
        }

        private bool TryContact((ITarget projectile, Action explosionCallback) tracker)
        {
            if (_trackers.Count == (int)ValueConstants.Zero)
                return false;

            if (_target != null)
                return Vector3.Distance(tracker.projectile.Position, _target.Position) <= _projectileRadius;

            _trackers.Clear();
            return false;
        }
    }
}