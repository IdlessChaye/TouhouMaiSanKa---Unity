using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class CommandMusicNode : BaseInterpreterNode {
        private List<BaseInterpreterNode> nodeList = new List<BaseInterpreterNode>();

        public override void Interpret(ScriptSentenceContext context) {
            context.SkipToken("Music");
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
            if (token.Equals("VoicePlay")) {
                node = new MusicVoicePlayNode();
            } else if (token.Equals("VoiceStop")) {
                node = new MusicVoiceStopNode();
            } else if (token.Equals("BGMPlay")) {
                node = new MusicBGMPlayNode();
            } else if (token.Equals("BGMPlay")) {
                node = new MusicBGMStopNode();
            } else {
                canParse = false;
            }

            if (canParse) {
                nodeList.Add(node);
                node.Interpret(scriptSentenceContext);
            } else {
                Debug.Log("CommandMusicNode FALSE canParse! token :" + token);
            }

            return canParse;
        }

    }
}