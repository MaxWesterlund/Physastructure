using Raylib_cs;

public static class Draw {
    public static void DrawPheremoneMap(float[,] PheremoneMap) {
        for (int y = 0; y < Settings.PxlAmnt; y++) {
            for (int x = 0; x < Settings.PxlAmnt; x++) {
                float avg = 0;
                for (int y2 = 0; y2 < Settings.Res; y2++) {
                    for (int x2 = 0; x2 < Settings.Res; x2++) {
                        int xCoord = x * Settings.Res + x2;
                        int yCoord = y * Settings.Res + y2;
                        avg += PheremoneMap[xCoord, yCoord];
                    }
                }
                avg /= Settings.Res * Settings.Res;
                int r = (int)(avg * 255);

                Color color = new Color(r, 0, 0, 255);

                Raylib.DrawRectangle(x * Settings.PxlPosScaling, y * Settings.PxlPosScaling, Settings.Ratio * 2, Settings.Ratio * 2, color);
            }
        }
    }

    public static void DrawDebug() {
        Raylib.DrawText("FPS: " + Raylib.GetFPS().ToString(), 10, 10, 20, Color.WHITE);
    }
}