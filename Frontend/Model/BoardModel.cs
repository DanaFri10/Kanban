using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        public int BoardID { get; set; }
        public string BoardName { get; set; }
        public string BoardOwner { get; set; }
        public int TasksCount { get; set; }
        public ObservableCollection<ColumnModel> Columns { get; set; }


        /// <summary>
        /// Constructor for BoardModel
        /// </summary>
        /// <param name="controller">The backend controller.</param>
        /// <param name="boardID">The board's ID.</param>
        /// <param name="boardName">The board's name.</param>
        /// <param name="boardOwner">The board's owner.</param>
        /// <param name="tasksCount">The amount of tasks in the board.</param>
        public BoardModel(BackendController controller, int boardID, string boardName, string boardOwner, int tasksCount, ObservableCollection<ColumnModel> columns) : base(controller)
        {
            BoardID = boardID;
            BoardName = boardName;
            BoardOwner = boardOwner;
            TasksCount = tasksCount;
            Columns = columns;
        }

    }
}
