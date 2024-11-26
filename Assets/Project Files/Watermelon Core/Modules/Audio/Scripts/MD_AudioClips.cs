using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Audio Clips", menuName = "Data/Core/Audio Clips")]
    public class AudioClips : ScriptableObject
    {
        [BoxGroup("Character", "Character")]
        public AudioClip[] characterHit;

        [BoxGroup("Character")]
        public AudioClip shotMinigun;
        [BoxGroup("Character")]
        public AudioClip shotShotgun;
        [BoxGroup("Character")]
        public AudioClip shotLavagun;
        [BoxGroup("Character")]
        public AudioClip shotTesla;

        [BoxGroup("Enemy", "Enemy")]
        public AudioClip[] enemyScreems;
        [BoxGroup("Enemy")]
        public AudioClip enemyMeleeHit;
        [BoxGroup("Enemy")]
        public AudioClip enemyShot;
        [BoxGroup("Enemy")]
        public AudioClip enemySniperShoot;

        [BoxGroup("Boss", "Boss")]
        public AudioClip jumpLanding;
        [BoxGroup("Boss")]
        public AudioClip bossScream;
        [BoxGroup("Boss")]
        public AudioClip punch1;
        [BoxGroup("Boss")]
        public AudioClip shoot2;
        [BoxGroup("Boss")]
        public AudioClip explode;

        [BoxGroup("Other", "Other")]
        public AudioClip door;
        [BoxGroup("Other")]
        public AudioClip complete;
        [BoxGroup("Other")]
        public AudioClip chestOpen;
        [BoxGroup("Other")]
        public AudioClip coinAppear;
        [BoxGroup("Other")]
        public AudioClip coinPickUp;
        [BoxGroup("Other")]
        public AudioClip cardPickUp;

        [BoxGroup("UI", "UI")]
        public AudioClip buttonSound;
        [BoxGroup("UI")]
        public AudioClip upgrade;
    }
}

// -----------------
// Audio Controller v 0.4
// -----------------