using System;
using UnityEngine;

namespace Ludiq
{
	public abstract class LudiqScriptableObject : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField, DoNotSerialize] // Serialize with Unity, but not with FullSerializer.
		protected SerializationData _data;

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			// Ignore the FullSerializer callback, but still catch the Unity callback
			if (Serialization.isCustomSerializing)
			{
				return;
			}

			Serialization.isUnitySerializing = true;

			try
			{
				OnBeforeSerializeLudiq();
				_data = this.Serialize(true);
                //Debug.Log($"[_LudiqScriptableObject.OnBeforeSerialize] {GetType()}, {_data.json}");
                OnAfterSerializeLudiq();
			}
			catch (Exception ex)
			{
				// Don't abort the whole serialization thread because this one object failed
				Debug.LogError($"Failed to serialize scriptable object.\n{ex}", this);
			}

			Serialization.isUnitySerializing = false;
		}

		public void OnAfterDeserialize()
		{
			// Ignore the FullSerializer callback, but still catch the Unity callback
			if (Serialization.isCustomSerializing)
			{
				return;
			}

			Serialization.isUnitySerializing = true;

            try
			{
				object @this = this;
				OnBeforeDeserializeLudiq();
				_data.DeserializeInto(ref @this, true);
                //Debug.Log($"[_LudiqScriptableObject.OnAfterDeserialize] {GetType()}, {_data.json}");
                OnAfterDeserializeLudiq();
				UnityThread.EditorAsync(OnPostDeserializeInEditor);
			}
			catch (Exception ex)
			{
				// Don't abort the whole deserialization thread because this one object failed
				Debug.LogError($"Failed to deserialize scriptable object.\n{ex}", this);
			}

			Serialization.isUnitySerializing = false;
		}

		protected virtual void OnBeforeSerializeLudiq() { }

		protected virtual void OnAfterSerializeLudiq() { }

		protected virtual void OnBeforeDeserializeLudiq() { }

		protected virtual void OnAfterDeserializeLudiq() { }

		protected virtual void OnPostDeserializeInEditor() { }

		protected virtual void ShowData()
		{
			_data.ShowString(ToString());
		}

		public override string ToString()
		{
			return this.ToSafeString();
		}
	}
}