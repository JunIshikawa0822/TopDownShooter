namespace TopDownShooter.Timing
{
    using TopDownShooter.Core.Timing;

    public sealed class GameClocks
    {
        // TimeGroup の並び順は「親が先」。UnityTimeDriver はこの順に進めるので、子は進めた後の親の倍率を使える
        private readonly TimeDomain[] _domains;

        public GameClocks(int requestCapacity)
        {
            _domains = new TimeDomain[System.Enum.GetValues(typeof(TimeGroup)).Length];

            TimeDomain real = Create(TimeGroup.Real, null, requestCapacity);
            Create(TimeGroup.UI, real, requestCapacity);
            TimeDomain game = Create(TimeGroup.Game, real, requestCapacity);
            Create(TimeGroup.Player, game, requestCapacity);
            Create(TimeGroup.Enemies, game, requestCapacity);
            Create(TimeGroup.Projectiles, game, requestCapacity);
            Create(TimeGroup.Environment, game, requestCapacity);
        }

        public TimeDomain Get(TimeGroup group)
        {
            return _domains[(int)group];
        }

        private TimeDomain Create(TimeGroup group, TimeDomain parent, int requestCapacity)
        {
            TimeDomain domain = new TimeDomain(group.ToString(), parent, requestCapacity);
            _domains[(int)group] = domain;
            return domain;
        }
    }
}
