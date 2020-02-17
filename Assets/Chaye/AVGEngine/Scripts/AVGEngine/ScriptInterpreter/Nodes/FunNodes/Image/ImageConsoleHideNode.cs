using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageConsoleHideNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("ConsoleHide");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("ImageConsoleHideNode");

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                StageRenderManager.I.ConsoleHide(false);
            } else {
                StageRenderManager.I.ConsoleHide();
            }
        }

    }
}