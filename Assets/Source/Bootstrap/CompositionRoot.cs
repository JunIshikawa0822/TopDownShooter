namespace TopDownShooter.Bootstrap
{
    using System;

    public sealed class CompositionRoot
    {
        // 依存されるものから順に new し、作った順に SystemRunner へ追加する。追加順がそのまま Update の実行順になる
        public void Compose(SystemRunner systemRunner)
        {
            throw new NotImplementedException();
        }
    }
}
