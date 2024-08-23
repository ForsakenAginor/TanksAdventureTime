using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;

namespace PlayerHelpers
{
    public class TargetSwitcher
    {
        private readonly List<IDamageableTarget> _targets;
        private readonly List<ISwitchable<IDamageableTarget>> _switchableObjects;
        private readonly Transform _transform;
        private readonly ImaginaryFieldOfView _fieldOfView;

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

        public void Search()
        {
            Vector3 currentPosition = _transform.position;
            IDamageableTarget resultTarget = _current;

            if (_current != null && (_current.Priority == TargetPriority.None || _fieldOfView.CanView(_current) == false))
                resultTarget = null;

            IReadOnlyCollection<IDamageableTarget> available = GetTargetsInRadius();

            if (available.Any())
                resultTarget = GetTarget(currentPosition, available);

            if (_current == resultTarget)
                return;

            _current = resultTarget;
            Notify();
        }

        private IDamageableTarget GetTarget(
            Vector3 currentPosition,
            IReadOnlyCollection<IDamageableTarget> targets)
        {
            TargetPriority priority = FindHightestPriority(targets);

            if (_current == null || _current.Priority < priority)
                return FindTarget(FindPrioritizedTargets(priority, targets), currentPosition);

            return _current;
        }

        private List<IDamageableTarget> GetTargetsInRadius()
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

            return result;
        }

        private TargetPriority FindHightestPriority(IEnumerable<IDamageableTarget> targets)
        {
            return (TargetPriority)Mathf.Max(targets.Select(target => (int)target.Priority).ToArray());
        }

        private IEnumerable<IDamageableTarget> FindPrioritizedTargets(
            TargetPriority priority,
            IEnumerable<IDamageableTarget> hightestPriorityTargets)
        {
            return hightestPriorityTargets.Where(target => target.Priority == priority);
        }

        private IDamageableTarget FindTarget(
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

            return result;
        }

        private void Notify()
        {
            foreach (ISwitchable<IDamageableTarget> switchable in _switchableObjects)
                switchable.Switch(_current);
        }
    }
}