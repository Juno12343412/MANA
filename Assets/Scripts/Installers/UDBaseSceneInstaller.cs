using UnityEngine;
using UDBase.UI.Common;

namespace UDBase.Installers {
	public class UDBaseSceneInstaller : UDBaseInstallers {

		public UIManager.Settings UISettings;

		public void AddUIManager(UIManager.Settings settings) {
			Container.BindInstance(settings);
			Container.Bind<UIManager>().FromNewComponentOnNewGameObject().AsSingle();
		}

		public override void InstallBindings() {
			AddUIManager(UISettings);
		}
	}
}