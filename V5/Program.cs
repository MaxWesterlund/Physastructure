using System.Numerics;
using Raylib_cs;
using Settings;

static class Program {
    static Random random = new Random();
    
    static void Main() {
        bool nodes = false;

        CoordinateData[,] grid = new CoordinateData[Simulation.Size, Simulation.Size];
        List<Agent> agents = new();

        int step = 0;
         
        for (int i = 0; i < Simulation.AgentAmount; i++) {
            Vector2 center = new Vector2(Simulation.Size / 2);
            Vector2 position = new Vector2(random.Next(Simulation.Size), random.Next(Simulation.Size));
            while (!Utils.IsWithinBounds(position)) {
                position = new Vector2(random.Next(Simulation.Size), random.Next(Simulation.Size));
            }
            Agent agent = new Agent((int)Math.Round(position.X), (int)Math.Round(position.Y),  (float)random.NextDouble() * 2 * MathF.PI);
            agents.Add(agent);
        }

        Raylib.InitWindow(Window.Size, Window.Size, "Physastructure");
        while (!Raylib.WindowShouldClose()) {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) {
                nodes = true;
            }
            if (!nodes) {
                PlacePoints(grid);
            }
            else {
                Step(grid, agents);
                step++;
            }
            Draw(grid, step);
        }
        Raylib.CloseWindow();
    }

    static void PlacePoints(CoordinateData[,] grid) {
        if (!Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) return;

        float ratio = (float)Simulation.Size / Window.Size;
        Vector2 mousePos = Raylib.GetMousePosition() * ratio;

        int xCoord = (int)Math.Round(mousePos.X);
        int yCoord = (int)Math.Round(mousePos.Y);
        Vector2 center = new Vector2(xCoord, yCoord);

        for (int x = Math.Clamp(xCoord - Simulation.PointRadius, 0, Simulation.Size - 1); x < Math.Clamp(xCoord + Simulation.PointRadius, 0, Simulation.Size - 1) + 1; x++) {
            for (int y = Math.Clamp(yCoord - Simulation.PointRadius, 0, Simulation.Size - 1); y < Math.Clamp(yCoord + Simulation.PointRadius, 0, Simulation.Size - 1) + 1; y++) {
                Vector2 coord = new Vector2(x, y);
                if (Vector2.Distance(coord, center) > Simulation.PointRadius || !Utils.IsWithinBounds(coord)) continue;

                grid[x, y].IsNode = true;
            }
        }
    }

    static void Step(CoordinateData[,] grid, List<Agent> agents) {
        foreach (Agent agent in agents) {
            agent.Move(grid);
            agent.LeaveSpore(grid);
        }
        for (int y = 0; y < Simulation.Size; y++) {
            for (int x = 0; x < Simulation.Size; x++) {
                grid[x, y].SporeStrength *= Simulation.DecayRate;
            }
        }
    }

    static void Draw(CoordinateData[,] grid, int step) {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Window.BackgroundColor);

        int length = Simulation.Size / Window.Resolution;
        int ratio = Window.Size / Simulation.Size;
        int positionFactor = Window.Resolution * ratio; 

        for (int y = 0; y < length; y++) {
            for (int x = 0; x < length; x++) {
                float avg = 0;
                bool isPoint = false;
                for (int y2 = 0; y2 < Window.Resolution; y2++) {
                    for (int x2 = 0; x2 < Window.Resolution; x2++) {
                        int xCoord = x * Window.Resolution + x2;
                        int yCoord = y * Window.Resolution + y2;
                        avg += grid[xCoord, yCoord].SporeStrength;
                        if (grid[xCoord, yCoord].IsNode) {
                            isPoint = true;
                        }
                    }
                }
                avg /= Window.Resolution * Window.Resolution;

                int r = (int)(avg * Window.SlimeColor.R);
                int g = (int)(avg * Window.SlimeColor.G);
                int b = (int)(avg * Window.SlimeColor.B);
                Color color;
                if (isPoint) {
                    color = Window.PointColor;
                }
                else {
                    color = new Color(r, g, b, 255);
                }
                Raylib.DrawRectangle(x * positionFactor, y * positionFactor, ratio * Window.Resolution, ratio * Window.Resolution, color);
            }
        }
        
        Raylib.DrawText("FPS: " + Raylib.GetFPS().ToString(), 10, 10, 20, Window.TextColor);
        Raylib.DrawText("Step: " + step, 10, 40, 20, Window.TextColor);

        Raylib.EndDrawing();
    }
}