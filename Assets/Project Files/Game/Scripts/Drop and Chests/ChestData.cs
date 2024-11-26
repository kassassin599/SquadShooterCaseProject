using UnityEngine;

namespace Watermelon.LevelSystem
{
    [System.Serializable]
    public class ChestData
    {
        [SerializeField] GameObject prefab;
        public GameObject Prefab => prefab;

        [SerializeField] LevelChestType type;
        public LevelChestType Type => type;

        private Pool pool;
        public Pool Pool => pool;

        public void Initialise()
        {
            pool = new Pool(prefab, $"Chest_{prefab.name}");
        }

        public void Unload()
        {
            pool?.Destroy();
        }
    }
}