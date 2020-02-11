using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageBackgroundImageChangeNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("BackgroundImageChange");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 1)
                throw new System.Exception("ImageBackgroundImageChangeNode");
            string index = paraList[0];
            StageRenderManager.I.BackgroundImageChange(index);
        }

    }
}