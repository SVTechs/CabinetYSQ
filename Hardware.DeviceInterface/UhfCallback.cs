namespace Hardware.DeviceInterface
{
    public class UhfCallback
    {
        public delegate void OnCardFoundDelegate(int comIndex, string cardId);
        public static OnCardFoundDelegate OnCardFound = null;

        public delegate void OnCardTakenDelegate(int comIndex, string cardId);
        public static OnCardTakenDelegate OnCardTaken = null;

        public delegate void OnCardReturnDelegate(int comIndex, string cardId);
        public static OnCardReturnDelegate OnCardReturn = null;
    }
}
