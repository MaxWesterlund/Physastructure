using Raylib_cs;

public class Simulation {
    public void Loop() {
        Raylib.InitWindow(Settings.WIDTH * Settings.RES, Settings.HEIGHT * Settings.RES, "S.P.I.S. - Country Roads since 2023");

        Agent[] agents = new Agent[Settings.AgentAmount];
        InitAgents(ref agents);
        float[,] decayMap = new float[Settings.WIDTH, Settings.HEIGHT];

        int progress = 0;
        while (!Raylib.WindowShouldClose()) {
            UpdateAgents(ref agents, decayMap);
            if (progress++ % Settings.DRAW_INTERVAL == 0) {
                Draw(agents, decayMap);
            }
        }

        Raylib.CloseWindow();
    }

    void InitAgents(ref Agent[] agents) {
        for (int i = 0; i < agents.Length; i++) {
            Random random = new Random();
            int x = Settings.WIDTH / 2;
            int y = Settings.HEIGHT / 2;
            float a = (float)random.NextDouble() * MathF.PI * 2;
            agents[i] = new Agent(x, y, a);
        }
    }

    void UpdateAgents(ref Agent[] agents, float[,] decayMap) {
        foreach (Agent agent in agents) {
            agent.Move(decayMap);
        }
        foreach (Agent agent in agents) {
            decayMap[agent.XCoord, agent.YCoord] = 1;
        }
        for (int y = 0; y < Settings.HEIGHT; y++) {
            for (int x = 0; x < Settings.WIDTH; x++) {
                decayMap[x, y] *= Settings.SporeDecayRate;
            }
        }
    }

    void Draw(Agent[] agents, float[,] decayMap) {
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLACK);
        
        for (int y = 0; y < Settings.HEIGHT; y++) {
            for (int x = 0; x < Settings.WIDTH; x++) {
                float value = decayMap[x, y];
                if (value == 0) {
                    continue;
                }
                int p = (int)Math.Round(MathF.Pow(value, 1) * 255f);
                Color color = value == 0 ? Color.BLACK : new Color(p, 0, 0, 255);
                Raylib.DrawRectangle(x * Settings.RES, y * Settings.RES, Settings.RES, Settings.RES, color);
            }
        }
        Debug();
        Raylib.EndDrawing();
    }

    void Debug() {
        Raylib.DrawText(Raylib.GetFPS().ToString(), 10, 10, 20, Color.WHITE);
    }
}