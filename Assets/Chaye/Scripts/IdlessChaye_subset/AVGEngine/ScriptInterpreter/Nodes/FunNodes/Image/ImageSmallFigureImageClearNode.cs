using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageSmallFigureImageClearNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("SmallFigureImageClear");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("ImageSmallFigureImageClearNode");

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                StageRenderManager.I.SmallFigureImageClear(false);
            } else {
                StageRenderManager.I.SmallFigureImageClear();
            }
        }

    }
}