using System.Numerics;
using Settings;

public class Pheremone {
    Vector2 position;
    int strength = 1;

    public Pheremone(int x, int y) {
        position = new Vector2(x, y);
    }

    public void Spread(ref float[,] decayMap, List<Pheremone> pheremones) {
        if (strength == Simulation.PheremoneSpreadRadius) {
            pheremones.Remove(this);
            return;
        }

        for (int y = -strength; y < strength; y++) {
            for (int x = -strength; x < strength; x++) {
                Vector2 coordVector = new Vector2(position.X + x, position.Y + y);
                if (Vector2.Distance(coordVector, position) <= (Simulation.PheremoneSpreadRadius - strength)) {
                    decayMap[Math.Clamp((int)coordVector.X, 0, Simulation.Size - 1), Math.Clamp((int)coordVector.Y, 0, Simulation.Size - 1)] = 1;
                }
            }
        }
        
        strength++;
    }
}