using System;

namespace Domain.ShangHaiDevice1.Domain
{
    public class ToolPurchaseInfo
    {
        public virtual string Id { get; set; }
        public virtual string RequestNo { get; set; }
        public virtual string ToolName { get; set; }
        public virtual string ToolSpec { get; set; }
        public virtual int ToolCount { get; set; }
        public virtual string CabinetName { get; set; }
        public virtual string RequesterName { get; set; }
        public virtual DateTime RequestTime { get; set; }
        public virtual int RequestStatus { get; set; }
    }
}
