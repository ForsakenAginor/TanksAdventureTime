using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PlayerHelpers
{
    public class TargetSwitcher
    {
        private readonly List<IDamageableTarget> _targets;
        private readonly List<ISwitchable<IDamageableTarget>> _switchableObjects;
        private readonly Transform _transform;
        private readonly ImaginaryFieldOfView _fieldOfView;

        private CancellationTokenSource _cancellation;
        private IDamageableTarget _current;

        public TargetSwitcher(
            List<IDamageableTarget> targets,
            List<ISwitchable<IDamageableTarget>> switchableObjects,
            Transform transform,
            ImaginaryFieldOfView fieldOfView)
        {
            _targets = targets;
            _switchableObjects = switchableObjects;
            _transform = transform;
            _fieldOfView = fieldOfView;
        }

        public void StartSearching()
        {
            _cancellation = new CancellationTokenSource();
            Search().Forget();
        }

        public void StopSearching()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid Search()
        {
            while (_cancellation.IsCancellationRequested == false)
            {
                Vector3 currentPosition = _transform.position;
                IDamageableTarget resultTarget = _current;

                if (_current != null &&
                    (_current.Priority == TargetPriority.None || _fieldOfView.CanView(_current) == false))
                {
                    resultTarget = null;
                }

                IReadOnlyCollection<IDamageableTarget> available = await GetTargetsInRadius();

                if (available.Any())
                    resultTarget = await GetTarget(available, currentPosition);

                if (_current != resultTarget)
                {
                    _current = resultTarget;
                    Notify();
                }

                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, _cancellation.Token);
            }
        }

        private async UniTask<IDamageableTarget> GetTarget(
            IReadOnlyCollection<IDamageableTarget> available,
            Vector3 currentPosition)
        {
            TargetPriority priority = await FindHightestPriority(available);

            if (_current == null || _current.Priority < priority)
                return await FindTarget(await FindPrioritizedTargets(priority, available), currentPosition);

            return _current;
        }

        private async UniTask<List<IDamageableTarget>> GetTargetsInRadius()
        {
            List<IDamageableTarget> result = new List<IDamageableTarget>();

            foreach (IDamageableTarget target in _targets)
            {
                if (target == null)
                {
                    _targets.Remove(null);
                    continue;
                }

                if (target.Priority == TargetPriority.None)
                    continue;

                if (_fieldOfView.CanView(target))
                    result.Add(target);
            }

            return await UniTask.FromResult(result);
        }

        private async UniTask<TargetPriority> FindHightestPriority(IEnumerable<IDamageableTarget> targets)
        {
            return await UniTask.FromResult(
                (TargetPriority)Mathf.Max(targets.Select(target => (int)target.Priority).ToArray()));
        }

        private async UniTask<IEnumerable<IDamageableTarget>> FindPrioritizedTargets(
            TargetPriority priority,
            IEnumerable<IDamageableTarget> hightestPriorityTargets)
        {
            return await UniTask.FromResult(hightestPriorityTargets.Where(target => target.Priority == priority));
        }

        private async UniTask<IDamageableTarget> FindTarget(
            IEnumerable<IDamageableTarget> prioritizedTargets,
            Vector3 position)
        {
            float resultDistance = float.MaxValue;
            IDamageableTarget result = null;

            foreach (IDamageableTarget target in prioritizedTargets)
            {
                float distance = Vector3.Distance(target.Position, position);

                if (distance >= resultDistance)
                    continue;

                resultDistance = distance;
                result = target;
            }

            return await UniTask.FromResult(result);
        }

        private void Notify()
        {
            foreach (ISwitchable<IDamageableTarget> switchable in _switchableObjects)
                switchable.Switch(_current);
        }
    }
}