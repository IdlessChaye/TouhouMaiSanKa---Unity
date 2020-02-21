using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageSmallFigureImageChangeNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("SmallFigureImageChange");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("ImageSmallFigureImageChangeNode");
            string index = paraList[0];

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                StageRenderManager.I.SmallFigureImageChange(index, false);
            } else {
                StageRenderManager.I.SmallFigureImageChange(index);
            }
        }

    }
}