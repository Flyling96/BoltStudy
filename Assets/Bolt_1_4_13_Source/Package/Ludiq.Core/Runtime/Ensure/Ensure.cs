using UnityEngine;

namespace Ludiq
{
	public static class Ensure
	{
		private static readonly EnsureThat instance = new EnsureThat();

		public static bool IsActive { get; set; }

		public static void Off() => IsActive = false;

		public static void On() => IsActive = true;

		public static EnsureThat That(string paramName)
		{
			instance.paramName = paramName;
			return instance;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void OnRuntimeMethodLoad()
		{
			IsActive = Application.isEditor || Debug.isDebugBuild;
		}
	}
}