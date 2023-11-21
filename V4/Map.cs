public class Data {
	public int Height;

	public float Solflux;
	public float NewSolflux;

	public bool IsOccupied;

	public Data() {
		Height = 0;
		Solflux = 0;
		NewSolflux = 0;

		IsOccupied = false;

		return;
	}

	public void Decay() {
		Solflux = NewSolflux;
		NewSolflux = 0;

		Solflux *= Settings.DecayRate;
		if (Solflux < 0.001) {
			Solflux = 0f;
		}
		return;
	}

	public float Evaluate(int height) {
		int heightDiff = Math.Abs(height - Height);
		float value =  Solflux * Settings.SolfluxWeight - heightDiff * Settings.HeightWeight;
		return value;
	}
}


