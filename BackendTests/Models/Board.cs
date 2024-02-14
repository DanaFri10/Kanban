using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.BackendTests
{
    public class Board
    {
        public int BoardID { get; }
        public string BoardName { get; }
        public string BoardOwner { get; }
        public List<Column> Columns { get; }

        public Board(int boardID, string boardName, string boardOwner, List<Column> columns)
        {
            BoardID = boardID;
            BoardName = boardName;
            BoardOwner = boardOwner;
            Columns = columns;
        }
    }
}
