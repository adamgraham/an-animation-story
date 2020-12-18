using UnityEngine;
using UnityEditor;

public sealed class PrefabEditorUtils
{
	[UnityEditor.MenuItem("Tools/Prefabs/Revert Selected Prefabs")]
	private static void RevertSelectedPrefabs()
	{
		GameObject[] selection = Selection.gameObjects;

		for (int i = 0; i < selection.Length; i++) {
			PrefabUtility.RevertPrefabInstance(selection[i]);
		}
	}

	[UnityEditor.MenuItem("Tools/Prefabs/Replace Selected Prefabs")]
	private static void ReplaceSelectedPrefabs()
	{
		ScriptableWizard.DisplayWizard("Replace Selected Prefabs", typeof(ReplacePrefabs), "Replace");
	}

}

[System.Serializable]
public sealed class ReplacePrefabs : ScriptableWizard
{
	public bool copyTransform = true;
	public bool keepChildren = false;

	public GameObject newPrefab;

	private void OnWizardCreate()
	{
		for (int i = 0; i < Selection.gameObjects.Length; i++)
		{
			GameObject selection = Selection.gameObjects[i];
			GameObject replacement = (GameObject)PrefabUtility.InstantiatePrefab(this.newPrefab);

			replacement.transform.parent = selection.transform.parent;

			if (this.copyTransform)
			{
				replacement.transform.position = selection.transform.position;
				replacement.transform.rotation = selection.transform.rotation;
				replacement.transform.localScale = selection.transform.localScale;
			}

			if (this.keepChildren)
			{
				Transform[] children = selection.GetComponentsInChildren<Transform>();
				
				for (int j = 0; j < children.Length; j++)
				{
					if (children[j] != selection) {
						children[j].parent = replacement.transform;
					}
				}
			}

			DestroyImmediate(selection);
		}
	}

}
