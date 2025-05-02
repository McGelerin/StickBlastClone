using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Panels.ScorePanel
{
    public class ScorePanelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Image _fillImage;

        public TextMeshProUGUI ScoreText => _scoreText;
        public Image FillImage => _fillImage;
    }
}