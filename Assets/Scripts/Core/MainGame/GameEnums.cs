namespace MANA.Enums {
    
    /// <summary>
    /// 이 게임의 오브젝트들의 종류
    /// </summary>
    public enum ObjectKind : byte {
        
        Player,
        NPC, Item, Obstacle,
        Enemy, Boss,
        NONE = 99
    }

    /// <summary>
    /// 플레이어의 상태 종류
    /// </summary>
    public enum PlayerState : byte {
        
        Idle,
        Walk, Run, Jump, Talk,
        Attack, SpecialAttack,
        Dead, Option,
        NONE = 99
    }

    /// <summary>
    /// AI의 상태 종류
    /// </summary>
    public enum AIState : byte {
    
        Idle,
        Patrol, Track,
        Attack,
        Dead,
        NONE = 99
    }

    /// <summary>
    /// 아이템의 상태 종류
    /// </summary>
    public enum ItemState : byte {
    
        Idle,
        Dead,
        NONE = 99
    }

    /// <summary>
    /// 아이템의 종류
    /// </summary>
    public enum ItemKind : byte {

        Test,
        NONE = 99
    }

    /// <summary>
    /// 파괴가능 오브젝트 종류
    /// </summary>
    enum DestructKind : byte
    {
        DestructibleX,
        DestructibleY,
        Grass,
        Explosive
    }

    /// <summary>
    /// 파티클 오브젝트 종류
    /// </summary>
    public enum ParticleKind : byte
    {
        Move,
        Hit, Obs, Obs2,
        Explosion,
        Dash, Dash2,
        NONE = 99
    }
}