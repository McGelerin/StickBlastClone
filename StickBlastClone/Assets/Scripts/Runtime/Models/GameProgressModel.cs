using Runtime.Data.Persistent.Level;
using UnityEngine;
using Zenject;

namespace Runtime.Models
{
    public class GameProgressModel : IGameProgressModel
    {
        private int _level;
        public int Level => _level;

        public Color32 ThemeColor => _levelsContainer.LevelData[_level % _levelsContainer.LevelData.Count].LevelColor;

        public int RequirementScore => _levelsContainer.LevelData[_level % _levelsContainer.LevelData.Count].RequirementScore;


        private const string GAME_PROGRESS_DATA_PATH = "GAME_PROGRESS";
        private const string LEVEL_KEY = "LEVEL";

        [Inject] private LevelsContainer _levelsContainer;
        

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