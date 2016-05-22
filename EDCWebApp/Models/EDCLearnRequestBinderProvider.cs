using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.ModelBinding;

namespace EDCWebApp.Models
{
    public class EDCLearnRequestModelBinder : IModelBinder
    {
        public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = actionContext.Request;

                var context = request.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject(context, bindingContext.ModelType);
                bindingContext.Model = obj;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
    public class EDCLearnRequestBinderProvider : ModelBinderProvider
    {
        public override System.Web.Http.ModelBinding.IModelBinder GetBinder(System.Web.Http.HttpConfiguration configuration, Type modelType)
        {
            return new EDCLearnRequestModelBinder();
        }

    }
}
