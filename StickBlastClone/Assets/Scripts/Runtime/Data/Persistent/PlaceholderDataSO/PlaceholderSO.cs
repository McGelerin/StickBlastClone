using System;
using System.Collections.Generic;
using Runtime.PlaceHolderObject;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.Persistent.PlaceholderDataSO
{
    [CreateAssetMenu(fileName = "PlaceholderData", menuName = "Placeholder/Placeholder Data", order = 0)]
    public class PlaceholderSO : SerializedScriptableObject
    {
        public Dictionary<PlaceHolderGridObjectType, List<List<PlaceholderDataVO>>> PlaceHolderData;

        public Dictionary<PlaceHolderGridObjectType, Vector2> PlaceholderOriginOffsets;
    }
    
    [Serializable]
    public class PlaceholderDataVO
    {
        public bool HasLeftEdge;
        public bool HasDownEdge;
    }
}