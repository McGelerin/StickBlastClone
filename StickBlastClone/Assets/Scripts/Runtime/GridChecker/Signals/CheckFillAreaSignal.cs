using UnityEngine;

namespace Runtime.GridChecker.Signals
{
    public readonly struct CheckFillAreaSignal
    {
        private readonly Color32 _levelColor;
        public Color32 LevelColor => _levelColor;

        public CheckFillAreaSignal(Color32 levelColor)
        {
            _levelColor = levelColor;
        }
    }
}