using UnityEngine;

namespace Watermelon.SquadShooter
{
    public class CowboyEnemyBehavior : BaseEnemyBehavior
    {
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] float bulletSpeed;

        [Header("Left side")]
        [SerializeField] Transform leftShootPoint;
        [SerializeField] ParticleSystem leftGunFireParticle;

        [Header("Right side")]
        [SerializeField] Transform rightShootPoint;
        [SerializeField] ParticleSystem rightGunFireParticle;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            healthbarBehaviour.FollowUpdate();
        }

        public override void Attack()
        {
            animatorRef.SetTrigger("Shoot");
        }

        public override void OnAnimatorCallback(EnemyCallbackType enemyCallbackType)
        {
            EnemyBulletBehavior bullet;

            switch (enemyCallbackType)
            {
                case EnemyCallbackType.LeftHit:
                    bullet = Instantiate(bulletPrefab).SetPosition(leftShootPoint.position).SetEulerAngles(leftShootPoint.eulerAngles).GetComponent<EnemyBulletBehavior>();
                    bullet.transform.LookAt(target.position.SetY(leftShootPoint.position.y));
                    bullet.Initialise(GetCurrentDamage(), bulletSpeed, 200);

                    leftGunFireParticle.Play();
                    AudioController.PlaySound(AudioController.AudioClips.enemyShot);

                    break;
                case EnemyCallbackType.RightHit:
                    bullet = Instantiate(bulletPrefab).SetPosition(rightShootPoint.position).SetEulerAngles(rightShootPoint.eulerAngles).GetComponent<EnemyBulletBehavior>();
                    bullet.transform.LookAt(target.position.SetY(rightShootPoint.position.y));
                    bullet.Initialise(GetCurrentDamage(), bulletSpeed, 200);

                    rightGunFireParticle.Play();
                    AudioController.PlaySound(AudioController.AudioClips.enemyShot);

                    break;

                case EnemyCallbackType.HitFinish:
                    InvokeOnAttackFinished();
                    break;
            }
        }
    }
}