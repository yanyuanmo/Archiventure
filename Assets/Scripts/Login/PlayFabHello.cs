using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabHello : MonoBehaviour
{
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "A0FD6";
        }

        var request = new LoginWithCustomIDRequest { CustomId = "yanyuan", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"Hello Archiventure£¡ {result.PlayFabId}");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}