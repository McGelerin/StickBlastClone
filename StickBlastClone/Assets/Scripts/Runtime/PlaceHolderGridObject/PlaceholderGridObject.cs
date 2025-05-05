using DG.Tweening;
using Runtime.Data.Persistent.SpriteContainer;
using Runtime.Input.Raycasting;
using Runtime.PlaceHolderObject;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Runtime.PlaceHolderGridObject
{
    public class PlaceholderGridObject : SerializedMonoBehaviour, IClickable, IPoolable<PlaceHolderGridObjectType,Transform,Color32,IMemoryPool>
    {
        [SerializeField] private SpriteRenderer visualSpriteRenderers;
        [SerializeField] private SpriteContainerSO spriteContainerSo;
        [SerializeField] private float clickOffset = 1f;
        
        IMemoryPool _pool;
        private Transform _firstTransform;
        private PlaceHolderGridObjectType placeHolderGridObjectType;
        
        public Vector3 GetPosition() => transform.position;
        public PlaceHolderGridObjectType GetPlaceholderType() => placeHolderGridObjectType;
        
        public void OnClick()
        {
            visualSpriteRenderers.transform.DOScale(Vector3.one, .001f);
            Vector3 firstPos = transform.position;
            transform.DOMove(new Vector3(firstPos.x, firstPos.y, firstPos.z + clickOffset), .001f);
        }

        public void OnClickEnd()
        {
            visualSpriteRenderers.transform.DOScale(new Vector3(.7f,.7f, .7f), .001f);
            transform.DOMove(_firstTransform.position, .001f);
        }
        
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
                visualSpriteRenderers.transform.DOScale(new Vector3(.7f,.7f, .7f), .001f);
                transform.position = _firstTransform.position;
            }
        }

        public void OnSpawned(PlaceHolderGridObjectType placeHolderGridObjectType,Transform firstTransform, Color32 color ,IMemoryPool pool)
        {
            _pool = pool;
            _firstTransform = firstTransform;
            this.placeHolderGridObjectType = placeHolderGridObjectType;
            visualSpriteRenderers.transform.localScale = new Vector3(.7f,.7f, .7f);
            
            SetColor(color);
            SetSprite();
            OpenVisual();
        }

        public void OnDespawned()
        {
            _pool = null;
            visualSpriteRenderers.gameObject.SetActive(false);
            this.DOKill();
        }

        private void SetColor(Color32 color)
        {
            visualSpriteRenderers.color = color;
        }

        private void SetSprite()
        {
            visualSpriteRenderers.sprite = spriteContainerSo.SpriteData[placeHolderGridObjectType].Sprite;
            visualSpriteRenderers.flipX = spriteContainerSo.SpriteData[placeHolderGridObjectType].FlipX;
        }

        private void OpenVisual()
        {
            visualSpriteRenderers.gameObject.SetActive(true);
        }
        
        public class Factory : PlaceholderFactory<PlaceHolderGridObjectType,Transform, Color32,PlaceholderGridObject>
        {
        }
    }
}