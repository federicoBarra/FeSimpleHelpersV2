using FeSimpleHelpers.UI;

namespace FeSimpleHelpers.UI
{
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