using System.Numerics;
using Settings;

public class Agent {
    Vector2 position;
    float heading;

    public Agent(int x, int y, float head) {
        position = new Vector2(x, y);
        heading = head;
    }

    public void Move(List<Pheremone> pheremones) {
        Vector2 dir = new Vector2(MathF.Cos(heading), MathF.Sin(heading));
        position += dir;

        Pheremone pheremone =  new Pheremone(Math.Clamp((int)position.X, 0, Simulation.Size - 1), Math.Clamp((int)position.Y, 0, Simulation.Size - 1));
        pheremones.Add(pheremone);
    }
}