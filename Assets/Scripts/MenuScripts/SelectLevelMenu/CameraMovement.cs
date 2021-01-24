using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    private int cameraSpeed;

	private Vector3 FirstTouchPosition;
	private Vector3 FinalTouchPosition;
	public  Vector2 pos;

	[SerializeField]
	private bool isEnablePlayerController= false;

	public float speed = 30f;
	public float minY = 10f;
	public float maxY = 80f;
	public float minX = 10f;
	public float maxX = 80f;

	[SerializeField]
	private GameObject[] levelButtons =null;

	private GameObject activeLevel = null;

	private void Start()
	{
		Background.OnTouchDownEventHandler += OnTouchDown;
		Background.OnTouchUpEventHandler += OnTouchUp;

		if (PlayerPrefs.GetInt("level") >= levelButtons.Length)
		{
			activeLevel = levelButtons[PlayerPrefs.GetInt("level") -1 ];
			return;
		}
		activeLevel = levelButtons[PlayerPrefs.GetInt("level")];
	}

	private void OnDisable()
	{
		Background.OnTouchDownEventHandler -= OnTouchDown;
		Background.OnTouchUpEventHandler -= OnTouchUp;
	}

	private void Update()
    {
		PlayerController();
		CameraMove(activeLevel);
	}

	private void OnTouchDown(Vector3 firstTouchPosition)
	{
		FirstTouchPosition = firstTouchPosition; 
	}

	private void OnTouchUp(Vector3 finalTouchPosition)
	{
		FinalTouchPosition = finalTouchPosition;
		CalculateTouchMove();
	}

	private void CalculateTouchMove()
	{
		if (!isEnablePlayerController)
			return;


		Vector3 distance = FinalTouchPosition - FirstTouchPosition;
		Vector3 targetPos = transform.position - (distance);
		targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
		targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

		StartCoroutine(CameraMove(targetPos, 0.5f));
	}

	private IEnumerator CameraMove( Vector3 target, float duration)
	{
		Vector3 diffVector = (target - transform.position);
		float diffLength = diffVector.magnitude;
		diffVector.Normalize();
		float counter = 0;
		while (counter < duration)
		{
			float movAmount = (Time.deltaTime * diffLength) / duration;
			transform.position += diffVector * movAmount;
			counter += Time.deltaTime;
			yield return null;
		}

		transform.position = target;
	}


	private void PlayerController()
	{
		if (!isEnablePlayerController)
			return;

		if (Input.GetKey("w"))
		{
			transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("s"))
		{
			transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("d"))
		{
			transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
		}
		if (Input.GetKey("a"))
		{
			transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
		}

		Vector3 pos = transform.position;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		pos.x = Mathf.Clamp(pos.x, minX, maxX);

		transform.position = pos;
	}

	private void CameraMove(GameObject activeLevel)
	{
		if (isEnablePlayerController)
			return;

		Vector2 tempPosition = activeLevel.transform.position;
		transform.position = Vector2.Lerp(transform.position, tempPosition, 0.01f);
		Vector3 pos = transform.position;

		pos.z = -10;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		pos.x = Mathf.Clamp(pos.x, minX, maxX);

		transform.position = pos;

		if ((int)tempPosition.x==(int)transform.position.x && (int)tempPosition.y == (int)transform.position.y)
		{
			isEnablePlayerController = true;
		}
		else if ((int)tempPosition.x == (int)transform.position.x || (int)tempPosition.y == (int)transform.position.y)
		{
			isEnablePlayerController = true;
		}
	}

}
