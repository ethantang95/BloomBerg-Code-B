using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Parser {
    public abstract class ParserAbstract<T> {
        protected Stack<string> _contents;

        public ParserAbstract(string toParse) {
            toParse = toParse.TrimEnd(Environment.NewLine.ToCharArray());
            _contents = new Stack<string>(toParse.Split(' ').Reverse().Where(s => !string.IsNullOrWhiteSpace(s)));
        }

        protected bool PopIfMessage(string message) {
            if (_contents.Peek() == message) {
                _contents.Pop();
                return true;
            }
            return false;
        }

        protected double PopDouble() {
            return double.Parse(_contents.Pop());
        }

        protected int PopInt() {
            return int.Parse(_contents.Pop());
        }

        protected string PopString() {
            return _contents.Pop();
        }

        protected bool HaveContents() {
            return _contents.Count > 0;
        }

        public abstract T GetModel();
    }
}