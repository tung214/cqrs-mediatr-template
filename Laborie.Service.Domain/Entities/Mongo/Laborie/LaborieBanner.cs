using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo.Laborie
{
    [BsonCollection("Laborie", "LaborieBanner")]
    public class LaborieBanner : MongoDocument
    {
        public int Order { get; set; }
        public string? Image { get; set; }
        public bool IsDelete { get; set; }
    }
}