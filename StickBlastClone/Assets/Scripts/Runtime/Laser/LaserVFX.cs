using System;
using UnityEngine;
using Zenject;

namespace Runtime.Laser
{
    public class LaserVFX : MonoBehaviour, IPoolable<bool,float,IMemoryPool>
    {
        [SerializeField] private GameObject HorizontalLaser;
        [SerializeField] private GameObject VerticalLaser;
        
        private IMemoryPool _pool;
        private float _startTime;
        private float _lifeTime;
        private bool _isHorizontal;
        
        public void Update()
        {
            if (Time.realtimeSinceStartup - _startTime > _lifeTime)
            {
                _pool.Despawn(this);
            }
        }
        
        public void OnDespawned()
        {
            HorizontalLaser.SetActive(false);
            VerticalLaser.SetActive(false);
        }
        
        public void OnSpawned(bool isHorizontal, float lifeTime, IMemoryPool pool)
        {
            _pool = pool;
            _isHorizontal = isHorizontal;
            _lifeTime = lifeTime;
            _startTime = Time.realtimeSinceStartup;

            OpenLaser();
        }

        private void OpenLaser()
        {
            if (_isHorizontal)
            {
                HorizontalLaser.SetActive(true);
            }
            else
            {
                VerticalLaser.SetActive(true);
            }
        }
        
        public class Factory : PlaceholderFactory<bool, float, LaserVFX>
        {
        }
    }
}