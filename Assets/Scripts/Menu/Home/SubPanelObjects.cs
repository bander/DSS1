using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelObjects : SubPanelBase {
    public void ActivateStair()
    {
        type = 0;
        buildType = BuildType.OnFloor;
        tileType = TileType.Stair;
        SubClick();
        rotateButton.SetActive(true);
    }
    public void ActivateTurret()
    {
        type = 1;
        buildType = BuildType.OnFloor;
        tileType = TileType.Turret;
        SubClick();
    }
}
