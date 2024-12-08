using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
    
public class StyleSelectManager : MonoBehaviour
{
    [Header("Style Buttons")]
    [SerializeField] private Button chineseStyleButton;
    [SerializeField] private Button europeanStyleButton;  

    [Header("UI Elements")]
    [SerializeField] private Button logoutButton;         

    void Start()
    {
        chineseStyleButton.onClick.AddListener(OnChineseStyleClick);
        logoutButton.onClick.AddListener(OnLogoutClick);

        if (europeanStyleButton != null)
            europeanStyleButton.interactable = false;
    }

    private void OnChineseStyleClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnLogoutClick()
    {
        SceneManager.LoadScene("StartScene");
    }
}
