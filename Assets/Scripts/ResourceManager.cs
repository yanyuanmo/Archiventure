using UnityEngine;
using UnityEngine.UI;

namespace Archiventure
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        [System.Serializable]
        public class ResourceData
        {
            public float gold;
            public int population;
        }

        [Header("Resources")]
        public float gold; 
        public int population;  

        [Header("UI Elements")]
        //[SerializeField] private GameObject resourceCanvas;
        [SerializeField] public Text goldText;
        [SerializeField] public Text populationText; 


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (goldText != null)
                goldText.text = $"{Mathf.Floor(gold)}";
            if (populationText != null)
                populationText.text = $"{population}";
        }

        public bool SpendGold(float amount)
        {
            if (gold >= amount)
            {
                gold -= amount;
                return true;
            }
            return false;
        }

        public void AddGold(float amount)
        {
            gold += amount;
        }

        public void AddPopulation(int amount)
        {
            population += amount;
        }

        public ResourceData GetResourceData()
        {
            return new ResourceData
            {
                gold = this.gold,
                population = this.population
            };
        }

        public void LoadResourceData(ResourceData data)
        {
            this.gold = data.gold;
            this.population = data.population;
        }
    }
}