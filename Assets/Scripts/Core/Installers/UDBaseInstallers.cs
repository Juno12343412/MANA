using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.TimeSystem;
using Zenject;

namespace UDBase.Installers {
	public abstract class UDBaseInstallers : MonoInstaller {

		public void AddEmptyLogger() {
			Container.Bind<ILog>().To<EmptyLog>().AsSingle();
		}

		public void AddUnityLogger(UnityLog.Settings settings) {
			Container.BindInstance(settings);
			Container.Bind<ILog>().To<UnityLog>().AsSingle();
		}

		public void AddEvents() {
			Container.Bind<IEvent>().To<EventController>().AsSingle();
		}

		public void AddDirectSceneLoader() {
			Container.Bind<IScene>().To<DirectSceneLoader>().AsSingle();
		}

		public void AddAsyncSceneLoader(AsyncSceneLoader.Settings settings) {
			Container.Bind<AsyncLoadHelper>().FromNewComponentOnNewGameObject().AsSingle();
			Container.BindInstance(settings);
			Container.Bind<IScene>().To<AsyncSceneLoader>().AsSingle();
		}

		public void AddLocalTime() {
			Container.Bind<ITime>().To<LocalTime>().AsSingle();
		}

	}
}
