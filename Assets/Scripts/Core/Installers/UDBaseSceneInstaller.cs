using UnityEngine;
using UDBase.UI.Common;
using UDBase.Controllers.ObjectSystem;
using UDBase.Controllers.ParticleSystem;

namespace UDBase.Installers {
	public class UDBaseSceneInstaller : UDBaseInstallers {

		public UIManager.Settings UISettings;
		public PlayerManager.Stats PlayerStats;
		public ParticleManager.Settings ParticleSettings;

		public void AddUIManager(UIManager.Settings settings) {
			Container.BindInstance(settings);
			Container.Bind<UIManager>().FromNewComponentOnNewGameObject().AsSingle();
		}

		public void AddPlayerManager(PlayerManager.Stats stats) {
			Container.BindInstances(stats);
			Container.Bind<PlayerManager>().FromNewComponentOnNewGameObject().AsSingle();
        }

		public void AddParticleManager(ParticleManager.Settings settings) {
			Container.BindInstances(settings); 
			Container.Bind<ParticleManager>().FromNewComponentOnNewGameObject().AsSingle();
		}

		public override void InstallBindings() {
			AddUIManager(UISettings);
			AddPlayerManager(PlayerStats);
			AddParticleManager(ParticleSettings);
		}
	}
}