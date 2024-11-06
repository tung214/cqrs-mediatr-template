using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laborie.Service.Domain.Entities.Mongo.Laborie
{
    public class LaborieProductImage
    {
        public required string Url { get; set; }
        public string? Alt { get; set; }
    }
}