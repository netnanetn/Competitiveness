using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Util;
using System.Web;

namespace Falcon.Mvc
{
    public class FalconRequestValidation : RequestValidator
    {
        public FalconRequestValidation()
        {

        }

        protected override bool IsValidRequestString(
            HttpContext context, string value,
            RequestValidationSource requestValidationSource, string collectionKey,
            out int validationFailureIndex)
        {
            validationFailureIndex = -1;  //Set a default value for the out parameter.

            //This application does not use RawUrl directly so you can ignore the check.
            if (requestValidationSource == RequestValidationSource.RawUrl)
                return true;

            //Bỏ chặn các trường Option theo danh mục của Rao Vặt & Sản Phẩm
            if ((requestValidationSource == RequestValidationSource.QueryString) &&
                (collectionKey.StartsWith("__option_")))
            {                
                return true;                
            }
            //Còn lại check như bình thường
            else
            {
                return base.IsValidRequestString(context, value, requestValidationSource,
                                                 collectionKey, out validationFailureIndex);
            }
        }
    }
}
