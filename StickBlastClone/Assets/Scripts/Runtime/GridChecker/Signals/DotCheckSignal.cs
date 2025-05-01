using UnityEngine;

namespace Runtime.GridChecker.Signals
{
    public readonly struct DotCheckSignal
    {
        private readonly Color32 _levelColor;
        public Color32 LevelColor => _levelColor;
        
        public DotCheckSignal(Color32 levelColor)
        {
            _levelColor = levelColor;
        }
    }
}