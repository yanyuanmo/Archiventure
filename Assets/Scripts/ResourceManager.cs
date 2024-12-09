using UnityEngine;
using UnityEngine.UI;

namespace Archiventure
{
    public class ResourceManager : MonoBehaviour
    {
        [Header("Resources")]
        public float gold; 
        public int population;  

        [Header("UI Elements")]
        [SerializeField] public Text goldText;
        [SerializeField] public Text populationText; 


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
    }
}