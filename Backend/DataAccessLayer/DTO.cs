using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// Generic DTO object
    /// </summary>
    public abstract class DTO
    {
        protected DalController _dalController;

        /// <summary>
        /// Abstract function that inserts the instance of the DTO to DB.
        /// </summary>
        public abstract void Insert();

        /// <summary>
        /// abstract function that deletes the instance of the DTO from DB.
        /// </summary>
        public abstract void Delete();

        /// <summary>
        /// Constructor that gets DalController and initializes the field.
        /// </summary>
        /// <param name="controller">DalController to the specific DTO.</param>
        protected DTO(DalController controller)
        {
            _dalController = controller;
        }
    }
}
