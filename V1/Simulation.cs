using Raylib_cs;

public class Simulation {
    public void Loop() {
        Raylib.InitWindow(Settings.MapWidth * Settings.RES, Settings.MapHeight * Settings.RES, "S.P.I.S. - Country Roads since 2023");

        Agent[] agents = new Agent[Settings.AgentAmount];
        InitAgents(ref agents);
        CoordData[,] map = new CoordData[Settings.MapWidth, Settings.MapHeight];
        InitMap(ref map);

        int progress = 0;
        while (!Raylib.WindowShouldClose()) {
            UpdateAgents(ref agents, map);
            if (progress++ % Settings.DRAW_INTERVAL == 0) {
                Draw(agents, map);
            }
        }

        Raylib.CloseWindow();
    }

    void InitAgents(ref Agent[] agents) {
        for (int i = 0; i < agents.Length; i++) {
            Random random = new Random();
            int x = Settings.MapWidth / 2;
            int y = Settings.MapHeight / 2;
            float a = (float)random.NextDouble() * MathF.PI * 2;
            agents[i] = new Agent(x, y, a);
        }
    }

    void InitMap(ref CoordData[,] map) {
        for (int y = 0; y < Settings.MapHeight; y++) {
            for (int x = 0; x < Settings.MapWidth; x++) {
                map[x, y] = new CoordData(new Random().NextDouble() < 0.0001);
            }
        }
    }

    void UpdateAgents(ref Agent[] agents, CoordData[,] map) {
        foreach (Agent agent in agents) {
            agent.Move(map);
        }
        foreach (Agent agent in agents) {
            CoordData coord = map[agent.XCoord, agent.YCoord];
            coord.SporeStrength = 1;
            if (coord.IsPoint) {
                agent.PheremoneStrength = 1;
            }
            if (agent.PheremoneStrength > 0) {
                coord.PheremoneStrength = 1;
            } 
        }
        for (int y = 0; y < Settings.MapHeight; y++) {
            for (int x = 0; x < Settings.MapWidth; x++) {
                map[x, y].Decay();
            }
        }
    }

    void Draw(Agent[] agents, CoordData[,] map) {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLACK);
        
        for (int y = 0; y < Settings.MapHeight; y++) {
            for (int x = 0; x < Settings.MapWidth; x++) {
                CoordData coord = map[x, y];
                Color color;
                
                if (coord.IsPoint) {
                    color = Color.WHITE;
                }
                else if (coord.SporeStrength == 0) {
                    continue;
                }
                else {
                    int r = (int)Math.Round(MathF.Pow(coord.SporeStrength, 1) * 255f);
                    int b = (int)Math.Round(MathF.Pow(coord.PheremoneStrength, 1) * 255f);
                    color = coord.SporeStrength == 0 ? Color.BLACK : new Color(r, b, 0, 255);
                }


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
