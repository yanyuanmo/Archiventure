using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private TextMeshProUGUI messageText;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);

        messageText.text = "";
    }

    private void OnLoginClicked()
    {

        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            messageText.text = "Username/password can't be empty!";
            return;
        }

        messageText.text = "Login...";

        var request = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(request,
            OnLoginSuccess,
            OnLoginFailure
        );
    }

    private void OnRegisterClicked()
    {
        
        if (string.IsNullOrEmpty(usernameInput.text) || string.IsNullOrEmpty(passwordInput.text))
        {
            messageText.text = "Username/password can't be empty!";
            return;
        }
   
        messageText.text = "Registering...";

        var request = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false // For simplicity, don't ask for email
        };

        PlayFabClientAPI.RegisterPlayFabUser(request,
            OnRegisterSuccess,
            OnRegisterFailure
        );
    }

    private void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Login succeeded!";
        Debug.Log("Login succeeded!");
        SceneManager.LoadScene("SelectCultureScene");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        messageText.text = "Login failed: " + error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Register succeeded!";
        Debug.Log("Register succeeded!");
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        messageText.text = "Register failed: " + error.ErrorMessage;
        Debug.LogError(error.GenerateErrorReport());
    }
}