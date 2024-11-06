using Laborie.Service.Domain.Attributes;
using Laborie.Service.Shared.Constant;

namespace Laborie.Service.Domain.Entities.Mongo.Laborie
{
    [BsonCollection("Laborie", "LaborieAgency")]
    public class LaborieAgency : MongoDocument
    {
        public required string Email { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public AgencyStatus Status { get; set; }
        /// <summary>
        /// Mã giới thiệu người khác
        /// </summary>
        public string? InviteCode { get; set; }
        /// <summary>
        /// Mã người giới thiệu
        /// </summary>
        /// <value></value>
        public string? ReferCode { get; set; }
        /// <summary>
        /// Mức hoa hồng được hưởng
        /// </summary>
        /// <value></value>
        public float? Commission { get; set; }
        public string? Password { get; set; }
        public required string Salt { get; set; }
        public string? ResetPassword { get; set; }
        public DateTime? ResetDate { get; set; }
        public int ResetCount { get; set; } = 0;
        public bool IsDelete { get; set; } = false;
    }
}