namespace TopDownShooter.Bootstrap
{
    using System;
    using System.Collections.Generic;
    using TopDownShooter.Core;

    // GameMain内でイベント関数を直接実行するのではなく、
    // SystemRunnerを介して実行することで、Systemの追加や削除、実行時のルール管理を容易にするためのクラス
    public sealed class SystemRunner : IDisposable
    {
        // 各種イベント関数内で呼ぶクラスを保持するリスト
        private readonly List<IInitializable> _initializables = new();
        private readonly List<IOnPreUpdate> _preUpdates = new();
        private readonly List<IOnUpdate> _updates = new();
        private readonly List<IOnFixedUpdate> _fixedUpdates = new();
        private readonly List<IOnLateUpdate> _lateUpdates = new();
        private readonly List<IDisposable> _disposables = new();

        public void Add(object system)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void PreUpdate()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        // Unity の FixedUpdate からではなく、UnityTimeDriver がゲームの時間に合わせて物理を1ステップ進めるたびに呼ぶ
        public void FixedUpdate()
        {
            throw new NotImplementedException();
        }

        public void LateUpdate()
        {
            throw new NotImplementedException();
        }

        // 後から作られた System ほど先に作られた System に依存しているため、逆順に破棄する
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
