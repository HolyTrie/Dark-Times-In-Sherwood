using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGVolumeSettingUpdater : MonoBehaviour
{
    public void SetBGMultiplier()
    {
        VolumeSettings.BGMultiplier = this.gameObject.GetComponent<Slider>().value;
    }
}
