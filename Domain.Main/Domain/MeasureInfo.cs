namespace Domain.Main.Domain
{
    public class MeasureInfo
    {
        public string Id { get; set; }
        
        public int ToolType { get; set; }

        public string ToolId { get; set; }

        public string StepId { get; set; }

        public string RepairId { get; set; }

        public string TargetValue { get; set; }

        public string RealValue { get; set; }

        public string JobResult { get; set; }

        public string UserId { get; set; }

        public int SyncStatus { get; set; }
    }
}
