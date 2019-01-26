namespace Interactive
{
    public interface IHands
    {
        void GrabWithLeftHand(Pickup pickup);
        void GrabWithRightHand(Pickup pickup);
        void GrabWithBothHands(Pickup pickup);
    }
}