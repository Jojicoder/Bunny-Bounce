using UnityEngine;

public class MobileControlsUI : MonoBehaviour
{
    [Tooltip("LeftButton, RightButton, JumpButtonをまとめた親オブジェクト")]
    public GameObject controlsRoot;

    private void Start()
    {
        bool isTouchDevice = Input.touchSupported;

        controlsRoot.SetActive(isTouchDevice);
    }
}
