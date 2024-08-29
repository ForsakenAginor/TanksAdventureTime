using Cinemachine;
using UnityEngine;

namespace UI
{
    public class CinemachineModifier : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.02f;

        private void Start()
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }

        private float GetAxisCustom(string inputAxis)
        {
            const string MouseXInput = "Mouse X";

            if (inputAxis == MouseXInput)
                return _rotationSpeed;

            return 0;
        }
    }
}