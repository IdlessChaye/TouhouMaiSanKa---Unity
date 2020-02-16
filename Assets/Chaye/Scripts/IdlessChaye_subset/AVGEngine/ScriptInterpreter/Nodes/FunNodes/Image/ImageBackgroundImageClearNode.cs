using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageBackgroundImageClearNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("BackgroundImageClear");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("ImageBackgroundImageClearNode");

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                StageRenderManager.I.BackgroundImageClear(false);
            }else {
                StageRenderManager.I.BackgroundImageClear();
            }
        }

    }
}