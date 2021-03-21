namespace UDBase.Controllers.SceneSystem {

	/// <summary>
	/// IScene은 씬을 로드하는 여러가지 방법을 제공함
	/// 이름별로 - 가장 간단한 방법
	/// 특정 유형및 매개변수 별로 - 특정 유형 및 유연한 이동이 가능함
	/// 예를 들어 다음과 같은 장면 구조(또는 훨씬 더 복잡한 장면 구조)가 있음:
	/// - MainMenu (로드된 이름)
	/// Level_1, Level_2, .., Level_N (특정 유형및 매개변수)
	/// 커스텀 클래스/구조는 ISceneInfo에서 상속하고 사용자 지정 논리와 함께 Name 속성을 구현함
	/// 사용할 수 있는 또 다른 간단한 옵션은 Scene {MainMenu, Level}과 같은 열거형이며, 두 가지 경우 모두 사용할 수 있음
	/// LoadScene(Scenes.MainMenu) => loads "MainMenu"
	/// LoadScene(Scenes.Level, "1") => loads "Level_1"
	/// </summary>
	public interface IScene {

		/// <summary>
		/// 현재 로드된 씬 정보
		/// </summary>
		ISceneInfo CurrentScene { get; }

		/// <summary>
		/// 특정 정보별로 씬 로드
		/// </summary>
		void LoadScene(ISceneInfo sceneInfo);

		/// <summary>
		/// 이름별로 씬 로드
		/// </summary>
		void LoadScene(string sceneName);

		/// <summary>
		/// 커스텀 타입별로 씬 로드
		/// </summary>
		void LoadScene<T>(T type);

		/// <summary>
		/// 커스텀 타입과 매개변수별로 씬 로드
		/// </summary>
		void LoadScene<T>(T type, string param);

		/// <summary>
		/// 커스텀 타입과 매개변수들로 씬 로드
		/// </summary>
		void LoadScene<T>(T type, params string[] parameters);

		/// <summary>
		/// 현재 씬을 재실행
		/// </summary>
		void ReloadScene();
	}
}
