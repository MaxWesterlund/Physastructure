using Raylib_cs;

public class Visuals {
    float[,] decayMap = new float[Settings.WIDTH, Settings.HEIGHT];
    Agent[] agents = new Agent[Settings.AgentAmount];

    public void Draw() {
        Raylib.InitWindow(Settings.WIDTH * Settings.RES, Settings.HEIGHT * Settings.RES, "S.P.I.S. - Country Roads since 2023");

        for (int i = 0; i < agents.Length; i++) {
            Random random = new Random();
            int x = Settings.WIDTH / 2;
            int y = Settings.HEIGHT / 2;
            float a = (float)random.NextDouble() * MathF.PI * 2;
            agents[i] = new Agent(x, y, a);
        }

        while (!Raylib.WindowShouldClose()) {
            foreach (Agent agent in agents) {
                agent.Move(decayMap);
            }
            Raylib.BeginDrawing();
            for (int y = 0; y < Settings.HEIGHT; y++) {
                for (int x = 0; x < Settings.WIDTH; x++) {
                    float value = decayMap[x, y];
                    int p = (int)Math.Round(MathF.Pow(value, 10) * 255f);
                    Color color = value == 0 ? Color.BLACK : new Color(p, p, 0, 255);
                    Raylib.DrawRectangle(x * Settings.RES, y * Settings.RES, Settings.RES, Settings.RES, color);
                    decayMap[x, y] *= Settings.SporeDecayRate;
                }
            }
            foreach (Agent agent in agents) {
                decayMap[agent.XCoord, agent.YCoord] = 1;
            }
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}