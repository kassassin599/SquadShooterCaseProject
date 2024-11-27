using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon.SquadShooter;
using Watermelon.Upgrades;
using Watermelon;

public class SniperGunBehavior : BaseGunBehavior

{
  [LineSpacer]
  [SerializeField] LayerMask targetLayers = (1 << 9) | (1 << 8);
  [SerializeField] float bulletDisableTime = 5.0f;

  // Gun variables
  private float spread;
  private float attackDelay;
  private DuoFloat bulletSpeed;

  // Shooting cooldown
  private float nextShootTime;

  // Bullet pool (based on prefab from Upgrade)
  private Pool bulletPool;

  // Gun upgrade
  private SniperGunUpgrade upgrade;

  public override void Initialise(CharacterBehaviour characterBehaviour, WeaponData data)
  {
    base.Initialise(characterBehaviour, data);

    // Get upgrade from database
    upgrade = UpgradesController.GetUpgrade<SniperGunUpgrade>(data.UpgradeType);

    // Get weapon's current upgrade stage
    BaseWeaponUpgradeStage currentStage = upgrade.CurrentStage as BaseWeaponUpgradeStage;

    GameObject bulletObj = (upgrade.CurrentStage as BaseWeaponUpgradeStage).BulletPrefab;

    // Create pool object from bullet prefab
    bulletPool = new Pool(bulletObj, bulletObj.name);

    // Recalculate gun variables
    RecalculateDamage();
  }

  public override void RecalculateDamage()
  {
    // Get weapon's current upgrade stage
    BaseWeaponUpgradeStage stage = upgrade.GetCurrentStage();

    damage = stage.Damage;
    attackDelay = 1f / stage.FireRate;
    spread = stage.Spread;
    bulletSpeed = stage.BulletSpeed;
  }

  public override void GunUpdate()
  {
    // Check if any enemy is in shooting range
    if (!characterBehaviour.IsCloseEnemyFound) return;

    // Check if shooting cooldown is finished
    if (nextShootTime >= Time.timeSinceLevelLoad) return;

    // Get direction of the closest enemy
    Vector3 shootDirection = characterBehaviour.ClosestEnemyBehaviour.transform.position.SetY(shootPoint.position.y) - shootPoint.position;

    // Check with raycast if the target is physically reachable and layer of the target is "Enemy"
    if (Physics.Raycast(transform.position, shootDirection, out var hitInfo, 300f, targetLayers) && hitInfo.collider.gameObject.layer == PhysicsHelper.LAYER_ENEMY)
    {
      // Check if a character is looking in the target's direction
      if (Vector3.Angle(shootDirection, transform.forward.SetY(0f)) < 40f)
      {
        // Activate the highlight circle under the target 
        characterBehaviour.SetTargetActive();

        // Set next shooting cooldown
        nextShootTime = Time.timeSinceLevelLoad + attackDelay;

        // Get bullet object from the pool
        GameObject bulletObject = bulletPool.GetPooledObject();
        bulletObject.transform.position = shootPoint.position;
        bulletObject.transform.eulerAngles = characterBehaviour.transform.eulerAngles + Vector3.up * Random.Range((float)-spread, spread);

        // Get bullet component and initialise its logic
        PlayerBulletBehavior bullet = bulletObject.GetComponent<PlayerBulletBehavior>();
        bullet.Initialise(damage.Random() * characterBehaviour.Stats.BulletDamageMultiplier, bulletSpeed.Random(), characterBehaviour.ClosestEnemyBehaviour, bulletDisableTime);

        // Invoke the OnGunShooted method to let the character know when to play shooting animation 
        characterBehaviour.OnGunShooted();

        // Here you can add extra code such as particles, sounds, etc..
        // AudioController.PlaySound(audioClipVariable);
        // particleVariable.Play();
      }
    }
    else
    {
      characterBehaviour.SetTargetUnreachable();
    }
  }

  /// <summary>
  /// This method is called when the player unselects the gun.
  /// Use it to reset variables before the prefab is destroyed.
  /// </summary>
  public override void OnGunUnloaded()
  {
    // Destroy bullets pool
    if (bulletPool != null)
    {
      bulletPool.Clear();
      bulletPool = null;
    }
  }

  /// <summary>
  /// This method is called when player selects gun.
  /// Here you should change parent of the gun prefab and place it to correct position.
  /// For default guns we added Transforms to BaseCharacterGraphics script to easily modify position of gun in the editor.
  /// </summary>
  public override void PlaceGun(BaseCharacterGraphics characterGraphics)
  {
    // Use transform variable inside of BaseCharacterGraphics script
    transform.SetParent(characterGraphics.SniperHolderTransform);
    transform.ResetLocal();

    // OR
    // Use parent character object and just apply offset to the gun
    // transform.SetParent(characterBehaviour.transform);
    // transform.localPosition = new Vector3(1.36f, 4.67f, 2.5f);
  }

  /// <summary>
  /// This method is called when the character enters a room or revives.
  /// Here you can return the gun to the default state or reset some variables.
  /// </summary>
  public override void Reload()
  {
    bulletPool.ReturnToPoolEverything();
  }
}
