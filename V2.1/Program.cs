using System.Numerics;
using Raylib_cs;

public class Program {
	
	enum SimState {
		Run,
		Pause,
		Next,
		Reset,
		Quit,
		Save
	}

	static SimState state;
	static Simulation? sim;
	static int frame;

	public static void Main() {
start:
		Settings.LoadSettings("Settings.toml");

		sim = new Simulation();
		state = SimState.Run;
		frame = 0;

		Raylib.InitWindow(Settings.WindowSize, Settings.WindowSize, Settings.SplashText);
		while (!Raylib.WindowShouldClose()) {
			Input();
			if (state == SimState.Reset) {
				Raylib.CloseWindow();
				GC.Collect();
				goto start;
			}

			if (state == SimState.Run || state == SimState.Next) {
				sim.Step();
			}

			if (frame % Settings.FrameSkip == 0) {
				Draw(sim.Scene.Grid, sim.Agents);
			}
			frame++;
		}
	}

	public static void Draw(Data[,] grid, List<Agent> agents) {
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.BLACK);
	
		for (int y = 0; y < Settings.PixelAmount; y++) {
            for (int x = 0; x < Settings.PixelAmount; x++) {
                float avg = 0;
                for (int y2 = 0; y2 < Settings.Resolution; y2++) {
                    for (int x2 = 0; x2 < Settings.Resolution; x2++) {
                        int xCoord = x * Settings.Resolution + x2;
                        int yCoord = y * Settings.Resolution + y2;
                        avg += grid[xCoord, yCoord].PheremoneStrength;
                    }
                }
                avg /= Settings.Resolution * Settings.Resolution;
                int r = (int)(avg * 255);

                Color color = new Color(0, 0, r, 255);

                Raylib.DrawRectangle(x * Settings.PixelScaler, y * Settings.PixelScaler, Settings.PixelScaler, Settings.PixelScaler, color);
            }
        }

		// if (Settings.DrawAgents) {
		// 	Raylib.DrawCircle(agents[0].X * Settings.PixelScaler, agents[0].Y * Settings.PixelScaler, 5, Color.WHITE);
		// }

		Raylib.DrawText("FPS: " + Raylib.GetFPS().ToString(), 10, 10, 20, Color.WHITE);
		Raylib.DrawText("Agents: " + agents.Count, 10, 30, 20, Color.WHITE);
		// float avgHeading = 0;
		// foreach (Agent agent in agents) {
		// 	avgHeading += agent.Heading;
		// }
		// avgHeading /= agents.Length;
		// Raylib.DrawText("Avg Heading: " + avgHeading.ToString(), 10, 50, 20, Color.WHITE);

		Raylib.EndDrawing();

		if (state == SimState.Next) {
			state = SimState.Pause;
		}

		return;
	}

	public static void Input() {
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_P)) {
			state = state == SimState.Run ? SimState.Pause : SimState.Run;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_N)) {
			state = SimState.Next;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_R)) {
			state = SimState.Reset;
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q)) {
			Environment.Exit(0);
		}
		else if (Raylib.IsKeyPressed(KeyboardKey.KEY_S)) {
			Raylib.TakeScreenshot("SPIS" + frame.ToString() + ".png");
			// TODO: implement real saving of the actual data not just screenshot
		}
		return;
	}
}
