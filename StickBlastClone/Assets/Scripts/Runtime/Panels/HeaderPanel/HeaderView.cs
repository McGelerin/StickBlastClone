using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Panels.Header
{
    public class HeaderView : SerializedMonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        public Button RestartButton => _restartButton;
    }
}