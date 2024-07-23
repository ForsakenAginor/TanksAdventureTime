using System.Collections.Generic;
using PlayerHelpers;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class EnemyTest : MonoBehaviour // Используется только в EnemyTesting сцене
    {
        private readonly List<IDamageableTarget> _targets = new ();

        [SerializeField] private TargetTest _target;
        [SerializeField] private EnemySetup[] _enemies;
        [SerializeField] private Bomb[] _bombs;
        [SerializeField] private PlayerHelperSetup _helper;

        private void Awake()
        {
            foreach (EnemySetup enemy in _enemies)
                enemy.Init(_target, OnAudioCreated, _targets.Add);

            IExplosive explosive = new Explosive(_target);

            foreach (Bomb bomb in _bombs)
                bomb.Init(explosive, OnAudioCreated);

            _helper.Init(
                _targets,
                PlayerHelperTypes.Grenade,
                OnAudioCreated,
                activationHandler =>
                {
                    Debug.Log(nameof(activationHandler.onEnable));
                    Debug.Log(nameof(activationHandler.onDisable));
                });
        }

        private void OnAudioCreated(AudioSource source)
        {
        }
    }
}