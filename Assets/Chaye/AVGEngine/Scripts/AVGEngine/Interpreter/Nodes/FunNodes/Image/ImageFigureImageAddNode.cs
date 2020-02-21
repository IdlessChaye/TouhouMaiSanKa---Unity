using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ImageFigureImageAddNode : FunNode {
        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("FigureImageAdd");
            InterpretPart(context);
        }

        protected override void OnUpdateStageContext() {
            if (paraList.Count != 4 && paraList.Count != 6)
                throw new System.Exception("ImageFigureImageAddNode");
            string uiKey = paraList[0];
            string index = paraList[1];
            float pos_x = float.Parse(paraList[2]);
            float pos_y = float.Parse(paraList[3]);
            float scale_x;
            float scale_y;
            StateBuff stateBuff = PachiGrimoire.I.StateMachine.StateBuff;
            if (stateBuff == StateBuff.Next) {
                if (paraList.Count == 4) {
                    StageRenderManager.I.FigureImageAdd(uiKey, index, pos_x, pos_y, 1f, 1f, false);
                } else if (paraList.Count == 6) {
                    scale_x = float.Parse(paraList[4]);
                    scale_y = float.Parse(paraList[5]);
                    StageRenderManager.I.FigureImageAdd(uiKey, index, pos_x, pos_y, scale_x, scale_y, false);
                }
            } else {
                if (paraList.Count == 4) {
                    StageRenderManager.I.FigureImageAdd(uiKey, index, pos_x, pos_y);
                } else if (paraList.Count == 6) {
                    scale_x = float.Parse(paraList[4]);
                    scale_y = float.Parse(paraList[5]);
                    StageRenderManager.I.FigureImageAdd(uiKey, index, pos_x, pos_y, scale_x, scale_y);
                }
            }
        }

    }
}