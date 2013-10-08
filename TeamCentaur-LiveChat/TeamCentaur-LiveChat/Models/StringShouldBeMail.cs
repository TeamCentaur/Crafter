using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TeamCentaur_LiveChat.Models
{
    public class StringShouldBeMail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string valueAsString = value as string;

            if (valueAsString == null)
            {
                return true;
            }

            if (Regex.IsMatch(valueAsString, @"^\w+@[a-zA-Z]+?\.[a-zA-Z]{2,4}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}