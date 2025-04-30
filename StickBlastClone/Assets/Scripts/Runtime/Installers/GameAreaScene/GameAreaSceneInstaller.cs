using Runtime.GameArea;
using Runtime.GameArea.LevelArea;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.GridChecker;
using Runtime.GridChecker.Signals;
using Runtime.PlaceHolder;
using Runtime.PlaceHolderObject;
using Runtime.Signals;
using UnityEngine;
using Zenject;

namespace Runtime.Installers.GameAreaScene
{
    public class GameAreaSceneInstaller : MonoInstaller<GameAreaSceneInstaller>
    {
        [SerializeField] private GameObject gridGeneratorGameObject;
        [SerializeField] private GameObject spawnAreaViewGameObject;
        [SerializeField] private GameObject placeholderPrefab;
        
        public override void InstallBindings()
        {
            BindSignals();
            
            BindGrid();
            BindSpawnArea();
            
            Container.BindInterfacesAndSelfTo<LevelCreator>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameAreaInitializer>().AsSingle();
        }

        private void BindGrid()
        {
            Container.Bind<GridGenerator>().FromComponentsOn(gridGeneratorGameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<EdgeChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridFillChecker>().AsSingle();
        }

        private void BindSpawnArea()
        {
            Container.Bind<SpawnAreaView>().FromComponentsOn(spawnAreaViewGameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnAreaController>().AsSingle();
            
            Container.BindFactory<PlaceHolderType,Transform, Color32, PlaceholderObject, PlaceholderObject.Factory>()
                .FromPoolableMemoryPool<PlaceHolderType, Transform, Color32, PlaceholderObject, PlaceholderPool>(poolBinder => poolBinder
                    .WithInitialSize(5)
                    .FromComponentInNewPrefab(placeholderPrefab)
                    .UnderTransformGroup("PlaceholderObjects"));
        }
        
        private void BindSignals()
        {
            Container.DeclareSignal<CreateLevelAreaSignal>();
            Container.DeclareSignal<SpawnObjectSignal>();
            Container.DeclareSignal<SpawnedObjectClearSignal>();
            Container.DeclareSignal<CheckFillAreaSignal>();

        }
        
        class PlaceholderPool : MonoPoolableMemoryPool<PlaceHolderType,Transform, Color32, IMemoryPool, PlaceholderObject>
        {
        }
    }
}