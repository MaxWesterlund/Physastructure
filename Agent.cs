using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

public class Agent {
    public int XCoord;
    public int YCoord;
    public float Heading;

    float xPos;
    float yPos;

    public Agent(int xStart, int yStart, int heading) {
        XCoord = xStart;
        YCoord = yStart;
        Heading = heading;

        xPos = xStart;
        yPos = yStart;
    }

    public void Move(float[,] decayMap) {
        xPos = MathF.Round(xPos + MathF.Cos(Heading));
        yPos = MathF.Round(yPos + MathF.Sin(Heading));
        
        XCoord = (int)MathF.Round(xPos);
        YCoord = (int)MathF.Round(yPos);
        XCoord = Math.Clamp(XCoord, 0, Settings.WIDTH - 1);
        YCoord = Math.Clamp(YCoord, 0, Settings.HEIGHT - 1);

        decayMap[XCoord, YCoord] = 1;
    }
}