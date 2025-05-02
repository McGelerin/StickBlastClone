using System;
using System.Collections.Generic;
using Runtime.PlaceHolderObject;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Data.Persistent.PlaceholderDataSO
{
    [CreateAssetMenu(fileName = "PlaceholderData", menuName = "Placeholder/Placeholder Data", order = 0)]
    public class PlaceholderSO : SerializedScriptableObject
    {
        public Dictionary<PlaceHolderGridObjectType, List<List<PlaceholderDataVO>>> PlaceHolderData;
    }
    
    [Serializable]
    public class PlaceholderDataVO
    {
        public bool HasLeftEdge;
        public bool HasDownEdge;
    }

}