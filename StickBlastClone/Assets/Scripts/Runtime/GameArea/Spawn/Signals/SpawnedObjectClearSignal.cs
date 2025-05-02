using Runtime.Input.Raycasting;

namespace Runtime.GameArea.Spawn
{
    public readonly struct SpawnedObjectClearSignal
    {
        private readonly IClickable _clickable;
        public IClickable Clickable => _clickable;

        public SpawnedObjectClearSignal(IClickable clickable)
        {
            _clickable = clickable;
        }
    }
}