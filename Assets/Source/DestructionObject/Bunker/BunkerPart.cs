using UnityEngine;

namespace DestructionObject
{
    public class BunkerPart : MonoBehaviour
    {
        [SerializeField] private Transform _destruction;

        private BunkerDestroyedPart[] _bunkerDestructionObjects;
        private Transform _transform;

        public bool IsDestroyed { get; private set; } = false;

        private void Awake() => Init();

        public void React()
        {
            _destruction.position = _transform.position;
            _destruction.rotation = _transform.rotation;
            _destruction.gameObject.SetActive(true);
            gameObject.SetActive(false);

            foreach (var part in _bunkerDestructionObjects)
                part.React(_transform);

            IsDestroyed = true;
        }

        private void Init()
        {
            _transform = transform;
            _bunkerDestructionObjects = new BunkerDestroyedPart[_destruction.childCount];

            for (int i = 0; i < _bunkerDestructionObjects.Length; i++)
                _bunkerDestructionObjects[i] = _destruction.GetChild(i).gameObject.AddComponent<BunkerDestroyedPart>();
        }
    }
}