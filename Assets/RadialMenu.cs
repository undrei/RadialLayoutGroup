using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
	[SerializeField] 
	private RadialMenuPointer pointer;
	[SerializeField] 
	private RadialMenuPointer outerPointer;
	[SerializeField] 
	private List<Button> menuElements = new List<Button>();
	[SerializeField]
	private int currentIndex = 0;
	[SerializeField] 
	private float rotationSpeed = 720f;

	private float currentAngle; 
	private float targetAngle;
	private float menuTotalAngle => GetComponent<RadialLayoutGroup>().fullCircle ? 360f : GetComponent<RadialLayoutGroup>().angleSpace * menuElements.Count;
	
	private void Start()
	{
		menuElements.Clear();
		
		foreach (Button button in GetComponentsInChildren<Button>())
		{
			menuElements.Add(button);
			button.onClick.AddListener(() => Debug.Log($"Menu element [{button.name}] clicked", button.gameObject));
		}
		
		outerPointer.angle = pointer.angle = menuTotalAngle / menuElements.Count;
		UpdatePointerToIndex(pointer, 0);
	}
	
	private void Update()
	{
		UpdatePointer();
		AnimatePointer();
	}

	private void UpdatePointer()
	{
		Vector2 localMousePosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, null, out localMousePosition);

		//Mouse position calc
		float angle = Mathf.Atan2(localMousePosition.y, localMousePosition.x) * Mathf.Rad2Deg;
		angle = (angle + 360f) % 360f;
		
		float sectorAngle = menuTotalAngle / menuElements.Count;
		angle = (angle + sectorAngle * 0.5f) % 360f;

		currentIndex = Mathf.FloorToInt(angle / sectorAngle);
		targetAngle = pointer.angle * currentIndex;

		if (Input.GetMouseButtonDown(0))
		{
			menuElements[currentIndex].onClick?.Invoke();
			UpdatePointerToIndex(outerPointer, currentIndex);
		}
	}
	
	private void AnimatePointer()
	{
		currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
		pointer.SetPosition(currentAngle);
	}
	
	private void UpdatePointerToIndex(RadialMenuPointer neededPointer, int index)
	{
		float targetAngle = neededPointer.angle * index;
		neededPointer.SetPosition(targetAngle);
	}
}
