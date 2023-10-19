public static class Settings {
    public const int ScrnSize = 800;
    public const int Size = 200; // Can't be larger than ScrnSize.
    public const int Res = 1;

    public const int PxlAmnt = Size / Res;
    public const int Ratio = ScrnSize / Size;
    public const int PxlPosScaling = Res * Ratio;

    public const float DecayRate = 0.99f;

    public const int AgentAmnt = 100;
    public const float AgentSpeed = 1f;
    public const float AgentTurnRad = 0.5f;
    public const int AgentSensorDist = 10;
}