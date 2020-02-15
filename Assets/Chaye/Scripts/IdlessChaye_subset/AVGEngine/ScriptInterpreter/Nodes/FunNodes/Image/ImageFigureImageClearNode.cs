using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageFigureImageClearNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("FigureImageClear");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("ImageFigureImageClearNode");
            StageRenderManager.I.FigureImageClear();
        }

    }
}