using Runtime.Input.Enums;
using Runtime.Input.Signals;
using UnityEngine;
using Zenject;

namespace Runtime.Input.InputStates
{
    public class DragState : IInputState
    {
        private readonly SignalBus _signalBus;

        private DragState(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Enter()
        {
        }

        public void Tick()
        {
            if (CheckTransitions()) return;
            
            DragItem();
        }
        
        private bool CheckTransitions()
        {
            bool transition = false;
            
            if (UnityEngine.Input.touchCount == 0)
            {
                SwitchToState(InputState.Idle);
                transition = true;
            }

            return transition;
        }
        
        private void DragItem()
        {
            Touch touch = UnityEngine.Input.GetTouch(0);

            Vector2 deltaPos = touch.deltaPosition;
            
            //drag item;
        }

        private void SwitchToState(InputState targetState)
        {
            ChangeInputStateSignal changeInputStateSignal = new (targetState);
            _signalBus.Fire(changeInputStateSignal);
        }

        public void Exit()
        {
        }
    }
}