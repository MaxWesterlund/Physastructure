using Raylib_cs;
using System;

public class Program {
	
	public static void Main() {
		Raylib.InitWindow(Settings.Width * Settings.Scaling, Settings.Height * Settings.Scaling, "S.P.I.S. - Country Roads since 2023");

		Map map = new Map();
		Agent[] agents = new Agent[Settings.AgentAmount];
		
		Random rnd = new Random();

		float sum = 0;
		for (int i = 0; i < Settings.AgentAmount; i++) {
			float h = (float)(2 * MathF.PI * rnd.NextDouble());
			agents[i] = new Agent(rnd.Next(Settings.Width), rnd.Next(Settings.Height), h);
			sum += h * 180 / MathF.PI;
		}

		while (!Raylib.WindowShouldClose()) {
			map.Decay();

			foreach (Agent a in agents) {
				a.Sense(map);
				a.Move(ref map);
			}

			for (int i = agents.Length -1; i >= 0; i--) {
				switch (agents[i].State) {
				case AgentAction.Skip:
					continue;

				case AgentAction.Delete:
					map.Grid[agents[i].X, agents[i].Y].IsOccupied = false;
					RemoveAt(ref agents, i);
					break;

				case AgentAction.Spawn:

					for (int x = agents[i].X -1; x < agents[i].X +1; x++) {
						for (int y = agents[i].Y -1; y < agents[i].Y +1; y++) {
							
							if (map.IsOutOfBounds(x, y)) {
								continue;
							}

							if (!map.Grid[x, y].IsOccupied) {
								float heading = (float)(2 * MathF.PI * rnd.NextDouble());
								agents = agents.Append(new Agent(x, y, heading)).ToArray();
								map.Grid[x, y].IsOccupied = true;
								goto brk;
							}
						}
					}
					brk:
					break;
				}
			}

			Draw(map);
		}
	}

	public static void Draw(Map map) {
		Raylib.BeginDrawing();
		Raylib.ClearBackground(Color.BLACK);
	
		for (int x = 0; x < Settings.Width; x++) {
			for (int y = 0; y < Settings.Height; y++) {
				Color c;
				if (map.Grid[x, y].IsOccupied) {
					c = Color.WHITE;
				}
				else if (map.Grid[x, y].PheremoneStrength > 0) {
					c = Color.RED;
				}
				else {
					c = Color.BLACK;
				}

				Raylib.DrawRectangle(x * Settings.Scaling, y * Settings.Scaling, Settings.Scaling, Settings.Scaling, c);
			
			}
		}

		Raylib.EndDrawing();

		return;
	}

	public static void RemoveAt(ref Agent[] source, int index) {
		for (int i = index; i < source.Length -1; i++) {
			source[i] = source[i+1];
		}
		Array.Resize(ref source, source.Length -1);
		return;
	}
}
