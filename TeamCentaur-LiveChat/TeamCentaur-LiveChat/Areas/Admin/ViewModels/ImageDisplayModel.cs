using Crafter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TeamCentaur_LiveChat.Areas.Admin.ViewModels
{
    public class ImageDisplayModel
    {
        public static Expression<Func<Image, ImageDisplayModel>> FromImage
        {
            get
            {
                return image => new ImageDisplayModel
                {
                    Id = image.Id,
                    ImagePath = image.ImagePath
                };
            }
        }

        public int Id { get; set; }

        public string ImagePath { get; set; }
    }
}
