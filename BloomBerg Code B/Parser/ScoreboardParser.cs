using BloomBerg_Code_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloomBerg_Code_B.Parser {
    public class ScoreboardParser : ParserAbstract<ScoreboardModel> {

        public ScoreboardParser(string toParse) : base(toParse) {
        }

        public override ScoreboardModel GetModel() {
            PopIfMessage("SCOREBOARD_OUT");
            var scoreBoardList = new List<ScoreboardEntry>();
            while (HaveContents()) {
                var entry = new ScoreboardEntry(PopString(), PopInt(), PopInt());
                scoreBoardList.Add(entry);
            }

            scoreBoardList.Sort();

            var model = new ScoreboardModel(scoreBoardList);
            return model;
        }
    }
}