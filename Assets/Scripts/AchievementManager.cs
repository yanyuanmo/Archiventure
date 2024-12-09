using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Archiventure
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }

        [System.Serializable]
        public class AchievementData
        {
            public string id;
            public string displayName;
            public float goldReward;
            public bool isUnlocked;
        }

        [SerializeField]
        private List<AchievementData> achievementsList = new List<AchievementData>
        {
            new AchievementData { id = "FIRST_BUILDING", displayName = "First Building", goldReward = 100f },
            new AchievementData { id = "CITY_EXPANSION", displayName = "City Expansion", goldReward = 500f },
            new AchievementData { id = "POPULATION_GROWTH", displayName = "Population Growth", goldReward = 1000f },
            new AchievementData { id = "WEALTH_ACCUMULATION", displayName = "Wealth Accumulation", goldReward = 2000f }
        };

        // private AchievementNotification notificationPanel;
        [Header("Game Objects")]
        public GameObject notificationPanel;

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

        private void Start()
        {
            LoadAchievements();
        }

        public void Init()
        {
            //if (SceneManager.GetActiveScene().name == "MainMenu")
            //{
            //    GameObject notificationObj = GameObject.Find("NotificationPanel");
            //    if (notificationObj != null)
            //    {
            //        notificationPanel = notificationObj.GetComponent<AchievementNotification>();
            //        Debug.Log("Found notification panel");
            //    }
            //    else
            //    {
            //        Debug.LogWarning("NotificationPanel not found in MainMenu scene");
            //    }
            //}
            if (notificationPanel != null)
            {
                Debug.Log("Found notification panel");
            }
            else
            {
                Debug.Log("xiba");
            }
        }

        public void CheckAchievements(GameManager gameManager)
        {
            if (notificationPanel == null) Init();

            // Check first building achievement
            if (!achievementsList[0].isUnlocked && gameManager.save.buildings.Count > 0)
            {
                UnlockAchievement("FIRST_BUILDING");
            }

            // Check city expansion achievement
            if (!achievementsList[1].isUnlocked && gameManager.save.buildings.Count >= 10)
            {
                UnlockAchievement("CITY_EXPANSION");
            }

            // Check population growth achievement
            if (!achievementsList[2].isUnlocked && ResourceManager.Instance.population >= 100)
            {
                UnlockAchievement("POPULATION_GROWTH");
            }

            // Check wealth accumulation achievement
            if (!achievementsList[3].isUnlocked && ResourceManager.Instance.gold >= 50000)
            {
                UnlockAchievement("WEALTH_ACCUMULATION");
            }
        }

        private void UnlockAchievement(string achievementId)
        {
            var achievement = achievementsList.Find(a => a.id == achievementId);
            if (achievement != null && !achievement.isUnlocked)
            {
                Debug.Log("Unlocked achivement");
                achievement.isUnlocked = true;

                if (notificationPanel != null)
                {
                    //notificationPanel.ShowAchievement(
                    //    achievement.displayName,
                    //    achievement.goldReward,
                    //    () => {
                    //        ResourceManager.Instance.AddGold(achievement.goldReward);
                    //        SaveAchievement(achievementId);
                    //    }
                    //);
                    notificationPanel.SetActive(true);
                    AchievementNotification script = notificationPanel.GetComponent<AchievementNotification>();

                    script.achievementText.text = $"Achievement Unlocked!\n{achievement.displayName}\nReward: {achievement.goldReward} Gold";
                    //ResourceManager.Instance.AddGold(achievement.goldReward);
                    SaveAchievement(achievementId);
                }
            }
        }

        private void SaveAchievement(string achievementId)
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { achievementId, "true" }
                }
            };

            PlayFabClientAPI.UpdateUserData(request,
                result => { Debug.Log($"Achievement saved: {achievementId}"); },
                error => { Debug.LogError($"Failed to save achievement: {error.ErrorMessage}"); }
            );
        }

        private void LoadAchievements()
        {
            var request = new GetUserDataRequest();

            PlayFabClientAPI.GetUserData(request,
                result => {
                    if (result.Data != null)
                    {
                        foreach (var achievement in achievementsList)
                        {
                            if (result.Data.ContainsKey(achievement.id))
                            {
                                achievement.isUnlocked = bool.Parse(result.Data[achievement.id].Value);
                            }
                        }
                    }
                },
                error => { Debug.LogError($"Failed to load achievements: {error.ErrorMessage}"); }
            );
        }
    }
}