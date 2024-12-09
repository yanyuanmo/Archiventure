using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Archiventure
{
    public class AchievementNotification : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] public TextMeshProUGUI achievementText;
        [SerializeField] public Button claimButton;

        private RectTransform rectTransform;
        private System.Action onRewardClaimed;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);

            if (claimButton != null)
                claimButton.onClick.AddListener(OnClaimButtonClicked);
        }

        private void OnClaimButtonClicked()
        {
            onRewardClaimed?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (claimButton != null)
                claimButton.onClick.RemoveListener(OnClaimButtonClicked);
        }
    }
}