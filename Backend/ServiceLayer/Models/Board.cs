using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Board
    {
        public int BoardID { get; set; }
        public string BoardName { get; set; }
        public string BoardOwner { get; set; }
        public List<Column> Columns { get; set; }


        public Board() { }

        public Board(int boardID, string boardName, string boardOwner, List<Column> columns)
        {
            BoardID = boardID;
            BoardName = boardName;
            BoardOwner = boardOwner;
            Columns = columns;
        }

        public Board(BusinessLayer.Board b)
        {
            BoardID = b.BoardID;
            BoardName = b.BoardName;
            BoardOwner = b.BoardOwner;
            Columns = new List<Column>();
            for (int i = 0; i < BusinessLayer.Board.COUNT_COLUMNS; i++)
            {
                Columns.Add(new Column(b.GetColumn(i)));

            }
        }
    }
}
