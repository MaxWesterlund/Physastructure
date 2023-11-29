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

	static Simulation? sim;
	static int frame;

	public static void Main() {
		Settings.LoadSettings("Settings.toml");

		sim = new Simulation();
		frame = 0;

		Raylib.InitWindow(Settings.Width * Settings.Scaling, Settings.Height * Settings.Scaling, Settings.SplashText);

		while (!Raylib.WindowShouldClose()) {
			sim.Step();

			if (frame % Settings.FrameSkip == 0) {
				Draw();
			}

			if (sim.NodesPlaced) {
				frame++;
			}
		}
	}

	public static void Draw() {
		Data[,] grid = sim.Scene.Grid;

		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.BLACK);
	
		for (int x = 0; x < Settings.Width; x++) {
			for (int y = 0; y < Settings.Height; y++) {
				Color c;

				if (grid[x, y].Solflux > 0) {
					int solfluxColor = (int)grid[x, y].Solflux * 50 % 255;
					c = new Color(50, 0, solfluxColor, 255);
				}
				else {
					continue;
				}

				if (grid[x, y].IsOccupied && Settings.DrawAgents) {
					c = Color.WHITE;
				}

				foreach (Node n in sim.Nodes) {
					if (n.X == x && n.Y == y) {
						Raylib.DrawRectangle(x * Settings.Scaling - 5, y * Settings.Scaling - 5, 10, 10, Color.PINK);
						break;
					}
				}

				Raylib.DrawRectangle(x * Settings.Scaling, y * Settings.Scaling, Settings.Scaling, Settings.Scaling, c);
			
			}
		}
		Raylib.DrawText(frame.ToString(), 15, 15, 20, Color.WHITE);
		Raylib.EndDrawing();

		return;
	}
}
