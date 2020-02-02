using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdlessChaye.IdleToolkit.AVGEngine;


public class TestStateMachine : MonoBehaviour {
    private StateMachineManager stateMachineManager;
    void Start() {
        stateMachineManager = new StateMachineManager(VoidState.Instance);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            stateMachineManager.TransferStateTo(InitState.Instance);
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            stateMachineManager.TransferStateTo(VoidState.Instance);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            stateMachineManager.TransferStateTo(IdleState.Instance);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            stateMachineManager.TransferStateTo(RunWaitState.Instance);
        } else if (Input.GetKeyDown(KeyCode.F)) {
            stateMachineManager.TransferStateTo(RunScriptState.Instance);
        } else if (Input.GetKeyDown(KeyCode.G)) {
            stateMachineManager.TransferStateTo(RunAutoState.Instance);
        } else if (Input.GetKeyDown(KeyCode.H)) {
            stateMachineManager.TransferStateTo(RunSkipState.Instance);
        } else if (Input.GetKeyDown(KeyCode.J)) {
            stateMachineManager.TransferStateTo(RunNextState.Instance);
        } else if (Input.GetKeyDown(KeyCode.J)) {
            stateMachineManager.TransferStateTo(RunAnimateState.Instance);
        } else if (Input.GetKeyDown(KeyCode.C)) {
            stateMachineManager.TransferStateTo(SleepState.Instance);
        }
    }
}
