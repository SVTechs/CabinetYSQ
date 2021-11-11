namespace Domain.ServerMain.Domain
{
    public class CabinetInfo
    {

        public virtual string Id { get; set; }

        public virtual string CabinetName { get; set; }

        public virtual string CabinetAlias { get; set; }

        public virtual int? CabinetOrder { get; set; }
    }
}
