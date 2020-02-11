using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageBackgroundImageChangeBlackNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("BackgroundImageChangeBlack");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("ImageBackgroundImageChangeBlackNode");
            StageRenderManager.I.BackgroundImageChangeBlack();
        }

    }
}