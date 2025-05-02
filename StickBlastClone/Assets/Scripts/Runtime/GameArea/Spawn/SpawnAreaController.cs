using System.Collections.Generic;
using Runtime.Data.Persistent.Level;
using Runtime.Data.Persistent.LevelDataSO;
using Runtime.Infrastructures.Template;
using Runtime.Models;
using Runtime.PlaceHolder;
using Runtime.PlaceHolderObject;
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

        private int _spawnedObjects = 0;

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
            _spawnedObjects--;
            SpawnPlaceholderObjects();
        }
        
        private void SpawnPlaceholderObjects()
        {
            if (_spawnedObjects != 0) return;
            _spawnedObjects = 3;
            
            int level = _gameProgressModel.Level % _levelsContainer.LevelData.Count;
            
            LevelSo levelSo = _levelsContainer.LevelData[level];

            for (int i = 0; i <= 2; i++)
            {
                PlaceHolderType randomType = levelSo.PlaceHolderTypes[Random.Range(0, levelSo.PlaceHolderTypes.Count)];

                var placeholderObject = _placeholderObjectFactory.Create(randomType, _spawnAreaView.SpawnObjectHolder[i].transform,
                    levelSo.LevelColor);

                var transform = placeholderObject.transform;
                transform.position = _spawnAreaView.SpawnObjectHolder[i].position;
                transform.rotation = Quaternion.identity;
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