using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class CurrencyData
    {
        [SerializeField] GameObject model;
        public GameObject Model => model;

        [SerializeField] bool displayAlways = false;
        public bool DisplayAlways => displayAlways;

        private Pool pool;
        public Pool Pool => pool;

        public void Init(Currency currency)
        {
            pool = new Pool(model, $"Currency_{currency.CurrencyType}");
        }
    }
}