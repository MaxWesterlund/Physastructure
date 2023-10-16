public static class Map {
    public static float[,] PheremoneMap = new float[Settings.Size, Settings.Size];
    public static bool[,] DiscoveredMap = new bool[Settings.Size, Settings.Size];
    public static bool[,] OccupiedMap = new bool[Settings.Size, Settings.Size];

    public static void Decay() {
        for (int y = 0; y < Settings.Size; y++) {
            for (int x = 0; x < Settings.Size; x++) {
                PheremoneMap[x, y] *= Settings.DecayRate;
            }
        }
    }
}