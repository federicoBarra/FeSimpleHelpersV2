using Tymski;

namespace FeSimpleHelpers.ScenesHandling
{
	public interface ISceneInfoProvider
	{
		ISceneInfoProvider NextSceneInfoProvider { get; }
		SceneReference CurrentScene { get; }
	}
}