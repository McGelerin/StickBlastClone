using UnityEngine;

namespace Runtime.Models
{
    public interface IGameProgressModel
    {
        int Level { get; }
        
        Color32 ThemeColor { get; }

        void IncreaseLevel();
    }
}