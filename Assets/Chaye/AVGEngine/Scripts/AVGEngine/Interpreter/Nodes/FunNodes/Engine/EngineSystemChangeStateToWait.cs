using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineSystemChangeStateToWait : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("SystemChangeStateToWait");
            InterpretPart(context);
        }




        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("EngineSystemChangeStateToWait");
            PachiGrimoire.I.StateMachine.TransferStateTo(RunWaitState.Instance);
        }

    }
}