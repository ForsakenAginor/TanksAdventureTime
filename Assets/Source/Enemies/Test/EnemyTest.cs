using Assets.Source.Sound.AudioMixer;
using Projectiles;
using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] private TargetTest _target;
        [SerializeField] private EnemySetup[] _enemies;
        [SerializeField] private Bomb[] _bombs;

        private SoundInitializer _soundInitializer;

        public void Init(SoundInitializer soundInitializer)
        {
            _soundInitializer = soundInitializer != null ?
                soundInitializer :
                throw new ArgumentNullException(nameof(soundInitializer));

            foreach (EnemySetup enemy in _enemies)
                enemy.Init(_target, OnAudioCreated);            

            IExplosive explosive = new Explosive(_target);

            foreach (Bomb bomb in _bombs)
                bomb.Init(explosive, OnAudioCreated); 
        }

        private void OnAudioCreated(AudioSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _soundInitializer.AddEffectSource(source);
        }
    }
}