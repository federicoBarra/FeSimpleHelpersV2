namespace FeSimpleHelpers.Core
{
	public class Singleton<T> where T : class, new()
	{
		private static T instance = null;

		protected Singleton() { }

		public static T Instance
		{
			get
			{
				if( instance == null )
					instance = new T();
				return instance;
			}
		}

		public static T Get() { return Instance; }
	}
}