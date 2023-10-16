using Raylib_cs;
using System.Numerics;

public class Simulation {
    public void Run() {
        Agent[] agents = new Agent[Settings.AgentAmnt];
        for (int i = 0; i < agents.Length; i++) {
            float p = (float)i / Settings.AgentAmnt;
            agents[i] = new(p * 360);
        }

        Raylib.InitWindow(Settings.ScrnSize, Settings.ScrnSize, "SPIS");

        while (!Raylib.WindowShouldClose()) {
            // Shuffle(ref agents);
            foreach (Agent agent in agents) {
                agent.Move();
                agent.LeaveSpore();
            }

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.BLACK);
            Draw.DrawPheremoneMap(Map.PheremoneMap);
            Draw.DrawDebug();

            Raylib.EndDrawing();

            Map.Decay();
        }

        Raylib.CloseWindow();
    }

    public static void Shuffle<T>(ref T[] array) {
        Random rng = new();
        int n = array.Length;
        while (n > 1) {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}