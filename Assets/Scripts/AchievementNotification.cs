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

        //[Header("Animation Settings")]
        //[SerializeField] private float slideDuration = 0.5f;

        private RectTransform rectTransform;
        //private float currentAnimationTime;
        //private bool isSliding;
        //private Vector2 startPosition;
        //private Vector2 targetPosition;
        private System.Action onRewardClaimed;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);

            if (claimButton != null)
                claimButton.onClick.AddListener(OnClaimButtonClicked);
        }

        //public void ShowAchievement(string achievementName, float reward, System.Action onClaimed)
        //{
        //    achievementText.text = $"Achievement Unlocked!\n{achievementName}\nReward: {reward} Gold";
        //    onRewardClaimed = onClaimed;

            
        //    startPosition = new Vector2(0, 300);  
        //    targetPosition = new Vector2(0, 0);   

            
        //    gameObject.SetActive(true);
        //    rectTransform.anchoredPosition = startPosition;
        //    isSliding = true;
        //    currentAnimationTime = 0;
        //}

        private void OnClaimButtonClicked()
        {
            onRewardClaimed?.Invoke();
            gameObject.SetActive(false);
        }

        //private void Update()
        //{
        //    if (isSliding)
        //    {
        //        currentAnimationTime += Time.deltaTime;
        //        float t = Mathf.Clamp01(currentAnimationTime / slideDuration);

        //        rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);

        //        if (t >= 1)
        //        {
        //            isSliding = false;
        //        }
        //    }
        //}

        private void OnDestroy()
        {
            if (claimButton != null)
                claimButton.onClick.RemoveListener(OnClaimButtonClicked);
        }
    }
}