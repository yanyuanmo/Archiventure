using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;

namespace Archiventure
{
    public class CloudSaveManager : MonoBehaviour
    {
        public static CloudSaveManager Instance { get; private set; }

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

        // Save game data to cloud
        public void SaveToCloud(SaveData saveData)
        {
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"GameSaveData", JsonUtility.ToJson(saveData)}
                }
            };

            PlayFabClientAPI.UpdateUserData(request,
                result => { Debug.Log("Cloud save successful"); },
                error => { Debug.LogError($"Cloud save failed: {error.ErrorMessage}"); }
            );
        }

        // Load game data from cloud
        public void LoadFromCloud(System.Action<SaveData> onDataLoaded)
        {
            var request = new GetUserDataRequest();

            PlayFabClientAPI.GetUserData(request,
                result => {
                    if (result.Data != null && result.Data.ContainsKey("GameSaveData"))
                    {
                        var saveData = JsonUtility.FromJson<SaveData>(result.Data["GameSaveData"].Value);
                        onDataLoaded?.Invoke(saveData);
                        Debug.Log("Cloud load successful");
                    }
                    else
                    {
                        Debug.Log("No cloud save found");
                        onDataLoaded?.Invoke(new SaveData());
                    }
                },
                error => {
                    Debug.LogError($"Cloud load failed: {error.ErrorMessage}");
                    onDataLoaded?.Invoke(new SaveData());
                }
            );
        }
    }
}