namespace TopDownShooter.Bootstrap
{
    using UnityEngine;

    public sealed class GameMain : MonoBehaviour
    {
        private readonly SystemRunner _systemRunner = new();

        private void Awake()
        {
            CompositionRoot compositionRoot = new CompositionRoot();
            compositionRoot.Compose(_systemRunner);
        }

        private void Start()
        {
            _systemRunner.Initialize();
        }

        private void Update()
        {
            _systemRunner.PreUpdate();
            _systemRunner.Update();
        }

        private void LateUpdate()
        {
            _systemRunner.LateUpdate();
        }

        private void OnDestroy()
        {
            _systemRunner.Dispose();
        }
    }
}
