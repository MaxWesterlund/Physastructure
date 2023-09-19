using Raylib_cs;
using System.Security.Cryptography;

public class Visuals {
    float[,] decayMap = new float[Settings.WIDTH, Settings.HEIGHT];
    Agent[] agents = new Agent[Settings.AgentAmount];

    public void Draw() {
        Raylib.InitWindow(Settings.WIDTH * Settings.RES, Settings.HEIGHT * Settings.RES, "S.P.I.S. - Country Roads since 2023");

        for (int i = 0; i < agents.Length; i++) {
            agents[i] = new Agent(Settings.WIDTH / 2, Settings.HEIGHT / 2, RandomNumberGenerator.GetInt32(0, 360));
        }

        while (!Raylib.WindowShouldClose()) {
            foreach (Agent agent in agents) {
                agent.Move(decayMap);
            }
            Raylib.BeginDrawing();
            for (int y = 0; y < Settings.HEIGHT; y++) {
                for (int x = 0; x < Settings.WIDTH; x++) {
                    Color color = new Color((int)Math.Round(decayMap[x, y] * 255), 0, 0, 255);
                    Raylib.DrawRectangle(x * Settings.RES, y * Settings.RES, Settings.RES, Settings.RES, color);
                    decayMap[x, y] *= 0.7f;
                }
            }
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}