using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Falcon.UI.Validation
{
    public class EmailAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public EmailAttribute() :
            base(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$") { }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "validemail"
            };
        }
    }
}
