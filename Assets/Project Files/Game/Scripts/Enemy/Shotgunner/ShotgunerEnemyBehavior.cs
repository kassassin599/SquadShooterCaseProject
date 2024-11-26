using UnityEngine;

namespace Watermelon.SquadShooter
{
    public class ShotgunerEnemyBehavior : BaseEnemyBehavior
    {
        [Space]
        [SerializeField] Transform shootPoint;
        [SerializeField] ParticleSystem shootParticle;
        [SerializeField] ParticleSystem reloadParticle;

        [Space]
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] float bulletSpeed;
        [SerializeField] float spreadAngle;
        [SerializeField] DuoInt bulletsCount;

        public override void Attack()
        {
            animatorRef.SetTrigger("Shoot");
        }

        public override void OnAnimatorCallback(EnemyCallbackType enemyCallbackType)
        {
            switch (enemyCallbackType)
            {
                case EnemyCallbackType.Hit:
                    int bullets = bulletsCount.Random();
                    for (int i = 0; i < bullets; i++)
                    {
                        var bullet = Instantiate(bulletPrefab).SetPosition(shootPoint.position).SetEulerAngles(shootPoint.eulerAngles).GetComponent<EnemyBulletBehavior>();
                        bullet.transform.LookAt(target.position.SetY(shootPoint.position.y));
                        bullet.Initialise(GetCurrentDamage(), bulletSpeed, Stats.AttackDistance + 10f);
                        bullet.transform.Rotate(new Vector3(0f, i == 0 ? 0f : (Random.Range(spreadAngle * 0.25f, spreadAngle * 0.5f) * (Random.Range(0, 2) == 0 ? -1f : 1f)), 0f));
                    }

                    shootParticle.Play();
                    AudioController.PlaySound(AudioController.AudioClips.shotShotgun);

                    break;
                case EnemyCallbackType.HitFinish:
                    InvokeOnAttackFinished();
                    break;

                case EnemyCallbackType.ReloadFinished:
                    reloadParticle.Play();
                    break;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            healthbarBehaviour.FollowUpdate();
        }
    }
}