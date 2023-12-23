public enum DefeatReason
{
    Fall,
    Timeout,
    Detect,
}

public enum EnemyType
{
    Idle,
    IdleFOV,
    Patrol,
    PatrolJump,
    Chase,
    ChaseJump,
}

public enum EasingType
{
    Linear = 1,
    InSine,
    OutSine,
    InOutSine,
    InQuad,
    OutQuad,
    InOutQuad,
    InCubic,
    OutCubic,
    InOutCubic,
    InQuart,
    OutQuart,
    InOutQuart,
    InQuint,
    OutQuint,
    InOutQuint,
    InExpo,
    OutExpo,
    InOutExpo,
    InCirc,
    OutCirc,
    InOutCirc,
    InElastic,
    OutElastic,
    InOutElastic,
    InBack,
    OutBack,
    InOutBack,
    InBounce,
    OutBounce,
    InOutBounce,
    Flash,
    InFlash,
    OutFlash,
    InOutFlash,
    Custom
}

public enum GameStatus
{
    Playing,
    Defeat,
    Victory,
    MainScreen,
    Shop,
    
}

public enum TypeSound
{
    None,
    Button,
	OpenPanel,
	ClosePanel,
}

public enum VictoryAnimType
{
    BellyDance,
    ChickenDance,
    HiphopDance,
    MacarenaDance,
    RobotDance,
    SlideDance,
    SnakeDance,
    TutHiphopDance,
    TwistDance,
    WaveHiphopDance,
    YmcaDance,
    
}