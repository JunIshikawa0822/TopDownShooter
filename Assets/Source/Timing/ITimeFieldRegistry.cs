namespace TopDownShooter.Timing
{
    public interface ITimeFieldRegistry
    {
        void Register(TimeField field);

        void Unregister(TimeField field);
    }
}
