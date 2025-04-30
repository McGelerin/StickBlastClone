using Runtime.PlaceHolderObject;
using UnityEngine;

namespace Runtime.Input.Raycasting
{
    public interface IClickable
    {
        PlaceHolderType OnClick();
        void OnDrag();
        void OnDragEnd(bool isDeSpawn);
    }
}