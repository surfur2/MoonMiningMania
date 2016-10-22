using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour {

    private Toggle myToggle;

    void Start()
    {
        myToggle = this.gameObject.GetComponent<Toggle>();
        myToggle.isOn = GameManager.isMute;
    }

    public void Mute(bool mute)
    {
        AudioListener.volume = mute ? 0 : 1;
        GameManager.isMute = mute;
    }
}
