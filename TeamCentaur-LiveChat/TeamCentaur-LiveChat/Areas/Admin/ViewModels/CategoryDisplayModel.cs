using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class CategoryDisplayModel
    {
        public static Expression<Func<Category, CategoryDisplayModel>> FromCategory
        {
            get
            {
                return category => new CategoryDisplayModel
                {
                    Id = category.Id,
                    Name = category.Name
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}