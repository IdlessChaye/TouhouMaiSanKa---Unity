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
            StageRenderManager.I.SmallFigureImageClear();
        }

    }
}