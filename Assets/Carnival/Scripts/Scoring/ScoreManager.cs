using TMPro;
using UnityEngine;

namespace Carnival
{
    public sealed class ScoreManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        public int Score { get; private set; }

        // Observer lifecycle: subscribe only while this component is enabled.
        private void OnEnable()
        {
            Target.Hit += HandleTargetHit;
            RefreshDisplay();
        }

        private void OnDisable() => Target.Hit -= HandleTargetHit;

        private void Start()
        {
            if (scoreText == null) Debug.LogError("ScoreManager needs a Score Text reference.", this);
        }

        private void HandleTargetHit(int points)
        {
            Score += points;
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (scoreText != null) scoreText.text = "Score: " + Score;
        }
    }
}
