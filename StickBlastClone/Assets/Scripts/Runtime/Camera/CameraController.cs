using DG.Tweening;
using Runtime.Infrastructures.Template;
using Runtime.Signals;
using UniRx;
using UnityEngine;

namespace Runtime.Camera
{
    public class CameraController : SignalListener
    {
        private readonly UnityEngine.Camera _camera;

        public CameraController(UnityEngine.Camera camera)
        {
            _camera = camera;
        }

        private void OnShakeCamera(ShakeCameraSignal signal)
        {
            _camera.DOShakePosition(duration: .5f, strength: new Vector3(.08f, 0, 0), vibrato: 10, randomness: 90);
        }

        protected override void SubscribeToSignals()
        {
            _signalBus.GetStream<ShakeCameraSignal>()
                .Subscribe(OnShakeCamera)
                .AddTo(_disposables);
        }
    }
}