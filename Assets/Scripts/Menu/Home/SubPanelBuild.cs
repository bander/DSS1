using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelBuild : SubPanelBase {

    public void ActivateFloor()
    {
        type = 0;
        buildType = 0;
        objectType = 0;
        SubClick();
    }
    public void ActivateWall1()
    {
        type = 1;
        buildType = 1;
        objectType = 0;
        SubClick();
    }
    public void ActivateWall2()
    {
        type = 2;
        buildType = 1;
        objectType = 1;
        SubClick();
    }
    public void ActivateWall3()
    {
        type = 3;
        buildType = 1;
        objectType = 2;
        SubClick();
    }
    public void ActivateDoor()
    {
        type = 4;
        buildType = 2;
        objectType = 0;
        SubClick();
    }

}
