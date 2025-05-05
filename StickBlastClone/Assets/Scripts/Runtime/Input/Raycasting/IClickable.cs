using Runtime.PlaceHolderObject;
using UnityEngine;

namespace Runtime.Input.Raycasting
{
    public interface IClickable
    {
        public PlaceHolderGridObjectType GetPlaceholderType();
        public Vector3 GetPosition();
        public void OnClick();
        public void OnClickEnd();
        void OnDrag(Vector3 targetPosition);
        void OnDragEnd(bool isDeSpawn);
    }
}