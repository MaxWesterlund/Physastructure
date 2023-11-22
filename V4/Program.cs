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

		Raylib.InitWindow(Settings.Width * Settings.Scaling, Settings.Height * Settings.Scaling, Settings.SplashText);
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
				Draw(sim.Scene.Grid);
			}
			frame++;
		}
	}

	public static void Draw(Data[,] grid) {
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
					//c = Color.WHITE;
				}

				foreach (Node n in sim.Nodes) {
					if (n.X == x && n.Y == y) {
						c = Color.GOLD;
						break;
					}
				}

				Raylib.DrawRectangle(x * Settings.Scaling, y * Settings.Scaling, Settings.Scaling, Settings.Scaling, c);
			
			}
		}
		Raylib.EndDrawing();

		if (state == SimState.Next) {
			state = SimState.Pause;
		}

		return;
	}

	public static void Input() {
		if (Raylib.IsKeyReleased(KeyboardKey.KEY_P)) {
			state = state == SimState.Run ? SimState.Pause : SimState.Run;
		}
		else if (Raylib.IsKeyReleased(KeyboardKey.KEY_N)) {
			state = SimState.Next;
		}
		else if (Raylib.IsKeyReleased(KeyboardKey.KEY_R)) {
			state = SimState.Reset;
		}
		else if (Raylib.IsKeyReleased(KeyboardKey.KEY_Q)) {
			Environment.Exit(0);
		}
		else if (Raylib.IsKeyReleased(KeyboardKey.KEY_S)) {
			Raylib.TakeScreenshot("SPIS" + frame.ToString() + ".png");
			// TODO: implement real saving of the actuall data not just screenshot
		}
		return;
	}
}
