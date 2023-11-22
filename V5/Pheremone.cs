using System.Numerics;
using Settings;

public class Pheremone {
    Vector2 position;
    int strength = Simulation.PheremoneSpreadRadius;

    public Pheremone(int x, int y) {
        position = new Vector2(x, y);
    }

    public void Spread(ref float[,] decayMap, List<Pheremone> pheremones) {
        if (strength == 0) {
            pheremones.Remove(this);
            return;
        }

        for (int y = 0; y < Simulation.Size; y++) {
            for (int x = 0; x < Simulation.Size; x++) {
                Vector2 coordVector = new Vector2(x, y);
                if (Vector2.Distance(coordVector, position) <= (Simulation.PheremoneSpreadRadius - strength)) {
                    decayMap[(int)coordVector.X, (int)coordVector.Y] = 1;
                }
            }
        }
        
        strength--;
    }
}