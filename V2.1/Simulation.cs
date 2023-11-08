public class Simulation {
	public Scene Scene = new Scene();
	public List<Agent> Agents = new List<Agent>();

	Random rng = new Random();

	public Simulation() {
		for (int i = 0; i < Settings.AgentCount; i++) {
			Agents.Add(new Agent(rng.Next(Settings.Size), rng.Next(Settings.Size)));
		}
	}

	public void Step() {
		Scene.Decay();

		Shuffle(ref Agents);
		foreach (Agent a in Agents) {
			a.Sense(Scene);
			a.Move(ref Scene);
		}

		for (int i = 0; i < Agents.Count; i++) {
			switch (Agents[i].State) {
				case Agent.Action.Skip:
					continue;

				case Agent.Action.Delete:
					Scene.Grid[Agents[i].X, Agents[i].Y].IsOccupied = false;
					Agents.Remove(Agents[i]);
					break;

				case Agent.Action.Spawn:
					for (int x = Agents[i].X -1; x < Agents[i].X +1; x++) {
						for (int y = Agents[i].Y -1; y < Agents[i].Y +1; y++) {

							if (Scene.IsOutOfBounds(x, y)) {
								continue;
							}

							if (!Scene.Grid[x, y].IsOccupied) {
								Agents.Add(new Agent(x, y));
								Scene.Grid[x, y].IsOccupied = true;
								goto brk;
							}
						}
					}
				brk:
				break;
			}
		}
	}

	void Shuffle<T>(ref List<T> list) {  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
    	}  
	}
}