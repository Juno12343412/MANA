namespace UDBase.Controllers.SceneSystem {

	/// <summary>
	/// 씬 변경 시 발생하는 이벤트
	/// </summary>
	public struct Scene_Loaded {
		public ISceneInfo SceneInfo { get; private set; }

		public Scene_Loaded(ISceneInfo sceneInfo) {
			SceneInfo = sceneInfo;

			// To Do...
		}
	}
}
