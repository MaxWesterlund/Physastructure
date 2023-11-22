using Raylib_cs;
using Settings;

static class Program {
    static Random random = new Random();
    
    static void Main() {
        float[,] decayMap = new float[Simulation.Size, Simulation.Size];
        List<Agent> agents = new();
        List<Pheremone> pheremones = new();
         
        for (int i = 0; i < Simulation.AgentAmount; i++) {
            Agent agent = new Agent(random.Next(Simulation.Size), random.Next(Simulation.Size), (float)random.NextDouble() * 2 * MathF.PI);
            agents.Add(agent);
        }

        Raylib.InitWindow(Window.Size, Window.Size, "Physastructure");
        while (!Raylib.WindowShouldClose()) {
            Step(ref decayMap, agents, pheremones);
            Draw(decayMap);
        }
        Raylib.CloseWindow();
    }

    static void Step(ref float[,] decayMap, List<Agent> agents, List<Pheremone> pheremones) {
        foreach (Agent agent in agents) {
            agent.Move(pheremones);
        }
        for (int i = pheremones.Count - 1; i > -1; i--) {
            pheremones[i].Spread(ref decayMap, pheremones);
        }
        for (int y = 0; y < Simulation.Size; y++) {
            for (int x = 0; x < Simulation.Size; x++) {
                decayMap[x, y] *= Simulation.DecayRate;
            }
        }
    }

    static void Draw(float[,] decayMap) {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Window.backgroundColor);

        int length = Simulation.Size / Window.Resolution;
        int ratio = Window.Size / Simulation.Size;
        int positionFactor = Window.Resolution * ratio; 

        for (int y = 0; y < length; y++) {
            for (int x = 0; x < length; x++) {
                float avg = 0;
                for (int y2 = 0; y2 < Window.Resolution; y2++) {
                    for (int x2 = 0; x2 < Window.Resolution; x2++) {
                        int xCoord = x * Window.Resolution + x2;
                        int yCoord = y * Window.Resolution + y2;
                        avg += decayMap[xCoord, yCoord];
                    }
                }
                avg /= Window.Resolution * Window.Resolution;

                int r = (int)(avg * 255);
                Color color = new Color(r, 0, 0, 255);
                Raylib.DrawRectangle(x * positionFactor, y * positionFactor, ratio * Window.Resolution, ratio * Window.Resolution, color);
            }
        }

        Raylib.EndDrawing();
    }
}