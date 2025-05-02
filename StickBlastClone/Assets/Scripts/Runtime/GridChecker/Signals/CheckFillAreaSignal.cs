using UnityEngine;

namespace Runtime.GridChecker.Signals
{
    public readonly struct CheckFillAreaSignal
    {
        private readonly bool _isInitialize;
        public bool IsInitialize => _isInitialize;

        public CheckFillAreaSignal(bool isInitialize)
        {
            _isInitialize = isInitialize;
        }
    }
}