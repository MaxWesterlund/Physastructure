public static class Map {
    public static void Decay(ref float[,] decayMap) {
        for (int y = 0; y < Settings.Size; y++) {
            for (int x = 0; x < Settings.Size; x++) {
                decayMap[x, y] *= Settings.DecayRate;
            }
        }
    }
}