using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class CommandNode : BaseInterpreterNode {
        private BaseInterpreterNode node;

        public override void Interpret(ScriptSentenceContext context) {
            string commandToken = context.CurrentToken;
            if (!CanParse(commandToken, context))
                throw new System.Exception("ERROR IN CommandNode! NOT DEFINDED!" + commandToken);
        }

        public override void Execute() {
            node.Execute();
        }

        private bool CanParse(string commandToken, ScriptSentenceContext context) {
            bool canParse = true;

            if (commandToken.Equals("Engine")) {
                node = new CommandEngineNode();
            } else if (commandToken.Equals("Music")) {
                node = new CommandMusicNode();
            } else if (commandToken.Equals("Text")) {
                node = new CommandTextNode();
            } else if (commandToken.Equals("Image")) {
                node = new CommandImageNode();
            } else {
                canParse = false;
            }

            if (canParse) {
                Debug.Log("CommandNode CanParse commandToken: " + commandToken);
                node.Interpret(context);
            } else {
                Debug.Log("CommandNode FALSE canParse! token :" + commandToken);
            }

            return canParse;
        }
    }
}