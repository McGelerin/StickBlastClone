using System;
using UnityEngine;
using Zenject;

namespace Runtime.Laser
{
    public class LaserVFX : MonoBehaviour, IPoolable<bool,float, Color32 ,IMemoryPool>
    {
        [SerializeField] private SpriteRenderer HorizontalLaserSpriteRenderer;
        [SerializeField] private SpriteRenderer VerticalLaserSpriteRenderer;
        
        private IMemoryPool _pool;
        private float _startTime;
        private float _lifeTime;
        private bool _isHorizontal;
        private Color32 _blastColor;
        
        public void Update()
        {
            if (Time.realtimeSinceStartup - _startTime > _lifeTime)
            {
                _pool.Despawn(this);
            }
        }
        
        public void OnDespawned()
        {
            HorizontalLaserSpriteRenderer.gameObject.SetActive(false);
            VerticalLaserSpriteRenderer.gameObject.SetActive(false);
        }
        
        public void OnSpawned(bool isHorizontal, float lifeTime, Color32 blastColor ,IMemoryPool pool)
        {
            _pool = pool;
            _isHorizontal = isHorizontal;
            _lifeTime = lifeTime;
            _startTime = Time.realtimeSinceStartup;
            _blastColor = blastColor;
            
            OpenLaser();
        }

        private void OpenLaser()
        {
            if (_isHorizontal)
            {
                HorizontalLaserSpriteRenderer.color = _blastColor;
                HorizontalLaserSpriteRenderer.gameObject.SetActive(true);
            }
            else
            {
                VerticalLaserSpriteRenderer.color = _blastColor;
                VerticalLaserSpriteRenderer.gameObject.SetActive(true);
            }
        }
        
        public class Factory : PlaceholderFactory<bool, float, Color32,LaserVFX>
        {
        }
    }
}