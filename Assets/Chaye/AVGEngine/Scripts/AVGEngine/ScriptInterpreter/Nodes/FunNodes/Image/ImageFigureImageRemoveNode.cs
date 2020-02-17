using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageFigureImageRemoveNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("FigureImageRemove");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("ImageFigureImageRemoveNode");
            string uiKey = paraList[0];

            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                StageRenderManager.I.FigureImageRemove(uiKey, false);
            } else {
                StageRenderManager.I.FigureImageRemove(uiKey);
            }
        }

    }
}