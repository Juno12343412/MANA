using UnityEngine;
using UDBase.Installers;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;

namespace UDBase.Installers {
    public sealed class MonoController : UDBaseInstallers {
        
        [SerializeField] private UnityLog.Settings _logSettings = new UnityLog.Settings();
        [SerializeField] private AsyncSceneLoader.Settings _sceneSettings = new AsyncSceneLoader.Settings();

        public sealed override void InstallBindings() {
            // Log
            //AddEmptyLogger();
            AddUnityLogger(_logSettings);
            // Log

            // Event
            AddEvents();
            // Event

            // Scene
            AddAsyncSceneLoader(_sceneSettings);
            // Scene
        }
    }
}
