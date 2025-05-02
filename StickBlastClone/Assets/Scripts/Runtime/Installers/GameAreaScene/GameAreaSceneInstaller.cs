using Runtime.GameArea;
using Runtime.GameArea.LevelArea;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.GridChecker;
using Runtime.GridChecker.Signals;
using Runtime.Laser;
using Runtime.PlaceHolder;
using Runtime.PlaceHolderObject;
using Runtime.Signals;
using Runtime.Signals.Settings;
using UnityEngine;
using Zenject;

namespace Runtime.Installers.GameAreaScene
{
    public class GameAreaSceneInstaller : MonoInstaller<GameAreaSceneInstaller>
    {
        [SerializeField] private GameObject gridGeneratorGameObject;
        [SerializeField] private GameObject spawnAreaViewGameObject;
        [SerializeField] private GameObject placeholderPrefab;
        [SerializeField] private GameObject laserPrefab;
        
        public override void InstallBindings()
        {
            BindSignals();
            BindPool();
            
            BindGrid();
            BindSpawnArea();
            
            Container.BindInterfacesAndSelfTo<LevelCreator>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameAreaInitializer>().AsSingle();
        }

        private void BindGrid()
        {
            Container.Bind<GridGenerator>().FromComponentsOn(gridGeneratorGameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<EdgeChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<DotChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridFillChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<BlastChecker>().AsSingle();
        }

        private void BindSpawnArea()
        {
            Container.Bind<SpawnAreaView>().FromComponentsOn(spawnAreaViewGameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnAreaController>().AsSingle();
            

        }

        private void BindPool()
        {
            Container.BindFactory<PlaceHolderType,Transform, Color32, PlaceholderGridObject, PlaceholderGridObject.Factory>()
                // We could just use FromMonoPoolableMemoryPool here instead, but
                // for IL2CPP to work we need our pool class to be used explicitly here
                .FromPoolableMemoryPool<PlaceHolderType, Transform, Color32, PlaceholderGridObject, PlaceholderPool>(poolBinder => poolBinder
                    .WithInitialSize(5)
                    .FromComponentInNewPrefab(placeholderPrefab)
                    .UnderTransformGroup("PlaceholderObjects"));
            
            Container.BindFactory<bool, float, LaserVFX ,LaserVFX.Factory>()
                // We could just use FromMonoPoolableMemoryPool here instead, but
                // for IL2CPP to work we need our pool class to be used explicitly here
                .FromPoolableMemoryPool<bool, float, LaserVFX, LaserPool>(poolBinder => poolBinder
                    .WithInitialSize(5)
                    .FromComponentInNewPrefab(laserPrefab)
                    .UnderTransformGroup("LaserObjects"));
        }
        
        private void BindSignals()
        {
            Container.DeclareSignal<CreateLevelAreaSignal>();
            Container.DeclareSignal<SpawnObjectSignal>();
            Container.DeclareSignal<SpawnedObjectClearSignal>();
            Container.DeclareSignal<CheckFillAreaSignal>();
            Container.DeclareSignal<DotCheckSignal>();
            Container.DeclareSignal<BlastCheckSignal>();
            
            //Setting Panel
            Container.DeclareSignal<GameSceneSettingsChangeSignal>();
        }
        
        class PlaceholderPool : MonoPoolableMemoryPool<PlaceHolderType,Transform, Color32, IMemoryPool, PlaceholderGridObject>
        {
        }
        
        class LaserPool : MonoPoolableMemoryPool<bool, float, IMemoryPool, LaserVFX>
        {
        }
    }
}