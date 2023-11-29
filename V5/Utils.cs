using System.Numerics;
using Settings;

public static class Utils {
    public static bool IsWithinBounds(Vector2 pos) {
        Vector2 center = new Vector2(Simulation.Size / 2);
        if (Vector2.Distance(pos, center) > Simulation.Size / 2) {
            return false;
        }
        return true;
    }
}