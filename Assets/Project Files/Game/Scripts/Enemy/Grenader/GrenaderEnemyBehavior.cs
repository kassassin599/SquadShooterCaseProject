using System.Collections.Generic;
using UnityEngine;
using Watermelon.LevelSystem;

namespace Watermelon.SquadShooter
{
    public class GrenaderEnemyBehavior : BaseEnemyBehavior
    {
        [SerializeField] GameObject grenadePrefab;
        [SerializeField] GameObject eliteGrenadePrefab;

        [Space]
        [SerializeField] Transform grenadeStartPosition;

        [Space]
        [SerializeField] GameObject grenadeObject;

        protected override void Awake()
        {
            base.Awake();

            CanMove = true;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!LevelController.IsGameplayActive)
                return;

            healthbarBehaviour.FollowUpdate();
        }

        public override void Attack()
        {
            animatorRef.SetBool("Is Shooting", true);
        }

        public override void OnAnimatorCallback(EnemyCallbackType enemyCallbackType)
        {
            switch (enemyCallbackType)
            {
                case EnemyCallbackType.Hit:
                    var grenade = Instantiate(Tier == EnemyTier.Elite ? eliteGrenadePrefab : grenadePrefab).GetComponent<GrenadeBehavior>();

                    grenade.Throw(grenadeStartPosition.position, TargetPosition, GetCurrentDamage());

                    grenadeObject.SetActive(false);

                    break;

                case EnemyCallbackType.HitFinish:
                    animatorRef.SetBool("Is Shooting", false);
                    InvokeOnAttackFinished();
                    grenadeObject.SetActive(true);
                    break;
            }
        }
    }
}