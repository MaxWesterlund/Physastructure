using Raylib_cs;
using System.Numerics;

public class Simulation {
    public void Run() {
        float[,] decayMap = new float[Settings.Size, Settings.Size];

        Agent[] agents = new Agent[Settings.AgentAmnt];
        for (int i = 0; i < agents.Length; i++) {
            float p = (float)i / Settings.AgentAmnt;
            agents[i] = new(p * 360);
        }

        Raylib.InitWindow(Settings.ScrnSize, Settings.ScrnSize, "SPIS");

        while (!Raylib.WindowShouldClose()) {
            foreach (Agent agent in agents) {
                agent.Move();
                agent.LeaveSpore(ref decayMap);
            }

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.BLACK);
            Draw.DrawDecayMap(decayMap);
            Draw.DrawDebug();

            Raylib.EndDrawing();

            Map.Decay(ref decayMap);
        }

        Raylib.CloseWindow();
    }

    float[,] GenerateDecayMap() {
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
}