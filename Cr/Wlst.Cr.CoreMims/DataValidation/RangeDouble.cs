using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreMims.DataValidation
{
    public class RangeDouble : ValidationAttribute
    {
        private double minvalue;
        private double maxvalue;
        private string msg;

        public RangeDouble(double min, double max, string errormeg = "")
        {
            minvalue = min;
            maxvalue = max;
            msg = errormeg;
            if (string.IsNullOrEmpty(errormeg))
            {
                msg = "输入数据介于" + minvalue + "与" + max + "之间";
            }
        }

        public override bool IsValid(object value)
        {
            //return base.IsValid(value);
            double tr = 0;
            if (Double.TryParse(value.ToString(), out tr))
            {
                if (tr > minvalue && tr < maxvalue) return true;
                else
                {
                    ErrorMessage = msg;
                    return false;
                }
            }
            else
            {
                ErrorMessage = "非法数据";
                return false;
            }
        }
    }
}
