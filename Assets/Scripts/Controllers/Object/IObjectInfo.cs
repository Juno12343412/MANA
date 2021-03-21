using MANA.Enums;

namespace UDBase.Controllers.ObjectSystem {

    /// <summary>
    /// 오브젝트의 기본정보
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// 오브젝트의 이름
        /// </summary>
        string     Name { get; }
        
        /// <summary>
        /// 오브젝트의 종류
        /// </summary>
        ObjectKind Kind { get; }

        /// <summary>
        /// 오브젝트 죽이는 함수
        /// </summary>
        void Kill();

        /// <summary>
        /// 오브젝트가 죽었는가 ?
        /// </summary>
        /// <returns></returns>
        bool IsDead();
    }
}