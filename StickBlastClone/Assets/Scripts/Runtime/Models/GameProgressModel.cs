using Runtime.Data.Persistent.Level;
using UnityEngine;
using Zenject;

namespace Runtime.Models
{
    public class GameProgressModel : IGameProgressModel, IInitializable
    {
        
        
        private int _level;
        public int Level => _level;

        public Color32 ThemeColor => _themeColor;
        private Color32 _themeColor;

        public int RequirementScore => _requirementScore;
        private int _requirementScore;

        private const string GAME_PROGRESS_DATA_PATH = "GAME_PROGRESS";
        private const string LEVEL_KEY = "LEVEL";

        [Inject] private LevelsContainer _levelsContainer;
        
        public void Initialize()
        {
            int level = Level % _levelsContainer.LevelData.Count;
            _themeColor = _levelsContainer.LevelData[level].LevelColor;
            _requirementScore = _levelsContainer.LevelData[level].RequirementScore;
        }
        public GameProgressModel()
        {
            _level = ES3.Load(LEVEL_KEY, GAME_PROGRESS_DATA_PATH, 0);
        }

        public void IncreaseLevel()
        {
            _level++;
            ES3.Save(LEVEL_KEY, _level, GAME_PROGRESS_DATA_PATH);
        }
    }
}