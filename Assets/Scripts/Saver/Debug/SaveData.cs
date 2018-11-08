using System;
using System.Collections.Generic;

[Serializable]
public struct SaveData
{
    public int currentHealth;
    public int lastLevelBeat;

    public List<MagicCubeData> magicCubes;
}

[Serializable]
public struct MagicCubeData
{
    public string id;
    public bool isRed;
}