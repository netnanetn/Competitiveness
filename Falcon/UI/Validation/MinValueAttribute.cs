using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Falcon.UI.Validation
{
    public class MinValueAttribute : ValidationAttribute
    {
        private readonly int _minValue;

        public MinValueAttribute(int maxValue)
        {
            _minValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return (int) value >= _minValue;
        }
    }
}
