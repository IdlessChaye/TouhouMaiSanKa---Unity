using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StateMachineManager {
        public BaseState LastState { get; set; }
        public BaseState CurrentState { get; set; }

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
    }

}