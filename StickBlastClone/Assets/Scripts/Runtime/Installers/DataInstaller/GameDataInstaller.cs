using Runtime.Audio.Data;
using Runtime.Data.Persistent.Level;
using Runtime.Data.Persistent.PlaceholderDataSO;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Runtime.Installers.DataInstaller
{
    [CreateAssetMenu(fileName = "Game Data Installer", menuName = "Installers/Data/Game Data Installer")]
    public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        [SerializeField] private LevelsContainer levelsContainer;
        [SerializeField] private AudioData audioData;
        [SerializeField] private PlaceholderSO placeholderSo;
        
        public override void InstallBindings()
        {
            Container.BindInstances(levelsContainer,audioData, placeholderSo);
        }
    }
}