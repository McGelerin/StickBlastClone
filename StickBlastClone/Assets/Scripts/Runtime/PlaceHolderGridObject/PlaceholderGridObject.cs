using System.Collections.Generic;
using Runtime.Input.Raycasting;
using Runtime.PlaceHolderObject;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Runtime.PlaceHolderGridObject
{
    public class PlaceholderGridObject : SerializedMonoBehaviour, IClickable, IPoolable<PlaceHolderGridObjectType,Transform,Color32,IMemoryPool>
    {
        [SerializeField] private Dictionary<PlaceHolderGridObjectType, GameObject> visualGameObjects;
        [SerializeField] private List<SpriteRenderer> allSpriteRenderers = new List<SpriteRenderer>();
        
        IMemoryPool _pool;
        private Transform _firstTransform;
        private PlaceHolderGridObjectType placeHolderGridObjectType;
        private Color32 _levelColor;
        
        public Vector3 GetPosition() => transform.position;
        public Color32 GetColor() => _levelColor;
        public PlaceHolderGridObjectType GetPlaceholderType() => placeHolderGridObjectType;


        public void OnDrag(Vector3 targetPosition)
        {
            var position = transform.position;
            Vector3 normalizedTargetPosition = Vector3.Lerp(position, position + targetPosition, 0.008f);

            position = normalizedTargetPosition;
            transform.position = position;
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

        public void OnSpawned(PlaceHolderGridObjectType placeHolderGridObjectType,Transform firstTransform, Color32 color ,IMemoryPool pool)
        {
            _pool = pool;
            _firstTransform = firstTransform;
            this.placeHolderGridObjectType = placeHolderGridObjectType;

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
            _levelColor = color;
            
            foreach (SpriteRenderer allSpriteRenderer in allSpriteRenderers)
            {
                allSpriteRenderer.color = color;
            }
        }

        private void OpenVisual()
        {
            visualGameObjects[placeHolderGridObjectType].SetActive(true);
        }
        
        public class Factory : PlaceholderFactory<PlaceHolderGridObjectType,Transform, Color32,PlaceholderGridObject>
        {
        }
    }
}