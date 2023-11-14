public class CoordData {
    public int MapHeight;
    public float SporeStrength;
    public float PheremoneStrength;
    public bool IsPoint;

    public CoordData(bool isPoint) {
        MapHeight = 0;
        SporeStrength = 0;
        PheremoneStrength = 0;
        IsPoint = isPoint;
    }

    public void Decay() {
        SporeStrength *= Settings.SporeDecayRate;
        PheremoneStrength *= Settings.PheremoneDecayRate;
        return;
    }
}

class LoadMapData {
    
}