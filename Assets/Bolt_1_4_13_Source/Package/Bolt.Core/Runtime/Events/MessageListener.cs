using System;
using System.Linq;
using Ludiq;
using UnityEngine;

namespace Bolt
{
	[DisableAnnotation]
	[AddComponentMenu("")]
	[IncludeInSettings(false)]
	public abstract class MessageListener : MonoBehaviour
	{
		private static Type[] _listenerTypes;

		public static Type[] listenerTypes
		{
			get
			{
				if (_listenerTypes == null)
				{
					_listenerTypes = RuntimeCodebase.types
					                                .Where(t => typeof(MessageListener).IsAssignableFrom(t) && t.IsConcrete())
					                                .ToArray();
				}

				return _listenerTypes;
			}
		}

		public static void AddTo(GameObject gameObject)
		{
			foreach (var listenerType in listenerTypes)
			{
				if (gameObject.GetComponent(listenerType) == null)
				{
					gameObject.AddComponent(listenerType);
				}
			}
		}
	}
}
