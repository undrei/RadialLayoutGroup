using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
	[SerializeField] 
	private RadialMenuPointer pointer;
	[SerializeField] 
	private List<Button> menuElements = new List<Button>();
	[SerializeField]
	private int currentPointerIndex = 0;
	[SerializeField] 
	private float rotationSpeed = 720f;

	private RectTransform rectTransform;
	private RadialLayoutGroup layoutGroup;
	private float currentAngle; 
	private float pointerTargetAngle;
	
	private float MenuTotalAngle => layoutGroup == null || layoutGroup.fullCircle ? 360f : layoutGroup.angleSpace * menuElements.Count;
	
	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		layoutGroup = GetComponent<RadialLayoutGroup>();

		Init();
	}

	private void OnDestroy()
	{
		menuElements.ForEach(button => button.onClick.RemoveAllListeners());
	}

	[ContextMenu("Reinitialize")]
	private void Init()
	{
		SetupElements();
		
		pointer.angle = MenuTotalAngle / menuElements.Count;
		pointer.radius = GetComponent<RectTransform>().rect.width / 2 + 48;
	}

	private void SetupElements()
	{
		OnDestroy();
		menuElements.Clear();
		
		foreach (Button button in GetComponentsInChildren<Button>())
		{
			menuElements.Add(button);
			Debug.Log(button.name);
			button.onClick.AddListener(() => 
			{
				StartCoroutine(HandleButtonPress(button.transform));
				Debug.Log($"Clicked on [{button.name}]", button);
			});
		}
	}
	
	private void Update()
	{
		UpdatePointer();
	}

	private void UpdatePointer()
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out Vector2 localMousePosition);
		
		float angle = Mathf.Atan2(localMousePosition.y, localMousePosition.x) * Mathf.Rad2Deg;
		angle = (angle + 360f) % 360f;
		
		angle = (angle - layoutGroup.startAngle + 360f) % 360f;

		float sectorAngle = MenuTotalAngle / menuElements.Count;
		angle = (angle + sectorAngle * 0.5f) % 360f;

		currentPointerIndex = Mathf.FloorToInt(angle / sectorAngle);
		pointerTargetAngle = layoutGroup.startAngle + sectorAngle * currentPointerIndex;

		if (Input.GetMouseButtonDown(0))
		{
			int currentActualIndex = Mathf.FloorToInt((layoutGroup.clockwise ? -angle : angle) / sectorAngle);
			currentActualIndex = (currentActualIndex + menuElements.Count) % menuElements.Count;

			menuElements[currentActualIndex]?.onClick?.Invoke();
		}

		MovePointer();
	}
	
	private void MovePointer()
	{
		currentAngle = Mathf.MoveTowardsAngle(currentAngle, pointerTargetAngle, rotationSpeed * Time.deltaTime);
		pointer.SetPosition(currentAngle);
	}
	
	private IEnumerator HandleButtonPress(Transform buttonTransform)
	{
		Vector3 originalScale = buttonTransform.localScale;
		
		buttonTransform.localScale = originalScale * 0.9f;
		yield return new WaitForSeconds(0.1f);
		
		buttonTransform.localScale = originalScale;
		yield return new WaitForSeconds(0.1f);
	}
}
