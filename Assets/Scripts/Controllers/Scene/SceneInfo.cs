using System.Text;

namespace UDBase.Controllers.SceneSystem {

	/// <summary>
	/// 씬 정보 팩토리
	/// </summary>
	public static class SceneInfo {
		public static ISceneInfo Get<T>(T type) {
			return new SceneParam<T>(type, "");
		}

		public static ISceneInfo Get<T>(T type, string param) {
			return new SceneParam<T>(type, param);
		}

		public static ISceneInfo Get<T>(T type, params string[] param) {
			return new MultiSceneParam<T>(type, param);
		}
	}

	/// <summary>
	/// 기본 씬 정보 - 오직 이름만 필요.
	/// EX : MainMenu, Settings, etc.
	/// </summary>
	public struct SceneName : ISceneInfo {
		public string Name { get; private set; }

		public SceneName(string name) {
			Name = name;
		}

		public override string ToString() {
			return Name;
		}
	}

	/// <summary>
	/// 특별 정보 - 커스텀 타입 그리고 (선택 사항) 매개변수.
	/// EX : (CustomType_Param), Level_1, Level_N, etc.
	/// </summary>
	public struct SceneParam<T> : ISceneInfo {
		public string Type  { get; private set; }
		public string Param { get; private set; }
		public string Name  { get; private set; }

		public SceneParam(T type, string param) {
			Type  = type.ToString();
			Param = param;
			if( string.IsNullOrEmpty(param) ) {
				Name = Type;
			} else {
				Name = string.Format("{0}_{1}", type, param);
			}
		}
	}

	/// <summary>
	/// 멀티 씬 매개변수 - 커스텀 타입과 >1 매개변수.
	/// EX : (CustomType_Params_N1_Params_N2...), Level_Type1_1, Level_TypeN_N, etc.
	/// </summary>
	public struct MultiSceneParam<T> : ISceneInfo {
		public string   Type   { get; private set; }
		public string[] Params { get; private set; }
		public string   Name   { get; private set; }

		public MultiSceneParam(T type, params string[] param) {
			Type = type.ToString();
			Params = param;
			var sb = new StringBuilder();
			sb.Append(type);
			for( int i = 0; i < param.Length; i++ ) {
				sb.Append("_");
				sb.Append(param[i]);
			}
			Name = sb.ToString();
		}
	}
}