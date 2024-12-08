using UnityEngine;
using PlayFab;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private string titleId = "A0FD6";

    void Awake()
    {
        PlayFabSettings.staticSettings.TitleId = titleId;
    }
}