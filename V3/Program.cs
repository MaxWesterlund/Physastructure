using System.Security.Cryptography;
using Raylib_cs;

class Program {
    public static void Main() {
        float[,] decayMap = GenerateDecayMap();

        Raylib.InitWindow(Settings.ScrnSize, Settings.ScrnSize, "Hello World");

        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            
            DrawDecayMap(decayMap);
            DrawDebug();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    static float[,] GenerateDecayMap() {
        float[,] map = new float[Settings.Size, Settings.Size];
        for (int y = 0; y < Settings.Size; y++) {
            for (int x = 0; x < Settings.Size; x++) {
                int xDist = Settings.Size / 2 - x;
                int yDist = Settings.Size / 2 - y;
                float dist = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
                float value = Math.Clamp(1 - (dist / (Settings.Size / 2)), 0, 1);
                map[x, y] = value;
            }
        }
        return map;
    }

    static void DrawDecayMap(float[,] decayMap) {
        int ratio = Settings.ScrnSize / Settings.Size;
        for (int y = 0; y < Settings.Size / Settings.Res; y++) {
            for (int x = 0; x < Settings.Size / Settings.Res; x++) {
                float avg = 0;
                for (int y2 = 0; y2 < Settings.Res; y2++) {
                    for (int x2 = 0; x2 < Settings.Res; x2++) {
                        int xCoord = x * Settings.Res + x2;
                        int yCoord = y * Settings.Res + y2;
                        avg += decayMap[xCoord, yCoord];
                    }
                }
                avg /= Settings.Res * Settings.Res;
                int r = (int)(avg * 255);
                Color color = new Color(r, 0, 0, 255);

                Raylib.DrawRectangle(x * ratio * Settings.Res, y * ratio * Settings.Res, Settings.Res * 2, Settings.Res * 2, color);
            }
        }
    }

    static void DrawDebug() {
        Raylib.DrawText("FPS: " + Raylib.GetFPS().ToString(), 10, 10, 20, Color.WHITE);
    }
}