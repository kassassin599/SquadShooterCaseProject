using UnityEngine;

namespace Watermelon.SquadShooter
{
    public class EnemyBulletBehavior : MonoBehaviour
    {
        private static readonly int PARTICLE_HIT_HASH = "Shotgun Hit".GetHashCode();
        private static readonly int PARTICLE_WALL_HIT_HASH = "Shotgun Wall Hit".GetHashCode();

        protected float damage;
        protected float speed;

        protected float selfDestroyDistance;
        protected float distanceTraveled = 0;

        protected TweenCase disableTweenCase;

        public virtual void Initialise(float damage, float speed, float selfDestroyDistance)
        {
            this.damage = damage;
            this.speed = speed;

            this.selfDestroyDistance = selfDestroyDistance;
            distanceTraveled = 0;

            gameObject.SetActive(true);
        }

        protected virtual void FixedUpdate()
        {
            transform.position += transform.forward * speed * Time.fixedDeltaTime;

            if (selfDestroyDistance != -1)
            {
                distanceTraveled += speed * Time.fixedDeltaTime;

                if (distanceTraveled >= selfDestroyDistance)
                {
                    SelfDestroy();
                }
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == PhysicsHelper.LAYER_PLAYER)
            {
                CharacterBehaviour characterBehaviour = other.GetComponent<CharacterBehaviour>();
                if (characterBehaviour != null)
                {
                    // Deal damage to enemy
                    if (characterBehaviour.TakeDamage(damage))
                    {
                        ParticlesController.PlayParticle(PARTICLE_HIT_HASH).SetPosition(transform.position);
                    }

                    SelfDestroy();
                }
            }
            else if (other.gameObject.layer == PhysicsHelper.LAYER_OBSTACLE)
            {
                SelfDestroy();

                ParticlesController.PlayParticle(PARTICLE_WALL_HIT_HASH).SetPosition(transform.position);
            }
        }

        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}