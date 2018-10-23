using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanelObjects : SubPanelBase {
    public void ActivateLedder()
    {
        type = 0;
        buildType = 3;
        objectType = 0;
        SubClick();
        rotateButton.SetActive(true);
    }
    public void ActivateTurret()
    {
        type = 1;
        buildType = 4;
        objectType = 0;
        SubClick();
    }
}
