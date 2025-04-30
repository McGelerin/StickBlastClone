using Runtime.PlaceHolderObject;
using UnityEngine;

namespace Runtime.Input.Raycasting
{
    public interface IClickable
    {
        public PlaceHolderType GetPlaceholderType();
        public Vector3 GetPosition();
        public Color32 GetColor();
        void OnDrag(Vector3 targetPosition);
        void OnDragEnd(bool isDeSpawn);
    }
}