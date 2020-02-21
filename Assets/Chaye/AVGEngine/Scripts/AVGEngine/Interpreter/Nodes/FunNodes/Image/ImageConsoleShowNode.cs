using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageConsoleShowNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ConsoleShow");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("ImageConsoleShowNode");

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                StageRenderManager.I.ConsoleShow(false);
            }else {
                StageRenderManager.I.ConsoleShow();
            }
        }

    }
}