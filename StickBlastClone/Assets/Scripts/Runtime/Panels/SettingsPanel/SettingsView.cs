using System.Collections.Generic;
using Runtime.Identifiers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Panels.Settings
{
    public class SettingsView : SerializedMonoBehaviour
    {
        [SerializeField] private Button _togglePanelButton;
        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _hapticButton;
        [SerializeField] private List<Transform> _buttonTransforms;
        [SerializeField] private Dictionary<SettingsTypes, GameObject> _settingsOffGameObjects;

        public Button TogglePanelButton => _togglePanelButton;
        public Button SoundButton => _soundButton;
        public Button MusicButton => _musicButton;
        public Button HapticButton => _hapticButton;
        public Dictionary<SettingsTypes, GameObject> SettingsOffGameObjects => _settingsOffGameObjects;
        public List<Transform> ButtonTransforms => _buttonTransforms;
    }
}