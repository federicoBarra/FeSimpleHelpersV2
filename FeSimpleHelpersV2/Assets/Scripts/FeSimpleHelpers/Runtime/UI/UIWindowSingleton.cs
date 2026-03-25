namespace FeSimpleHelpers.UI
{
	/// <summary>
	/// Singleton UIWindow. This is useful for things like debug windows, loading screens, or other UI elements where you only want one instance of it and want to be able to easily access it from anywhere.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class UIWindowSingleton<T> : UIWindow where T : UIWindow
	{
		public static T Instance { get; private set; } = null;

		public static T Get() { return Instance; }

		protected override void Awake()
		{
			base.Awake();
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this as T;
		}
	}
}