using UnityEngine;

namespace DestructionTest
{
    public class MoveGun : MonoBehaviour
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        [SerializeField] private float _speed;

        private void Update() => Move();

        private void Move() => transform.Translate(GetDirection() * (_speed * Time.deltaTime));

        private Vector3 GetDirection()
        {
            int positionZ = 0;
            return new Vector3(Input.GetAxis(Horizontal), Input.GetAxis(Vertical), positionZ);
        }
    }
}