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
    ShopAnimVictory,
    
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
    ArmsHipHopDance,
    Dancing1,
    DancingMaraschinoStep,
    GangnamStyle,
    HousingDance,
    HousingDancing1,
    HousingDancing2,
    JazzDancing,
    NsDance,
    NsSpinCombo,
    SalsaDancing,
    SambaDancing,
    Shuffing,
    StandardDancing,
    SwingDancing,
    SwingDancing1,
    TutDancing,
    WaveDancing,
}

public enum MainAnimType
{
    SlowRun,
    MediumRun,
    FastRun,
    AirSquat,
    BicepCurl,
    Burpee,
    DrunkBackward,
    DunkForward,
    FrontRaises,
    JumpingJacks,
    RunBackward,
    RunBackward1,
    RunForward,
    RunForward1,
    RunLockBack,
    Running,
    Running1,
    Running2,
    RunningBackward,
    SumoHighPull,
}

public enum CharacterRunStatus
{
    Normal,
    Sprint,
    Jog,
}

public enum ItemType
{
    Invisible,
    Shield,
    Knockout,
    Trap,
    Recovery,
    Consume,
}

public enum UpgradeType
{
    Speed,
    Fitness,
    RecoveryFitness,
    ConsumeFitness,
    MoneyIncome,
}

public enum UpgradeValueType
{
    Int, 
    Float,
    
}

public enum RewardType
{
    Coin,
    Gift
}