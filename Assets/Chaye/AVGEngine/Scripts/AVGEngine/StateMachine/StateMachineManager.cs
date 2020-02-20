using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StateMachineManager {

        public BaseState LastState { get; private set; }
        public BaseState CurrentState { get; private set; }
        public StateBuff StateBuff { get; private set; }

        private readonly BaseState[] stateArray = {
            VoidState.Instance , InitState.Instance, IdleState.Instance,
            RunScriptState.Instance, RunWaitState.Instance ,RunAnimateState.Instance,
            ChoiceWaitState.Instance,SleepState.Instance
        };

        public StateMachineManager(BaseState currentState) {
            this.CurrentState = currentState;
            StateBuff = StateBuff.Normal;
        }

        public void TransferStateTo(BaseState newState) {
            if (newState.CanBeTransferedFrom(CurrentState)) {
                CurrentState.OnExit(newState);
                newState.OnEnter(CurrentState);
                LastState = CurrentState;
                CurrentState = newState;
                CurrentState.OnHasEntered();
            } else {
                throw new System.Exception($"状态机切换状态失败!\n现状态 : {CurrentState.StateName}    新状态 : {newState.StateName}\n");
            }
        }

        public void SetStateBuff(StateBuff stateBuff) {
            StateBuff = stateBuff;
            Messenger.Broadcast("SetStateBuff", StateBuff);
        }

        public void LoadStoryRecord(string currentStateName,string lastStateName, StateBuff stateBuff) {
            StateBuff = stateBuff;
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

        public void FinalizeStory() {
            StateBuff = StateBuff.Normal;
        }
    }

}