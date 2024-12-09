using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace Archiventure
{
    public class GameManager : MonoBehaviour
    {
        [Header("Arrays")]
        public GameObject[] structureArray;
        [HideInInspector]
        public SaveData save;

        [Header("Scripts")]
        public CameraController2D cameraController;

        [Header("Game Objects")]
        public GameObject mainPanel;
        public GameObject shop;
        public GameObject replaceBuidlingsPanel;
        public GameObject destroyBuidlingsPanel;
        public GameObject spawnPointOfBuildings;
        [HideInInspector]
        public GameObject selectedBuilding;

        //Variables
        [HideInInspector]
        public bool buildingSelected;
        [HideInInspector]
        public int buildingNumber;
        [HideInInspector]
        public bool replaceMode;
        [HideInInspector]
        public bool destroyMode;

        [Header("Shopping tip")]
        [SerializeField] private TextMeshProUGUI tipText;      
        private float tipShowTime = 1.5f;           
        private float tipTimer;                     

        void Start()
        {
            mainPanel.SetActive(true);
            shop.SetActive(false);
            replaceBuidlingsPanel.SetActive(false);
            destroyBuidlingsPanel.SetActive(false);
            replaceMode = false;
            destroyMode = false;
            OnLoad();
            //OpenShop();
        }

        void Update()
        {
            if (buildingSelected)
            {
                cameraController.movement = false;
            }
            else
            {
                cameraController.movement = true;
            }

            // Set up the tip text
            if (tipText.gameObject.activeSelf)
            {
                tipTimer -= Time.deltaTime;
                if (tipTimer <= 0)
                {
                    tipText.gameObject.SetActive(false);
                }
            }

            // Check the achievements
            if (AchievementManager.Instance != null)
            {
                AchievementManager.Instance.CheckAchievements(this);
            }
        }

        //Buttons

        public void OpenShop()
        {
            mainPanel.SetActive(false);
            shop.SetActive(true);
            buildingSelected = true;
        }

        //This button close everything, so I used it for every Panel
        public void CloseButton()
        {
            mainPanel.SetActive(true);
            shop.SetActive(false);
            replaceBuidlingsPanel.SetActive(false);
            destroyBuidlingsPanel.SetActive(false);
            buildingSelected = false;
            replaceMode = false;
            destroyMode = false;
            selectedBuilding = null;
        }

        public void BuyBuilding(int number)
        {
            buildingNumber = number;

            // Get building prefab
            GameObject buildingPrefab = structureArray[buildingNumber];
            Building script = buildingPrefab.GetComponent<Building>();

            // Check if the user has enough gold
            if (ResourceManager.Instance.gold < script.buildCost)
            {
                // Update the position of tip text
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                tipText.transform.position = new Vector2(mousePosition.x, mousePosition.y);

                tipText.gameObject.SetActive(true);
                tipTimer = tipShowTime;

                return;
            }

            cameraController.movement = false;
            shop.SetActive(false);
            GameObject house = Instantiate(structureArray[buildingNumber], spawnPointOfBuildings.transform.position, Quaternion.identity);
            selectedBuilding = house;
            Building buildingScript = house.GetComponent<Building>();
            buildingScript.buildingState = Building.BuildingState.firstStart;
            buildingScript.mouseDrag = true;
            buildingSelected = true;
        }

        public void ReplaceBuildingsMode()
        {
            replaceMode = true;
            mainPanel.SetActive(false);
            shop.SetActive(false);
            replaceBuidlingsPanel.SetActive(true);
            if (selectedBuilding != null)
            {
                Building replacableBuilding = selectedBuilding.GetComponent<Building>();
                replacableBuilding.ResetCanvas();
                replacableBuilding.buildingState = Building.BuildingState.replacing;
            }
        }

        public void DestroyBuildingsMode()
        {
            destroyMode = true;
            mainPanel.SetActive(false);
            shop.SetActive(false);
            destroyBuidlingsPanel.SetActive(true);
            if (selectedBuilding != null)
            {
                Building destroyableBuilding = selectedBuilding.GetComponent<Building>();
                destroyableBuilding.ResetCanvas();
                destroyableBuilding.buildingState = Building.BuildingState.destroying;
            }
        }


        private void TurnOffSpritesChanging()
        {
        }

        //Save system

        public void OnSave()
        {
            //SerializationManager.Save(save);
            //save.resources = ResourceManager.Instance.GetResourceData();
            
            save.gold = ResourceManager.Instance.gold;
            save.population = ResourceManager.Instance.population;

            
            SerializationManager.Save(save);
            CloudSaveManager.Instance.SaveToCloud(save);

        }

        public void OnLoad()
        {
            //save = SerializationManager.Load();
            ////if (save.resources != null)
            ////{
            ////    ResourceManager.Instance.LoadResourceData(save.resources);
            ////}
            //ResourceManager.Instance.gold = save.gold;
            //ResourceManager.Instance.population = save.population;

            //for (int i = 0; i < save.buildings.Count; i++)
            //{
            //    BuildingData currentBuilding = save.buildings[i];
            //    GameObject obj = Instantiate(structureArray[(int)currentBuilding.buildingType]);

            //    Building structure = obj.GetComponent<Building>();
            //    structure.nextUpdate = currentBuilding.level;

            //    obj.transform.position = currentBuilding.position;
            //    obj.transform.rotation = currentBuilding.rotation;
            //}

            CloudSaveManager.Instance.LoadFromCloud(cloudSave =>
            {
                save = cloudSave;

                // Save to local as well
                SerializationManager.Save(save);

                for (int i = 0; i < save.buildings.Count; i++)
                {
                    BuildingData currentBuilding = save.buildings[i];
                    GameObject obj = Instantiate(structureArray[(int)currentBuilding.buildingType]);

                    Building structure = obj.GetComponent<Building>();
                    structure.nextUpdate = currentBuilding.level;

                    obj.transform.position = currentBuilding.position;
                    obj.transform.rotation = currentBuilding.rotation;
                }

                ResourceManager.Instance.gold = save.gold;
                ResourceManager.Instance.population = save.population;
            });
        }

        #if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            OnSave();  // quit and save
        }
        #endif
    }
}
