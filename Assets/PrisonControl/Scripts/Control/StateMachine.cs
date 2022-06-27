using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public abstract class State : MonoBehaviour
    {
        public virtual void OnEnter() {}
        public virtual void OnExit() {}

        // Returns true if event is consumed, false otherwise
        public virtual bool OnBackButtonPressed() 
        { 
            return false; 
        }
    }

    public class StateMachine<StateId> where StateId : System.Enum
    {
        public StateMachine(params (StateId, State)[] stateDefinitions)
        {
            foreach (var (stateId, state) in stateDefinitions)
                mStates.Add(stateId, state);
        }

        public void SwitchState(StateId stateId)
        {
            if (mCurrentState != null)
                mCurrentState.OnExit();

            State nextState = mStates[stateId];
            mCurrentState = nextState;
            nextState.OnEnter();
        }

        public void Reset()
        {
            if (mCurrentState != null)
                mCurrentState.OnExit();

            mCurrentState = null;
        }

        public State GetCurrentState()
        {
            return mCurrentState;
        }

        private State mCurrentState;
        private readonly Dictionary<StateId, State> mStates = new Dictionary<StateId, State>();
    }
}
