using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreMims.DataValidation
{
    public class RangString : ValidationAttribute
    {
        private double minvalue;
        private double maxvalue;
        private string msg;

        public RangString(double min, double max, string errormeg = "")
        {
            minvalue = min;
            maxvalue = max;
            msg = errormeg;
            if (string.IsNullOrEmpty(errormeg))
            {
                msg = "输入长度介于" + minvalue + "与" + max + "之间";
            }
        }

        public override bool IsValid(object value)
        {
            //return base.IsValid(value);
            string tr = null;
            if (Double.TryParse(value.ToString(), out tr))
            {
                if (tr.length > minvalue && tr.length < maxvalue) return true;
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
