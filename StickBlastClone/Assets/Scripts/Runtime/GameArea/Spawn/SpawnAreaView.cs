using System.Collections.Generic;
using UnityEngine;

namespace Runtime.GameArea.Spawn
{
    public class SpawnAreaView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnObjectHolder = new List<Transform>();
        public List<Transform> SpawnObjectHolder => _spawnObjectHolder;
    }
}