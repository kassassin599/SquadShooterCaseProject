using UnityEngine;

namespace Watermelon.Upgrades
{
    [CreateAssetMenu(fileName = "Upgrades Database", menuName = "Data/Upgrades/Upgrades Database")]
    public class UpgradesDatabase : ScriptableObject
    {
        [SerializeField] BaseUpgrade[] upgrades;
        public BaseUpgrade[] Upgrades => upgrades;
    }
}