using System.Numerics;
using Settings;

public static class Utils {
    public static bool IsWithinBounds(Vector2 pos) {
        Vector2 center = new Vector2(Simulation.Size / 2f - 1.5f);
        if (Vector2.Distance(pos, center) <= Simulation.Size / 2 && pos.X > 0 && pos.X < Simulation.Size && pos.Y > 0 && pos.Y < Simulation.Size) {
            return true;
        }
        return false;
    }
}