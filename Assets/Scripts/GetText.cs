using TMPro;
using UnityEngine;

public class GetText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifeTime = 1f;

    private TMP_Text textComponent;
    private Color startColor;
    private float timer;

    private void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();

        if (textComponent == null)
        {
            Debug.LogError("TMP_Textが見つかりません");
            return;
        }

        startColor = textComponent.color;
    }

    private void Update()
    {
        if (textComponent == null)
            return;

        timer += Time.deltaTime;

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        float alpha = Mathf.Clamp01(1f - timer / lifeTime);

        textComponent.color = new Color(
            startColor.r,
            startColor.g,
            startColor.b,
            alpha
        );

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}