using System.Collections.Generic;
using UnityEngine;
using Watermelon.Upgrades;

namespace Watermelon.SquadShooter
{
    public class MiniGunBehavior : BaseGunBehavior
    {
        [LineSpacer]
        [SerializeField] Transform barrelTransform;
        [SerializeField] ParticleSystem shootParticleSystem;

        [SerializeField] LayerMask targetLayers;
        [SerializeField] float bulletDisableTime;

        [Space]
        [SerializeField] float fireRotationSpeed;

        [Space]
        [SerializeField] List<float> bulletStreamAngles;

        private float spread;
        private float attackDelay;
        private DuoFloat bulletSpeed;

        private float nextShootTime;
        private float lastShootTime;

        private Pool bulletPool;

        private Vector3 shootDirection;

        private MinigunUpgrade upgrade;

        private TweenCase shootTweenCase;

        public override void Initialise(CharacterBehaviour characterBehaviour, WeaponData data)
        {
            base.Initialise(characterBehaviour, data);

            upgrade = UpgradesController.GetUpgrade<MinigunUpgrade>(data.UpgradeType);

            GameObject bulletObj = (upgrade.CurrentStage as BaseWeaponUpgradeStage).BulletPrefab;
            bulletPool = new Pool(bulletObj, $"Minigun_{bulletObj.name}");

            RecalculateDamage();
        }

        private void OnDestroy()
        {
            if (bulletPool != null)
                PoolManager.DestroyPool(bulletPool);
        }

        public override void OnLevelLoaded()
        {
            RecalculateDamage();
        }

        public override void RecalculateDamage()
        {
            var stage = upgrade.GetCurrentStage();

            damage = stage.Damage;
            attackDelay = 1f / stage.FireRate;
            spread = stage.Spread;
            bulletSpeed = stage.BulletSpeed;
        }

        public override void GunUpdate()
        {
            if(attackDelay > 0.2f)
                AttackButtonBehavior.SetReloadFill(1 - (Time.timeSinceLevelLoad - lastShootTime) / (nextShootTime - lastShootTime));

            if (!characterBehaviour.IsCloseEnemyFound)
                return;

            barrelTransform.Rotate(Vector3.forward * fireRotationSpeed);

            if (nextShootTime >= Time.timeSinceLevelLoad || !characterBehaviour.IsAttackingAllowed) 
            {
                return;
            }

            AttackButtonBehavior.SetReloadFill(0);

            shootDirection = characterBehaviour.ClosestEnemyBehaviour.transform.position.SetY(shootPoint.position.y) - shootPoint.position;

            if (Physics.Raycast(transform.position, shootDirection, out var hitInfo, 300f, targetLayers) && hitInfo.collider.gameObject.layer == PhysicsHelper.LAYER_ENEMY)
            {
                if (Vector3.Angle(shootDirection, transform.forward.SetY(0f)) < 40f)
                {
                    shootTweenCase.KillActive();

                    shootTweenCase = transform.DOLocalMoveZ(-0.0825f, attackDelay * 0.3f).OnComplete(delegate
                    {
                        shootTweenCase = transform.DOLocalMoveZ(0, attackDelay * 0.6f);
                    });

                    characterBehaviour.SetTargetActive();

                    shootParticleSystem.Play();

                    nextShootTime = Time.timeSinceLevelLoad + attackDelay;
                    lastShootTime = Time.timeSinceLevelLoad;

                    if (bulletStreamAngles.IsNullOrEmpty())
                    {
                        bulletStreamAngles = new List<float> { 0 };
                    }

                    int bulletsNumber = upgrade.GetCurrentStage().BulletsPerShot.Random();

                    for (int k = 0; k < bulletsNumber; k++)
                    {
                        for (int i = 0; i < bulletStreamAngles.Count; i++)
                        {
                            var streamAngle = bulletStreamAngles[i];

                            PlayerBulletBehavior bullet = bulletPool
                                .GetPooledObject()
                                .SetPosition(shootPoint.position)
                                .SetEulerAngles(characterBehaviour.transform.eulerAngles + Vector3.up * (Random.Range((float)-spread, spread) + streamAngle))
                                .GetComponent<PlayerBulletBehavior>();
                            bullet.Initialise(damage.Random() * characterBehaviour.Stats.BulletDamageMultiplier, bulletSpeed.Random(), characterBehaviour.ClosestEnemyBehaviour, bulletDisableTime);
                        }
                    }

                    characterBehaviour.OnGunShooted();

                    AudioController.PlaySound(AudioController.AudioClips.shotMinigun);
                }
            }
            else
            {
                characterBehaviour.SetTargetUnreachable();
            }
        }

        public override void OnGunUnloaded()
        {
            if (bulletPool != null)
            {
                PoolManager.DestroyPool(bulletPool);

                bulletPool = null;
            }
        }

        public override void PlaceGun(BaseCharacterGraphics characterGraphics)
        {
            transform.SetParent(characterGraphics.MinigunHolderTransform);
            transform.ResetLocal();
        }

        public override void Reload()
        {
            bulletPool.ReturnToPoolEverything();
        }
    }
}