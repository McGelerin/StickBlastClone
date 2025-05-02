using System.Collections.Generic;
using Runtime.Data.Persistent.LevelDataSO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Data.Persistent.Level
{
    [CreateAssetMenu(fileName = "LevelsContainer", menuName = "Level/LevelsDataContainer", order = 0)]
    public class LevelsContainer : SerializedScriptableObject
    {
        [SerializeField]private Dictionary<int, LevelSo> levelData;

        public Dictionary<int, LevelSo> LevelData => levelData;
    }
}