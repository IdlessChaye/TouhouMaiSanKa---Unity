using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class TextClearNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Clear");
            InterpretPart(context);
        }

        protected override void OnUpdateEngineState() {

        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 0)
                throw new System.Exception("TextClearNode");
            StageRenderManager.I.TextClear();
        }

        protected override void OnUpdateStageRender() {

        }

        protected override void OnLateUpdate() {

        }

    }
}