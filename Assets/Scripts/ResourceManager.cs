using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
        public Text goldText;       
        public Text populationText; 

        [Header("Income Settings")]
        public float baseGoldIncomePerSecond = 1f; // base income

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
            // Increase gold by time
            UpdateResources();
            UpdateUI();
        }

        private void UpdateResources()
        {
            gold += baseGoldIncomePerSecond * Time.deltaTime;
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