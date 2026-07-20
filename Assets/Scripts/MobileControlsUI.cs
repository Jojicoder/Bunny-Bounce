using UnityEngine;

public class MobileControlsUI : MonoBehaviour
{
    [Tooltip("LeftButton, RightButton, JumpButtonをまとめた親オブジェクト")]
    public GameObject controlsRoot;

    private void Start()
    {
        // WebGLではブラウザ(index.html)側からSetIsMobileが呼ばれるまで非表示にしておく。
        // それ以外(エディタ実行など)はInput.touchSupportedで代用する。
#if !UNITY_WEBGL || UNITY_EDITOR
        controlsRoot.SetActive(Input.touchSupported);
#else
        controlsRoot.SetActive(false);
#endif
    }

    // index.htmlからunityInstance.SendMessageで呼び出される
    public void SetIsMobile(string value)
    {
        controlsRoot.SetActive(value == "true");
    }
}
