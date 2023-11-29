using System.Numerics;
using Raylib_cs;

public class Simulation {

	public Scene Scene = new Scene();
	public Agent[] Agents = new Agent[Settings.AgentCount];
	public List<Node> Nodes = new();

	Random rnd = new Random();

	public bool NodesPlaced = false;

	public Simulation() {
		for (int i = 0; i < Settings.AgentCount; i++) {
			float h = (float)(2 * MathF.PI * rnd.NextDouble());
			Agents[i] = new Agent(rnd.Next(Settings.Width), rnd.Next(Settings.Height), h, ref rnd);
		}
	}

	public void Step() {
		if (!NodesPlaced) {
			if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) {
				Vector2 pos = Raylib.GetMousePosition() / Settings.Scaling;
				Nodes.Add(new Node((int)Math.Round(pos.X), (int)Math.Round(pos.Y)));
			}
			if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) {
				NodesPlaced = true;
			}
			return;
		}


		ShuffleAgents(ref Agents);
		foreach (Agent a in Agents) {
			a.Sense(Scene);
		}
		
		foreach (Node n in Nodes) {
			Scene.Grid[n.X, n.Y].Solflux += Settings.NodeSolflux;
		}

		foreach (Agent a in Agents) {
			a.Move(ref Scene);
		}
		Scene.DiffuseLattice();
		Scene.Decay();

		for (int i = Agents.Length -1; i >= 0; i--) {
			switch (Agents[i].State) {
			case Agent.Action.Skip:
				continue;

			case Agent.Action.Delete:
				Scene.Grid[(int)Agents[i].X, (int)Agents[i].Y].IsOccupied = false;
				RemoveAgent(ref Agents, i);
				break;

			case Agent.Action.Spawn:

				int tries = 0;
				while (tries < 18) {
					int x = (int)Agents[i].X + (rnd.Next(3) -1);
					int y = (int)Agents[i].Y + (rnd.Next(3) -1);
					
					if (Scene.IsOutOfBounds(x, y) || Scene.Grid[x, y].IsOccupied) {
						tries++;
						continue;
					}

					float heading = (float)(2 * MathF.PI * rnd.NextDouble());
					Agents = Agents.Append(new Agent(x, y, heading, ref rnd)).ToArray();
					Scene.Grid[x, y].IsOccupied = true;
					break;
				}
				break;
			}
		}
		return;
	}

	public void RemoveAgent(ref Agent[] source, int index) {
		for (int i = index; i < source.Length -1; i++) {
			source[i] = source[i+1];
		}
		Array.Resize(ref source, source.Length -1);
		return;
	}

	public void ShuffleAgents(ref Agent[] source)
	{
		Random random = new Random();

		for (int i = source.Length - 1; i > 0; i--)
		{
			int j = random.Next(0, i + 1);

			Agent temp = source[i];
			source[i] = source[j];
			source[j] = temp;
		}
		return;
	}
}

