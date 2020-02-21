using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class CommandImageNode : BaseInterpreterNode {
        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Image");
            while (true) {
                string funcToken = context.CurrentToken;
                if (funcToken == null)
                    break;

                if (!CanParse(funcToken, context))
                    break;
            }
        }

        public override void Execute() {
            for (int i = 0; i < nodeList.Count; i++) {
                BaseInterpreterNode node = nodeList[i];
                node.Execute();
            }
        }

        private bool CanParse(string token, ScriptSentenceContext scriptSentenceContext) {
            bool canParse = true;

            BaseInterpreterNode node = null;
            if (token.Equals("BackgroundImageChange")) {
                node = new ImageBackgroundImageChangeNode();
            } else if (token.Equals("BackgroundImageClear")) {
                node = new ImageBackgroundImageClearNode();
            } else if (token.Equals("ConsoleShow")) {
                node = new ImageConsoleShowNode();
            } else if (token.Equals("ConsoleHide")) {
                node = new ImageConsoleHideNode();
            } else if (token.Equals("SmallFigureImageChange")) {
                node = new ImageSmallFigureImageChangeNode();
            } else if (token.Equals("SmallFigureImageClear")) {
                node = new ImageSmallFigureImageClearNode();
            } else if (token.Equals("FigureImageAdd")) {
                node = new ImageFigureImageAddNode();
            } else if (token.Equals("FigureImageRemove")) {
                node = new ImageFigureImageRemoveNode();
            } else {
                canParse = false;
            }

            if (canParse) {
                nodeList.Add(node);
                node.Interpret(scriptSentenceContext);
            } else {
                Debug.Log("CommandImageNode FALSE canParse! token :" + token);
            }

            return canParse;
        }

    }
}