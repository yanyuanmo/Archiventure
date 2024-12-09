using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading;

namespace Archiventure
{
    public class AchievementManager : MonoBehaviour
    {
        [System.Serializable]
        public class AchievementData
        {
            public string id;
            public string displayName;
            public float goldReward;
            public bool isUnlocked;
        }

        [SerializeField]
        public List<AchievementData> achievementsList = new List<AchievementData>
        {
            new AchievementData { id = "FIRST_BUILDING", displayName = "First Building", goldReward = 100f },
            new AchievementData { id = "CITY_EXPANSION", displayName = "City Expansion", goldReward = 500f },
            new AchievementData { id = "POPULATION_GROWTH", displayName = "Population Growth", goldReward = 1000f },
            new AchievementData { id = "WEALTH_ACCUMULATION", displayName = "Wealth Accumulation", goldReward = 2000f }
        };

        [Header("Game Objects")]
        public GameObject notificationPanel;

        public AchievementData lastUnlockedAchievement;

        private void Start()
        {
            LoadAchievements();
            Thread.Sleep(2000);
            Debug.Log("guoplai");
        }

        public void Init()
        {
            if (notificationPanel != null)
            {
                Debug.Log("Found notification panel");
            }
            else
            {
                Debug.Log("Not found notification panel");
            }
        }

        public void UnlockAchievement(string achievementId)
        {
            var achievement = achievementsList.Find(a => a.id == achievementId);
            if (achievement != null && !achievement.isUnlocked)
            {
                Debug.Log("Unlocked achivement");
                achievement.isUnlocked = true;

                if (notificationPanel != null)
                {
                    notificationPanel.SetActive(true);
                    AchievementNotification script = notificationPanel.GetComponent<AchievementNotification>();

                    script.achievementText.text = $"Achievement Unlocked!\n{achievement.displayName}\nReward: {achievement.goldReward} Gold";
                    lastUnlockedAchievement = achievement;
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
                        Debug.Log("Found user data");
                        foreach (var achievement in achievementsList)
                        {
                            if (result.Data.ContainsKey(achievement.id))
                            {
                                Debug.Log("Found achievement");
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