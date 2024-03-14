using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeSettingUpdater : MonoBehaviour
{
    public void SetSfxMultiplier()
    {
        VolumeSettings.SfxMultiplier = this.gameObject.GetComponent<Slider>().value;
    }
}
