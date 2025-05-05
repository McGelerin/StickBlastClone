using System;
using System.Collections.Generic;
using Runtime.PlaceHolderObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Persistent.SpriteContainer
{
    [CreateAssetMenu(fileName = "SpriteContainer", menuName = "Placeholder/Sprite Container", order = 0)]
    public class SpriteContainerSO : SerializedScriptableObject
    {
        public Dictionary<PlaceHolderGridObjectType, SpriteDatum> SpriteData = new Dictionary<PlaceHolderGridObjectType, SpriteDatum>();
    }

    [Serializable]
    public class SpriteDatum
    {
        public Sprite Sprite;
        public bool FlipX;
    }
}