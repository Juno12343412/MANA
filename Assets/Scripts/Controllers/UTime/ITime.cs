using System;    

namespace UDBase.Controllers.TimeSystem
{
    /// <summary>
    /// 소스에서 시간을 검색하는 간단한 인터페이스
    /// </summary>
    public interface ITime {

        /// <summary>
        /// 시간이 활성화 중인가 ?
        /// </summary>
        bool IsAvailable { get; }
        
        /// <summary>
        /// 불러오기를 실패했는가 ?
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// 현재 시간
        /// </summary>
        DateTime CurrentTime { get; }
    }
}
