using UnityEngine;
using Watermelon.Upgrades;

[CreateAssetMenu(fileName = "Sniper Gun Upgrade", menuName = "Data/Upgrades/Sniper Gun Upgrade")]
public class SniperGunUpgrade : BaseWeaponUpgrade
{
  // You can remove this variable.
  // It is placed here only to show you where to find it in the editor.
  [SerializeField] GameObject specialGameObject;
  public GameObject SpecialGameObject => specialGameObject;

  public override void Initialise()
  {

  }
}