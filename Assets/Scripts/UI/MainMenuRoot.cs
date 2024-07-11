using Assets.Source.Sound.AudioMixer;
using UnityEngine;

public class MainMenuRoot : MonoBehaviour
{
    [SerializeField] private SoundInitializer _soundInitializer;

    private void Start()
    {
        _soundInitializer.Init();
    }
}
