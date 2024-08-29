using UnityEngine;

namespace DestructionObject.Buncer
{
    public class BunkerPart : MonoBehaviour
    {
        [SerializeField] private Transform _destruction;

        private DestroyedPartBunker[] _bunkerDestructionObjects;
        private Transform _transform;

        public bool IsDestroyed { get; private set; } = false;

        private void Awake() => Init();

        public void React()
        {
            _destruction.position = _transform.position;
            _destruction.rotation = _transform.rotation;
            _destruction.gameObject.SetActive(true);
            gameObject.SetActive(false);

            for (int i = 0; i < _bunkerDestructionObjects.Length; i++)
                _bunkerDestructionObjects[i].React(_transform);

            IsDestroyed = true;
        }

        private void Init()
        {
            _transform = transform;
            _bunkerDestructionObjects = new DestroyedPartBunker[_destruction.childCount];

            for (int i = 0; i < _bunkerDestructionObjects.Length; i++)
                _bunkerDestructionObjects[i] = _destruction.GetChild(i).gameObject.AddComponent<DestroyedPartBunker>();
        }
    }
}