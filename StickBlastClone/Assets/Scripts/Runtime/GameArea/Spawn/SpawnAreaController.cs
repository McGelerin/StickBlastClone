using System.Collections.Generic;
using Runtime.Data.Persistent.Level;
using Runtime.Data.Persistent.LevelDataSO;
using Runtime.Infrastructures.Template;
using Runtime.Input.Raycasting;
using Runtime.Models;
using Runtime.PlaceHolderGridObject;
using Runtime.PlaceHolderObject;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace Runtime.GameArea.Spawn
{
    public class SpawnAreaController : SignalListener
    {
        [Inject] private SpawnAreaView _spawnAreaView;
        
        private readonly GameProgressModel _gameProgressModel;
        private readonly LevelsContainer _levelsContainer;
        private readonly PlaceholderGridObject.Factory _placeholderObjectFactory;

        private List<IClickable> _spawnedPlaceholderGridObject = new List<IClickable>();
        public List<IClickable> SpawnedPlaceholderGridObject => _spawnedPlaceholderGridObject;

        public SpawnAreaController(GameProgressModel gameProgressModel, LevelsContainer levelsContainer, PlaceholderGridObject.Factory placeholderObjectFactory)
        {
            _gameProgressModel = gameProgressModel;
            _levelsContainer = levelsContainer;
            _placeholderObjectFactory = placeholderObjectFactory;
        }
        
        private void OnSpawnObjectSignal(SpawnObjectSignal spawnObjectSignal)
        {
            SpawnPlaceholderObjects();
        }

        private void OnSpawnedObjectClearSignal(SpawnedObjectClearSignal signal)
        {
            SpawnedPlaceholderGridObject.Remove(signal.Clickable);
            SpawnedPlaceholderGridObject.TrimExcess();
            SpawnPlaceholderObjects();
        }
        
        private void SpawnPlaceholderObjects()
        {
            if (!SpawnedPlaceholderGridObject.IsNullOrEmpty()) return;
            
            int level = _gameProgressModel.Level % _levelsContainer.LevelData.Count;
            
            LevelSo levelSo = _levelsContainer.LevelData[level];

            for (int i = 0; i <= 2; i++)
            {
                PlaceHolderGridObjectType randomGridObjectType = levelSo.PlaceHolderTypes[Random.Range(0, levelSo.PlaceHolderTypes.Count)];

                PlaceholderGridObject placeholderObject = _placeholderObjectFactory.Create(randomGridObjectType, _spawnAreaView.SpawnObjectHolder[i].transform,
                    levelSo.LevelColor);

                var transform = placeholderObject.transform;
                transform.position = _spawnAreaView.SpawnObjectHolder[i].position;
                transform.rotation = Quaternion.identity;
                
                SpawnedPlaceholderGridObject.Add(placeholderObject);
            }
        }
        
        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<SpawnObjectSignal>()
                .Subscribe(OnSpawnObjectSignal)
                .AddTo(_disposables);
            
            _signalBus.GetStream<SpawnedObjectClearSignal>()
                .Subscribe(OnSpawnedObjectClearSignal)
                .AddTo(_disposables);
        }
    }
}