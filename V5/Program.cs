using System.Diagnostics.Tracing;
using System.Numerics;
using System.Text;
using Raylib_cs;
using Settings;

static class Program {
    static Random random = new Random();
    
    static void Main() {
        bool nodesPlaced = false;
        bool isPaused = false;

        CoordinateData[,] grid = new CoordinateData[Simulation.Size, Simulation.Size];
        List<Agent> agents = new();

        float[,] diffuseKernel = GenerateDiffuseKernel();

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
            Control();
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE)) {
                isPaused = !isPaused;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) {
                nodesPlaced = true;
            }
            
            if (!isPaused) {
                if (!nodesPlaced) {
                PlaceNodes(grid);
                }
                else {
                    Step(grid, agents, diffuseKernel);
                    step++;
                }
            }

            Draw(grid, agents, step);
        }
        Raylib.CloseWindow();
    }

    static float[,] GenerateDiffuseKernel() {
		float sigma = MathF.Max((float)Simulation.KernelRadius / 2, 2);
		int kernelWidth = 2 * Simulation.KernelRadius + 1;
		float[,] kernel = new float[2 * kernelWidth, 2 * kernelWidth];
		float sum = 0f;

		for (int x = -Simulation.KernelRadius; x <= Simulation.KernelRadius; x++) {
			for (int y = -Simulation.KernelRadius; y <= Simulation.KernelRadius; y++) {
				float exponentNumerator = (float)-(x * x + y * y);
				float exponentDenominator = 2f * sigma * sigma;

				float eExpression = MathF.Pow(MathF.E, exponentNumerator / exponentDenominator);
				float kernelValue = (eExpression / (2f * MathF.PI * sigma * sigma));

				kernel[x + Simulation.KernelRadius, y + Simulation.KernelRadius] = kernelValue;
				sum += kernelValue;
			}
		}

		for (int x = 0; x < kernelWidth; x++) {
			for (int y = 0; y < kernelWidth; y++) {
				kernel[x, y] /= sum;
			}
		}
        
        return kernel;
	}

    static void Control() {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) {
            Window.Resolution++;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN) && Window.Resolution > 1) {
            Window.Resolution--;
        }
    }

    static void PlaceNodes(CoordinateData[,] grid) {
        if (!Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) return;

        float ratio = (float)Simulation.Size / Window.Size;
        Vector2 mousePos = Raylib.GetMousePosition() * ratio;

        int xCoord = (int)Math.Round(mousePos.X);
        int yCoord = (int)Math.Round(mousePos.Y);
        Vector2 center = new Vector2(xCoord, yCoord);

        for (int x = Math.Clamp(xCoord - Simulation.NodeSpreadRadius, 0, Simulation.Size - 1); x < Math.Clamp(xCoord + Simulation.NodeSpreadRadius, 0, Simulation.Size - 1) + 1; x++) {
            for (int y = Math.Clamp(yCoord - Simulation.NodeSpreadRadius, 0, Simulation.Size - 1); y < Math.Clamp(yCoord + Simulation.NodeSpreadRadius, 0, Simulation.Size - 1) + 1; y++) {
                Vector2 coord = new Vector2(x, y);
                float dist = Vector2.Distance(coord, center);
                if (dist > Simulation.NodeSpreadRadius || !Utils.IsWithinBounds(coord)) continue;

                float value = 1 - dist / Simulation.NodeSpreadRadius;
                grid[x, y].NodeStrength = value;

                if (Vector2.Distance(coord, center) <= Window.NodeRadius) {
                    grid[x, y].IsNode = true; 
                }
            }
        }
    }

    static void Step(CoordinateData[,] grid, List<Agent> agents, float[,] diffuseKernel) {
        Utils.Shuffle(agents);
        foreach (Agent agent in agents) {
            agent.Move(grid);
            agent.LeaveSpore(grid);
        }
        // DiffuseGrid(grid, diffuseKernel);
        for (int y = 0; y < Simulation.Size; y++) {
            for (int x = 0; x < Simulation.Size; x++) {
                grid[x, y].SporeStrength *= Simulation.DecayRate;
            }
        }
    }

    static void Draw(CoordinateData[,] grid, List<Agent> agents, int step) {
        Raylib.BeginDrawing();

        Raylib.ClearBackground(Window.BackgroundColor);

        int length = Simulation.Size / Window.Resolution;
        int ratio = Window.Size / Simulation.Size;
        int positionFactor = Window.Resolution * ratio; 

        for (int y = 0; y < length; y++) {
            for (int x = 0; x < length; x++) {
                float avgSpore = 0;
                float avgNode = 0;
                bool isNode = false;
                bool isOutOfBounds = false;
                for (int y2 = 0; y2 < Window.Resolution; y2++) {
                    for (int x2 = 0; x2 < Window.Resolution; x2++) {
                        int xCoord = x * Window.Resolution + x2;
                        int yCoord = y * Window.Resolution + y2;
                        avgSpore += grid[xCoord, yCoord].SporeStrength;
                        avgNode += grid[xCoord, yCoord].NodeStrength;
                        if (grid[xCoord, yCoord].IsNode) {
                            isNode = true;
                        }
                        if (!Utils.IsWithinBounds(new Vector2(xCoord, yCoord))) {
                            isOutOfBounds = true;
                        }
                    }
                }
                avgSpore /= Window.Resolution * Window.Resolution;
                avgNode /= Window.Resolution * Window.Resolution;

                int s = (int)(avgSpore * 255);
                int n = (int)(avgNode * 255);

                Color color;
                if (isNode) {
                    color = Window.NodeColor;
                }
                else if (!isOutOfBounds) {
                    color = new Color(s, s, 0, 255);
                }
                else {
                    color = Window.BorderColor;
                }
                Raylib.DrawRectangle(x * positionFactor, y * positionFactor, ratio * Window.Resolution, ratio * Window.Resolution, color);
            }
        }
        
        Raylib.DrawText("FPS: " + Raylib.GetFPS().ToString(), 10, 10, 20, Window.TextColor);
        Raylib.DrawText("Step: " + step, 10, 40, 20, Window.TextColor);

        Raylib.EndDrawing();
    }

    static void DiffuseGrid(CoordinateData[,] grid, float[,] kernel) {
		for (int x = 0; x < Simulation.Size; x++) {
			for (int y = 0; y < Simulation.Size; y++) {
				float value = 0f;

				for (int kx = -Simulation.KernelRadius; kx <= Simulation.KernelRadius; kx++) {
					for (int ky = -Simulation.KernelRadius; ky <= Simulation.KernelRadius; ky++) {
						
						float kernelValue = kernel[kx + Simulation.KernelRadius, ky + Simulation.KernelRadius];
						int offsetX = Math.Clamp(x + kx, 0, Simulation.Size -1);
						int offsetY = Math.Clamp(y + ky, 0, Simulation.Size -1);
						value += grid[offsetX, offsetY].SporeStrength * kernelValue;

					}
				}

				grid[x, y].SporeStrength = value;

			}
		}
	}
}
