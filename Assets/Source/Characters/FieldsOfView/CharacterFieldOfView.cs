using UnityEngine;

namespace Characters
{
    public class CharacterFieldOfView : IFieldOfView, ISwitchable<ITarget>
    {
        public CharacterFieldOfView(ITarget target, Transform viewPoint, float attackRadius)
        {
            Target = target;
            ViewPoint = viewPoint;
            AttackRadius = attackRadius;
        }

        public ITarget Target { get; private set; }

        public Transform ViewPoint { get; }

        public float AttackRadius { get; }

        public virtual bool IsBlockingByWall()
        {
            return false;
        }

        public virtual bool CanView()
        {
            return IsPlayerInRadius();
        }

        public bool IsPlayerInRadius()
        {
            return HaveTarget() && Vector3.Distance(ViewPoint.position, Target.Position) <= AttackRadius;
        }

        public void Switch(ITarget target)
        {
            Target = target;
        }

        private bool HaveTarget()
        {
            return Target != null;
        }
    }
}