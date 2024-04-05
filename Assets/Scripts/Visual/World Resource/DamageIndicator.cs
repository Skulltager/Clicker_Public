using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Text textField;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float initialMinForce;
    [SerializeField] private float initialMaxForce;
    [SerializeField] private float initialForceMaxAngle;
    [SerializeField] private float initialVerticalMaxOffset;
    [SerializeField] private float initialHorizontalMaxOffset;
    [SerializeField] private float lifeTime;
    [SerializeField] private float drag;
    [SerializeField] private Color critLevel0Color;
    [SerializeField] private Color critLevel1Color;
    [SerializeField] private Color critLevel2Color;
    [SerializeField] private Color critLevel3Color;
    [SerializeField] private Color critLevel4Color;

    private Vector3 force;
    private Vector3 worldPosition;
    private float lifeTimeRemaining;

    private RectTransform canvasRectTransform;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasRectTransform = GetComponentInParent<Canvas>().transform as RectTransform;
        rectTransform = transform as RectTransform;
    }

    public void Initialize(Vector3 startPosition, int damage, int critLevel)
    {
        worldPosition = startPosition;
        string damageText = damage.ToString() + GetCritLevelText(critLevel);
        textField.color = GetCritLevelColor(critLevel);

        textField.text = damageText;

        float verticalOffset = Random.Range(0, initialVerticalMaxOffset);
        float horizontalOffset = Random.Range(0, initialHorizontalMaxOffset);
        Vector3 initialOffset = Random.onUnitSphere;
        initialOffset.x *= horizontalOffset;
        initialOffset.z *= horizontalOffset;
        initialOffset.y *= verticalOffset;

        worldPosition += initialOffset;

        float offset = Random.Range(0, initialForceMaxAngle);
        float angle = Random.Range(0, 360f);

        float initialForce = Random.Range(initialMinForce, initialMaxForce);
        Vector3 randomRotation = Vector3.RotateTowards(Vector3.up, Vector3.right, offset * Mathf.Deg2Rad, 0);
        force = randomRotation.RotateAround(Vector3.up, angle) * initialForce;
        lifeTimeRemaining = lifeTime;
        SetPosition();
    }

    private Color GetCritLevelColor(int critLevel)
    {
        switch(critLevel)
        {
            case 0:
                return critLevel0Color;
            case 1:
                return critLevel1Color;
            case 2:
                return critLevel2Color;
            case 3:
                return critLevel3Color;
            default:
            case 4:
                return critLevel4Color;
        }
    }

    private string GetCritLevelText(int critLevel)
    {
        switch (critLevel)
        {
            case 0:
                return "";
            case 1:
                return "!";
            case 2:
                return "!!";
            case 3:
                return "!!!";
            default:
            case 4:
                return "!!!!";
        }
    }

    private void Update()
    {
        lifeTimeRemaining -= Time.deltaTime;
        if(lifeTimeRemaining <= 0)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        force *= 1 - Time.deltaTime * drag;
        worldPosition += force * Time.deltaTime;
        SetPosition();
    }

    private void SetPosition()
    {
        Vector3 difference = worldPosition - Camera.main.transform.position;

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        screenPoint.x = screenPoint.x / Screen.width * canvasRectTransform.sizeDelta.x;
        screenPoint.y = screenPoint.y / Screen.height * canvasRectTransform.sizeDelta.y;
        rectTransform.anchoredPosition = screenPoint;
        canvasGroup.alpha = Vector3.Dot(Camera.main.transform.forward, difference) > 0 ? 1 : 0;
    }
}