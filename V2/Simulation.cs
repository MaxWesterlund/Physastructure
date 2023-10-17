public class Simulation {

	public Scene Scene = new Scene();
	public Agent[] Agents = new Agent[Settings.AgentCount];

	Random rnd = new Random();

	public Simulation() {

		float sum = 0;
		for (int i = 0; i < Settings.AgentCount; i++) {
			float h = (float)(2 * MathF.PI * rnd.NextDouble());
			Agents[i] = new Agent(rnd.Next(Settings.Width), rnd.Next(Settings.Height), h);
			sum += h * 180 / MathF.PI;
		}
	}

	public void Step() {
		Scene.Decay();

		foreach (Agent a in Agents) {
			a.Sense(Scene);
			a.Move(ref Scene);
		}

		for (int i = Agents.Length -1; i >= 0; i--) {
			switch (Agents[i].State) {
			case Agent.Action.Skip:
				continue;

			case Agent.Action.Delete:
				Scene.Grid[Agents[i].X, Agents[i].Y].IsOccupied = false;
				RemoveAgent(ref Agents, i);
				break;

			case Agent.Action.Spawn:

				for (int x = Agents[i].X -1; x < Agents[i].X +1; x++) {
					for (int y = Agents[i].Y -1; y < Agents[i].Y +1; y++) {

						if (Scene.IsOutOfBounds(x, y)) {
							continue;
						}

						if (!Scene.Grid[x, y].IsOccupied) {
							float heading = (float)(2 * MathF.PI * rnd.NextDouble());
							Agents = Agents.Append(new Agent(x, y, heading)).ToArray();
							Scene.Grid[x, y].IsOccupied = true;
							goto brk;
						}
					}
				}
brk:
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

}

