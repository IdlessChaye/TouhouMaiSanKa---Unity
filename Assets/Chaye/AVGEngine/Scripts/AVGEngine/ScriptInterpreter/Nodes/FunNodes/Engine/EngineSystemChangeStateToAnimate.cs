using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineSystemChangeStateToAnimate : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("SystemChangeStateToAnimate");
            InterpretPart(context);
        }





        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("EngineSystemChangeStateToAnimate");
            PachiGrimoire.I.StateMachine.TransferStateTo(RunAnimateState.Instance);
        }


    }
}