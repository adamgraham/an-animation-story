using UnityEngine;

public sealed class SpriteDirectionFlipper : MonoBehaviour
{
	public enum Direction { Forward, Backward, Left, Right, Idle };
	public enum DirectionPriority { Vertical, Horizontal };

	[Header("References")]

	public Transform movementTransform;

	public GameObject defaultSide;
	public GameObject frontSide;
	public GameObject backSide;
	public GameObject leftSide;
	public GameObject rightSide;

	[Header("Switching")]

	private Vector3 _previousPosition;
	private GameObject _currentSide;
	private Direction _direction;

	[Header("Priority")]

	public DirectionPriority priority;

	[Header("Cooldown")]

	public float cooldown;

	private bool _cooldown;

	private void Start()
	{
		StoreInitialData();
	}

	private void OnEnable()
	{
		StoreInitialData();
	}

	private void StoreInitialData()
	{
		if (this.movementTransform != null) {
			_previousPosition = this.movementTransform.position;
		}
		
		if (_currentSide != null) {
			return;
		}

		DisableAllSides();

		if (this.defaultSide != null)
		{
			if (this.defaultSide == this.frontSide) {
				Switch(Direction.Backward);
			} else if (this.defaultSide == this.backSide) {
				Switch(Direction.Forward);
			} else if (this.defaultSide == this.leftSide) {
				Switch(Direction.Left);
			} else if (this.defaultSide == this.rightSide) {
				Switch(Direction.Right);
			}
		}
	}

	private void LateUpdate()
	{
		if (this.movementTransform == null) {
			return;
		}

		if (this.priority == DirectionPriority.Horizontal)
		{
			if (this.movementTransform.position.x < _previousPosition.x) {
				Switch(Direction.Left);
			} else if (this.movementTransform.position.x > _previousPosition.x) {
				Switch(Direction.Right);
			} else if (this.movementTransform.position.z > _previousPosition.z) {
				Switch(Direction.Forward);
			} else if (this.movementTransform.position.z < _previousPosition.z) {
				Switch(Direction.Backward);
			}
		}
		else
		{
			if (this.movementTransform.position.z > _previousPosition.z) {
				Switch(Direction.Forward);
			} else if (this.movementTransform.position.z < _previousPosition.z) {
				Switch(Direction.Backward);
			} else if (this.movementTransform.position.x < _previousPosition.x) {
				Switch(Direction.Left);
			} else if (this.movementTransform.position.x > _previousPosition.x) {
				Switch(Direction.Right);
			}
		}
		
		_previousPosition = this.movementTransform.position;
	}

	public void Switch(Direction direction)
	{
		if (direction == _direction) {
			return;
		}

		_direction = direction;

		if (_currentSide != null)
		{
			_currentSide.gameObject.SetActive(false);
			_currentSide = null;
		}

		switch (_direction)
		{
			case Direction.Forward:
				_currentSide = this.backSide; break;
			case Direction.Backward:
				_currentSide = this.frontSide; break;
			case Direction.Left:
				_currentSide = this.leftSide; break;
			case Direction.Right:
				_currentSide = this.rightSide; break;
		}

		if (_currentSide == null) {
			_currentSide = this.defaultSide;
		}

		if (_currentSide != null) {
			_currentSide.gameObject.SetActive(true);
		}
	}

	public void DisableAllSides()
	{
		if (this.frontSide != null) this.frontSide.gameObject.SetActive(false);
		if (this.backSide != null) this.backSide.gameObject.SetActive(false);
		if (this.leftSide != null) this.leftSide.gameObject.SetActive(false);
		if (this.rightSide != null) this.rightSide.gameObject.SetActive(false);

		_currentSide = null;
		_direction = Direction.Idle;
	}

	public void StartCooldown()
	{
		_cooldown = true;

		if (this.cooldown > 0.0f) {
			Invoke("StopCooldown", this.cooldown);
		} else {
			StopCooldown();
		}
	}

	public void StopCooldown()
	{
		_cooldown = false;
	}

}
