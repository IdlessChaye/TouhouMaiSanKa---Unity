using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdlessChaye.IdleToolkit.AVGEngine {
    public class ScriptSentenceContext {
        private int index = -1;
        private string[] tokens;
        private string currentToken;
        public string CurrentToken => currentToken;

        private bool isCorrect = false;
        public bool IsCorrect => isCorrect;

        public ScriptSentenceContext(string[] tokens) {
            isCorrect = Process(tokens);
            if(isCorrect) {
                this.tokens = tokens;
                NextToken();
            }
        }


        public bool IsOver() {
            return index >= tokens.Length;
        }

        public string NextToken() {
            if (IsOver()) {
                currentToken = null;
                return null;
            }
            index++;
            if (IsOver() == false) {
                currentToken = tokens[index];
            } else {
                currentToken = null;
            }
            return currentToken;
        }

        public string GetNextToken() {
            int nextIndex = index + 1;
            if (nextIndex < tokens.Length) {
                return tokens[nextIndex];
            } else {
                return null;
            }
        }

        public void SkipToken(string tokenName) {
            if (tokenName == null || IsOver() == true || tokenName.Equals(currentToken) == false) {
                throw new System.Exception("ERRER IN SkipToken!");
            }
            NextToken();
        }

        private bool Process(string[] tokens) {
            return true;
        }

    }

}