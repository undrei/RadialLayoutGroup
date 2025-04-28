using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RadialLayoutGroup : LayoutGroup
{
	[SerializeField] 
	private float radius = 100f;
	[SerializeField] 
	private float startAngle = 0f;

	[SerializeField]
	private bool fillCircle = true;
	[SerializeField] 
	private float space = 30f;


	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		ArrangeChildren();
	}

	public override void CalculateLayoutInputVertical()
	{
		ArrangeChildren();
	}

	public override void SetLayoutHorizontal()
	{
	}
	public override void SetLayoutVertical()
	{
	}

	private void ArrangeChildren()
	{
		int activeChildCount = 0;
		for (int i = 0; i < rectChildren.Count; i++)
		{
			if (rectChildren[i].gameObject.activeSelf) activeChildCount++;
		}

		if (activeChildCount == 0) 
			return;

		float angleStep;
		if (fillCircle) angleStep = 360f / activeChildCount;
		else angleStep = space;

		float currentAngle = startAngle;

		Vector2 center = GetAlignmentOffset();

		for (int i = 0; i < rectChildren.Count; i++)
		{
			RectTransform child = rectChildren[i];
			if (!child.gameObject.activeSelf) 
				continue;

			Vector2 pos = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;

			SetChildAlongAxis(child, 0, center.x + pos.x - (child.rect.width * 0.5f));
			SetChildAlongAxis(child, 1, center.y + pos.y - (child.rect.height * 0.5f));

			currentAngle += angleStep;
		}
	}

	private Vector2 GetAlignmentOffset()
	{
		Rect rect = rectTransform.rect;
		Vector2 pivot = Vector2.zero;

		switch (childAlignment)
		{
			case TextAnchor.UpperLeft:
				pivot = new Vector2(0, 1);
				break;
			case TextAnchor.UpperCenter:
				pivot = new Vector2(0.5f, 1);
				break;
			case TextAnchor.UpperRight:
				pivot = new Vector2(1, 1);
				break;
			case TextAnchor.MiddleLeft:
				pivot = new Vector2(0, 0.5f);
				break;
			case TextAnchor.MiddleCenter:
				pivot = new Vector2(0.5f, 0.5f);
				break;
			case TextAnchor.MiddleRight:
				pivot = new Vector2(1, 0.5f);
				break;
			case TextAnchor.LowerLeft:
				pivot = new Vector2(0, 0);
				break;
			case TextAnchor.LowerCenter:
				pivot = new Vector2(0.5f, 0);
				break;
			case TextAnchor.LowerRight:
				pivot = new Vector2(1, 0);
				break;
		}
		
		float paddedWidth = rect.width - padding.left - padding.right;
		float paddedHeight = rect.height - padding.top - padding.bottom;

		float offsetX = padding.left + paddedWidth * pivot.x;
		float offsetY = padding.bottom + paddedHeight * pivot.y;

		return new Vector2(offsetX, offsetY);
	}
}
