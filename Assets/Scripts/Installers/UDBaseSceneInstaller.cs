using UnityEngine;
using UDBase.UI.Common;
using UDBase.Controllers.ObjectSystem;

namespace UDBase.Installers {
	public class UDBaseSceneInstaller : UDBaseInstallers {

		public UIManager.Settings UISettings;
		public PlayerManager.Stats PlayerStats;

		public void AddUIManager(UIManager.Settings settings) {
			Container.BindInstance(settings);
			Container.Bind<UIManager>().FromNewComponentOnNewGameObject().AsSingle();
		}

		public void AddPlayerManager(PlayerManager.Stats stats) {
			Container.BindInstances(stats);
			Container.Bind<PlayerManager>().FromNewComponentOnNewGameObject().AsSingle();
        }

		public override void InstallBindings() {
			AddUIManager(UISettings);
			AddPlayerManager(PlayerStats);
		}
	}
}