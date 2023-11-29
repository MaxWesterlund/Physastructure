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

			if (frame % Settings.FrameSkip == 0 || !sim.NodesPlaced) {
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

				if (grid[x, y].Solflux <= 0) {
					continue;
				}

				int solfluxColor = (int)grid[x, y].Solflux * 50 % 255;
				c = new Color(50, 0, solfluxColor, 255);

				Raylib.DrawRectangle(x * Settings.Scaling, y * Settings.Scaling, Settings.Scaling, Settings.Scaling, c);
			}
		}
				
		if (Settings.DrawAgents) {
			foreach (Agent a in sim.Agents) {
				Raylib.DrawRectangle((int)a.X * Settings.Scaling, (int)a.Y * Settings.Scaling, Settings.Scaling, Settings.Scaling, Color.WHITE);

			}
		}


		foreach (Node n in sim.Nodes) {
			Raylib.DrawRectangle(n.X * Settings.Scaling - 5, n.Y * Settings.Scaling - 5, 10, 10, Color.PINK);
		}
	
		Raylib.DrawText(frame.ToString(), 15, 15, 20, Color.WHITE);
		Raylib.EndDrawing();

		return;
	}
}
