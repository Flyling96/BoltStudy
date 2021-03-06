using UnityEditor;

namespace Bolt
{
	[CustomEditor(typeof(MessageListener), true)]
	public class MessageListenerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox("This component is automatically added to relay Unity messages to Bolt.", MessageType.Info);
		}
	}
}