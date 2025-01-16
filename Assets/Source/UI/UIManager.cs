using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Marker))]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UserInterfaceElement _losingPanel;
        [SerializeField] private UserInterfaceElement _winingPanel;
        [SerializeField] private UserInterfaceElement _mobileInputCanvas;
        [SerializeField] private UserInterfaceElement _buttonsPanel;
        [SerializeField] private float _delay;
        [SerializeField] private LevelLabel _levelLabel;
        [SerializeField] private UserInterfaceElement _helperAttentionPanel;

        [Header("Marker")] [SerializeField]
        private float _minDistance;

        [SerializeField] private RectTransform _markerImage;
        private Marker _marker;

        private void Awake()
        {
            _marker = GetComponent<Marker>();
        }

#if !UNITY_EDITOR
        private void Start()
        {
            bool isWebGLOnMobile = Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer;

            if (isWebGLOnMobile == false)
                _mobileInputCanvas.Disable();
        }
#endif

        public void Init(IEnumerable<ITarget> enemies, Transform player, int levelNumber)
        {
            if (enemies == null)
                throw new ArgumentNullException(nameof(enemies));

            if (player == null)
                throw new ArgumentNullException(nameof(player));

            if (levelNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(levelNumber));

            _marker.Init(enemies, player, _minDistance, _markerImage);
            _levelLabel.Init(levelNumber);
        }

        public void ShowLosingPanel()
        {
            _buttonsPanel.Disable();
            _mobileInputCanvas.Disable();
            StartCoroutine(DisplayLosingPanel());
        }

        public void ShowWiningPanel()
        {
            _buttonsPanel.Disable();
            _mobileInputCanvas.Disable();
            _winingPanel.Enable();
        }

        public void ShowHelperAttentionPanel()
        {
            _helperAttentionPanel.Enable();
        }

        private IEnumerator DisplayLosingPanel()
        {
            WaitForSeconds delay = new (_delay);

            yield return delay;

            _losingPanel.Enable();
        }
    }
}