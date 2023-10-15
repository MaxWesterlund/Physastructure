using Raylib_cs;
using System.Numerics;
using System.Security.Cryptography;

public class Agent {
    Vector2 pos = new(Settings.Size / 2, Settings.Size / 2);
    Vector2 dir;

    public Agent(float a) {
        // float a = Raylib.GetRandomValue(0, 3600) / 10f;
        dir = new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
    }

        public void Move(in float[,] decayMap) {
        pos += Settings.AgentSpeed * dir;
        if (pos.X < 0 || pos.X >= Settings.Size || pos.X < 0 || pos.X >= Settings.Size) {
            dir.X *= -1;
        }
        if (pos.Y < 0 || pos.Y >= Settings.Size || pos.Y < 0 || pos.Y >= Settings.Size) {
            dir.Y *= -1;
        }
    }

    public void LeaveSpore(ref float[,] decayMap) {
        int x = (int)pos.X;
        int y = (int)pos.Y;
        if (x < 0 || x >= Settings.Size || y < 0 || y >= Settings.Size) return;
        
        decayMap[x, y] = 1;
    }
}