using System.Web.Mvc;

namespace Falcon.Mvc
{
    public class BaseModel
    {
        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }
    }
    public class BaseEntityModel : BaseModel
    {
        public virtual int Id { get; set; }
    }
}
