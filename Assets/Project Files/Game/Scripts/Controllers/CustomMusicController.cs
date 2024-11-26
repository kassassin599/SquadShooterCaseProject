using UnityEngine;

namespace Watermelon
{
    public class CustomMusicController : MonoBehaviour
    {
        private static CustomMusicController instance;

        [SerializeField] MusicSource menuMusicSource;
        [SerializeField] MusicSource gameMusicSource;

        private AudioClip defaultGameMusic;

        public void Init()
        {
            instance = this;

            menuMusicSource.Init();
            gameMusicSource.Init();

            defaultGameMusic = gameMusicSource.AudioSource.clip;
        }

        public static void PlayMenuMusic()
        {
            if (instance == null) return;

            instance.menuMusicSource.Activate();
        }

        public static void PlayGameMusic(AudioClip audioClip)
        {
            if (instance == null) return;

            if (audioClip == null)
                audioClip = instance.defaultGameMusic;

            instance.gameMusicSource.AudioSource.clip = audioClip;
            instance.gameMusicSource.Activate();
        }
    }
}

// -----------------
// Audio Controller v 0.3.3
// -----------------

// Changelog
// v 0.3.2
// • Added audio listener creation method
// v 0.3.2
// • Added volume float
// • AudioSettings variable removed (now sounds, music and vibrations can be reached directly)
// v 0.3.1
// • Added OnVolumeChanged callback
// • Renamed AudioSettings to Settings
// v 0.3
// • Added IsAudioModuleEnabled method
// • Added IsVibrationModuleEnabled method
// • Removed VibrationToggleButton class
// v 0.2
// • Removed MODULE_VIBRATION
// v 0.1
// • Added basic version
// • Added support of new initialization
// • Music and Sound volume is combined