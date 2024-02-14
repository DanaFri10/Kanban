using IntroSE.Kanban.Frontend;

namespace Frontend.Model
{
    public abstract class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        protected NotifiableModelObject(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
