using System.Collections.Generic;
using Runtime.Input.Raycasting;
using Runtime.PlaceHolderObject;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Runtime.PlaceHolder
{
    public class PlaceholderObject : MonoBehaviour, IClickable, IPoolable<PlaceHolderType,Transform,Color32,IMemoryPool>
    {
        [SerializeField] private SerializedDictionary<PlaceHolderType, GameObject> visualGameObjects = new SerializedDictionary<PlaceHolderType, GameObject>();
        [SerializeField] private List<SpriteRenderer> allSpriteRenderers = new List<SpriteRenderer>();
        
        IMemoryPool _pool;
        private Transform _firstTransform;
        private PlaceHolderType _placeHolderType;
        
        public PlaceHolderType OnClick()
        {
            return _placeHolderType;
        }

        public void OnDrag()
        {
            throw new System.NotImplementedException();
        }

        public void OnDragEnd(bool isDeSpawn)
        {
            if (isDeSpawn)
            {
                _pool.Despawn(this);
            }
            else
            {
                transform.position = _firstTransform.position;
            }
        }

        public void OnSpawned(PlaceHolderType placeHolderType,Transform firstTransform, Color32 color ,IMemoryPool pool)
        {
            _pool = pool;
            _firstTransform = firstTransform;
            _placeHolderType = placeHolderType;

            SetColor(color);
            OpenVisual();
        }
        
        public void OnDespawned()
        {
            _pool = null;
            foreach (GameObject gameObject in visualGameObjects.Values)
            {
                gameObject.SetActive(false);
            }
        }

        private void SetColor(Color32 color)
        {
            foreach (SpriteRenderer allSpriteRenderer in allSpriteRenderers)
            {
                allSpriteRenderer.color = color;
            }
        }

        private void OpenVisual()
        {
            visualGameObjects[_placeHolderType].SetActive(true);
        }
        
        public class Factory : PlaceholderFactory<PlaceHolderType,Transform, Color32,PlaceholderObject>
        {
        }
    }
}