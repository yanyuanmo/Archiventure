using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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
        public ResourceManager resourceManager;
        public AchievementManager achievementManager;

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

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

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

        public void CheckAchievements()
        {
            if (achievementManager.notificationPanel == null) achievementManager.Init();

            // Check first building achievement
            if (!achievementManager.achievementsList[0].isUnlocked && save.buildings.Count > 0)
            {
                achievementManager.UnlockAchievement("FIRST_BUILDING");
            }

            // Check city expansion achievement
            if (!achievementManager.achievementsList[1].isUnlocked && save.buildings.Count >= 10)
            {
                achievementManager.UnlockAchievement("CITY_EXPANSION");
            }

            // Check population growth achievement
            if (!achievementManager.achievementsList[2].isUnlocked && resourceManager.population >= 100)
            {
                achievementManager.UnlockAchievement("POPULATION_GROWTH");
            }

            // Check wealth accumulation achievement
            if (!achievementManager.achievementsList[3].isUnlocked && resourceManager.gold >= 50000)
            {
                achievementManager.UnlockAchievement("WEALTH_ACCUMULATION");
            }
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
            if (achievementManager != null)
            {
                CheckAchievements();
            }
        }

        //Buttons

        public void OpenShop()
        {
            mainPanel.SetActive(false);
            shop.SetActive(true);
            buildingSelected = true;
        }

        public void CloseAchivementButton()
        {
            resourceManager.AddGold(achievementManager.lastUnlockedAchievement.goldReward);
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
            if (resourceManager.gold < script.buildCost)
            {
                // Update the position of tip text
                //Vector3 mousePosition = Mouse.current.position.ReadValue();
                //if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
                //{
                //    mousePosition = Touchscreen.current.primaryTouch.position.ReadValue();
                //}

                //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                //tipText.transform.position = new Vector2(mousePosition.x, mousePosition.y);

                //tipText.gameObject.SetActive(true);
                //tipTimer = tipShowTime;

                //return;

                Vector3 mousePosition;

                // 检查是否有触摸输入
                if (Touch.activeTouches.Count > 0)
                {
                    // 使用第一个触摸点的位置
                    mousePosition = Touch.activeTouches[0].screenPosition;
                }
                else
                {
                    // 使用鼠标位置
                    mousePosition = Mouse.current.position.ReadValue();
                }

                // 转换屏幕坐标到世界坐标
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                tipText.transform.position = new Vector2(worldPosition.x, worldPosition.y);

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
            
            save.gold = resourceManager.gold;
            save.population = resourceManager.population;

            
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

                resourceManager.gold = save.gold;
                resourceManager.population = save.population;
            });
        }

        public void ReturnToCultureSelection()
        {
            OnSave();

            // Wait a short moment to ensure save is complete
            StartCoroutine(LoadCultureSelectionSceneWithDelay());
        }

        private System.Collections.IEnumerator LoadCultureSelectionSceneWithDelay()
        {
            // Add a small delay to ensure save is completed
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("SelectCultureScene");
        }

#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            OnSave();  // quit and save
        }
#endif
    }
}
