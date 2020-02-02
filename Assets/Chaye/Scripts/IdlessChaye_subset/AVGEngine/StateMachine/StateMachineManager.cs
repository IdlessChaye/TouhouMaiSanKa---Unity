using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class StateMachineManager {
        private BaseState currentState;
        public BaseState CurrentState => currentState;

        public StateMachineManager(BaseState currentState) {
            this.currentState = currentState;
        }

        public void TransferStateTo(BaseState newState) {
            if (newState.CanBeTransferedFrom(currentState)) {
                BaseState oldState = currentState;
                currentState.OnExit(newState);
                currentState = newState;
                currentState.OnEnter(oldState);
            } else {
                throw new System.Exception($"状态机切换状态失败!\n现状态 : {currentState.StateName}    新状态 : {newState.StateName}\n");
            }
        }
    }

}