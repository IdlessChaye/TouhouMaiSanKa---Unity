using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StateMachineManager {
        public BaseState LastState { get; private set; }
        public BaseState CurrentState { get; private set; }

        private readonly BaseState[] stateArray = {
            VoidState.Instance , InitState.Instance, IdleState.Instance,
            RunScriptState.Instance, RunWaitState.Instance,RunSkipState.Instance,
            RunAutoState.Instance,RunNextState.Instance,RunAnimateState.Instance,
            SleepState.Instance,ChoiceWaitState.Instance
        };

        public StateMachineManager(BaseState currentState) {
            this.CurrentState = currentState;
        }

        public void TransferStateTo(BaseState newState) {
            if (newState.CanBeTransferedFrom(CurrentState)) {
                LastState = CurrentState;
                CurrentState.OnExit(newState);
                CurrentState = newState;
                CurrentState.OnEnter(LastState);
            } else {
                throw new System.Exception($"状态机切换状态失败!\n现状态 : {CurrentState.StateName}    新状态 : {newState.StateName}\n");
            }
        }

        public void LoadStoryRecord(string currentStateName,string lastStateName) {
            for(int i = 0;i<stateArray.Length;i++) {
                BaseState state = stateArray[i];
                if(state.StateName.Equals(currentStateName)) {
                    CurrentState = state;
                    break;
                }
            }
            for (int i = 0; i < stateArray.Length; i++) {
                BaseState state = stateArray[i];
                if (state.StateName.Equals(lastStateName)) {
                    LastState = state;
                    break;
                }
            }
        }

        public void SetLastState(string lastStateName) {
            for (int i = 0; i < stateArray.Length; i++) {
                BaseState state = stateArray[i];
                if (state.StateName.Equals(lastStateName)) {
                    LastState = state;
                    break;
                }
            }
        }
    }

}