using UnityEngine;

public sealed class ScreenResizeEvent : MonoBehaviour 
{
	public delegate void OnScreenResize(Vector2 newSize);
	public static OnScreenResize onScreenResize;

	private static ScreenResizeEvent _instance;

	private static Vector3 _screenSize;
	private static Vector3 _screenCenter;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			_instance.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;

			StoreScreenSize();
		}
		else
		{
			StoreScreenSize();
			DestroyImmediate(this);
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			ScreenResizeEvent.onScreenResize = null;

			_instance = null;
		}
	}

	private void Update()
	{
		if (_screenSize.x != Screen.width || _screenSize.y != Screen.height)
		{
			StoreScreenSize();

			if (ScreenResizeEvent.onScreenResize != null) {
				ScreenResizeEvent.onScreenResize(_screenSize);
			}
		}
	}

	private static void StoreScreenSize()
	{
		_screenSize = new Vector3(Screen.width, Screen.height, 0.0f);
		_screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
	}

	public static Vector3 GetScreenSize()
	{
		return _screenSize;
	}

	public static Vector3 GetScreenCenter()
	{
		return _screenCenter;
	}

}
