using Runtime.Camera;
using Runtime.GameArea;
using Runtime.GameArea.LevelArea;
using Runtime.GameArea.Spawn;
using Runtime.Grid;
using Runtime.GridChecker;
using Runtime.GridChecker.Signals;
using Runtime.Laser;
using Runtime.LevelEnd;
using Runtime.Models;
using Runtime.PlaceHolderGridObject;
using Runtime.PlaceHolderObject;
using Runtime.Score;
using Runtime.Signals;
using Runtime.Signals.Score;
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
            BindScore();
            BindLevelEnd();
            BindCamera();
            
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

        private void BindScore()
        {
            Container.BindInterfacesAndSelfTo<ScoreModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreController>().AsSingle();
        }

        private void BindLevelEnd()
        {
            Container.BindInterfacesAndSelfTo<LevelEndController>().AsSingle();
        }

        private void BindCamera()
        {
            Container.Bind<UnityEngine.Camera>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraController>().AsSingle();
        }
        
        private void BindPool()
        {
            Container.BindFactory<PlaceHolderGridObjectType,Transform, Color32, PlaceholderGridObject, PlaceholderGridObject.Factory>()
                // We could just use FromMonoPoolableMemoryPool here instead, but
                // for IL2CPP to work we need our pool class to be used explicitly here
                .FromPoolableMemoryPool<PlaceHolderGridObjectType, Transform, Color32, PlaceholderGridObject, PlaceholderPool>(poolBinder => poolBinder
                    .WithInitialSize(5)
                    .FromComponentInNewPrefab(placeholderPrefab)
                    .UnderTransformGroup("PlaceholderObjects"));
            
            Container.BindFactory<bool, float, Color32, LaserVFX ,LaserVFX.Factory>()
                // We could just use FromMonoPoolableMemoryPool here instead, but
                // for IL2CPP to work we need our pool class to be used explicitly here
                .FromPoolableMemoryPool<bool, float, Color32, LaserVFX, LaserPool>(poolBinder => poolBinder
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
            Container.DeclareSignal<IncreaseScoreSignal>();
            Container.DeclareSignal<CheckLevelEndSignal>();
            Container.DeclareSignal<ShakeCameraSignal>();
            
            //Setting Panel
            Container.DeclareSignal<GameSceneSettingsChangeSignal>();
            
            //ScorePanel
            Container.DeclareSignal<UpdateScorePanelSignal>();
        }
        
        class PlaceholderPool : MonoPoolableMemoryPool<PlaceHolderGridObjectType,Transform, Color32, IMemoryPool, PlaceholderGridObject>
        {
        }
        
        class LaserPool : MonoPoolableMemoryPool<bool, float, Color32, IMemoryPool, LaserVFX>
        {
        }
    }
}