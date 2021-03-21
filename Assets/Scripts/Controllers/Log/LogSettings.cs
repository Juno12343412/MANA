using System;
using System.Collections.Generic;
using UnityEngine;

namespace UDBase.Controllers.LogSystem {

	/// <summary>
	/// 정의된 노드에서 특정 로그를 표시해야됨 context 또는 not
	/// </summary>
	[Serializable]
	public class LogNode {
		/// <summary>
		/// 현재 로그 컨텍스트
		/// </summary>
		[Tooltip("현재 로그 컨텍스트")]
		//[ClassImplements(typeof(ILogContext))]
		public ILogContext Context;

		/// <summary>
		/// 현재 컨텍스트에서 로그 사용이 가능한가 ?
		/// </summary>
		[Tooltip("현재 컨텍스트에서 로그 사용이 가능한가 ?")]
		public bool Enabled;
	}

	/// <summary>
	/// 기본 로그 셋팅
	/// </summary>
	[Serializable]
	public class CommonLogSettings {
		/// <summary>
		/// 기본적으로 모든 컨텍스트에 대한 로깅이 활성화되어 있나?
		/// </summary>
		[Tooltip("기본적으로 모든 컨텍스트에 대한 로깅이 활성화되어 있나?")]
		public bool EnabledByDefault = false;

		/// <summary>
		/// 특정 활성화 상태의 컨텍스트
		/// </summary>
		[Tooltip("특정 활성화 상태의 컨텍스트")]
		//[OneLine]
		public List<LogNode> Nodes = new List<LogNode>();

		internal bool IsContextEnabled(ILogContext context) {
			var contextType = context.GetType();
			foreach ( var node in Nodes ) {
				if ( (Type)node.Context == contextType ) {
					return node.Enabled;
				}
			}
			return EnabledByDefault;
		}
	}
}
