namespace CabinetMgr.RtDelegate
{
    public class DelegateReturnRecord
    {
        public delegate void RefreshHistoryGridDelegate();

        public static RefreshHistoryGridDelegate RefreshHistoryGrid = null;
    }
}
