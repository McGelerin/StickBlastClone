using System;
using Lofelt.NiceVibrations;
using Runtime.Audio;
using Runtime.Audio.Data;
using Runtime.Audio.Signal.Audio;
using Runtime.Haptic.Signal;
using Runtime.Input.Enums;
using Runtime.Input.Raycasting;
using Runtime.Input.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Runtime.Input.InputStates
{
    public class ClickState : IInputState
    {
        private readonly IInputModel _inputModel;
        private readonly SignalBus _signalBus;
        private readonly IClickRaycaster _clickRaycaster;
        
        
        private Vector2 _touchPosition;
        private Touch _touch;
        
        private IClickable _clickable;
        
                
        private const float SWITCH_TO_DRAG_STATE_SCREEN_SIZE_MOVEMENT_PERCENTAGE = 1;
        private static readonly int _switchToDragStatePixelDelta = CalculateSwitchToDragStateScreenSizeDelta();
        private readonly float _switchToDragDeltaSquared = Mathf.Pow(_switchToDragStatePixelDelta, 2);

        private Vector2 _touchStartPosition = Vector2.zero;
        
        private ClickState(SignalBus signalBus, IClickRaycaster clickRaycaster, IInputModel inputModel)
        {
            _signalBus = signalBus;
            _clickRaycaster = clickRaycaster;
            _inputModel = inputModel;
        }
        
        public void Enter()
        {
            if (UnityEngine.Input.touchCount == 0)
            {
                throw new Exception("No touch input detected when entering Click Input State!");
            }
            
            _touchStartPosition = UnityEngine.Input.GetTouch(0).position;

            _touch = UnityEngine.Input.GetTouch(0);
            _touchPosition = _touch.position;
            _clickable = _clickRaycaster.RaycastTouchPosition(_touchPosition);

            if (_clickable != null)
            {
                _clickable.GetPlaceholderType();
                _signalBus.Fire(new AudioPlaySignal(AudioPlayers.Sound, Sounds.InputClick));
                _signalBus.Fire(new VibrateSignal(HapticPatterns.PresetType.MediumImpact));
                _clickable.OnClick();
            }
        }

        public void Tick()
        {
            if (UnityEngine.Input.touchCount == 1)
            {
                _touch = UnityEngine.Input.GetTouch(0);
                _touchPosition = _touch.position;
                
                if (_touch.phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
                {
                    _clickable?.OnClickEnd();
                    _inputModel.ClearClickable();
                    SwitchToState(InputState.BeforeIdle);

                }
                else if (_clickable != null)
                {
                    if (Vector2.SqrMagnitude(_touchStartPosition - _touchPosition) > _switchToDragDeltaSquared)
                    {
                        _inputModel.SetClickable(_clickable);
                        SwitchToState(InputState.Drag);
                    }
                }
            }
        }
        
        public void Exit()
        {
        }
        
        private void SwitchToState(InputState targetState)
        {
            ChangeInputStateSignal changeInputStateSignal = new (targetState);
            _signalBus.Fire(changeInputStateSignal);
        }
        private static int CalculateSwitchToDragStateScreenSizeDelta()
        {
            float percentageAsFloat = SWITCH_TO_DRAG_STATE_SCREEN_SIZE_MOVEMENT_PERCENTAGE / 100f;
            return Mathf.FloorToInt(Screen.height * percentageAsFloat);
        }
    }
}