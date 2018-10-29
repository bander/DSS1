using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelBuild : SubPanelBase {

    public void ActivateFloor()
    {
        type = 0;
        buildType = BuildType.Floor;
        tileType = TileType.Floor;
        SubClick();
    }
    public void ActivateWall1()
    {
        type = 1;
        buildType = BuildType.Wall;
        wallType = WallType.Wall;
        SubClick();
    }
    public void ActivateWall2()
    {
        type = 2;
        buildType = BuildType.Wall;
        wallType = WallType.SmallWindow;
        SubClick();
    }
    public void ActivateWall3()
    {
        type = 3;
        buildType = BuildType.Wall;
        wallType = WallType.BigWindow;
        SubClick();
    }
    public void ActivateDoor()
    {
        type = 4;
        buildType = BuildType.Wall;
        wallType = WallType.Door;
        SubClick();
    }

}
