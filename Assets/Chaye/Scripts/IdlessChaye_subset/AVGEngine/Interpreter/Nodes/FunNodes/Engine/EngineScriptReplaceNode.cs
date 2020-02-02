using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class EngineScriptReplaceNode : FunNode {
        public EngineScriptReplaceNode() {
            
        }

        public override void Interpret(ScriptSentenceContext context) {
            //context.SkipToken("Initial");
            InterpretPart(context);
        }

        protected override void OnLateUpdate() {
            throw new System.NotImplementedException();
        }

        protected override void OnUpdateEngineState() {
            throw new System.NotImplementedException();
        }

        protected override void OnUpdateStageContext() {
            throw new System.NotImplementedException();
        }

        protected override void OnUpdateStageRender() {
            throw new System.NotImplementedException();
        }
    }
}