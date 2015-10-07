using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SpiderMove))]
public class EditorSpiderMove : Editor {

	public override void OnInspectorGUI() {
		var _GUIStyleNormal = new GUIStyle(GUI.skin.window);
		_GUIStyleNormal.wordWrap = true;
		_GUIStyleNormal.alignment = TextAnchor.UpperLeft;
		
		var _GUIStyleHeader = new GUIStyle(GUI.skin.window);
		_GUIStyleHeader.wordWrap = true;
		_GUIStyleHeader.alignment = TextAnchor.UpperLeft;
		_GUIStyleHeader.fontSize = 12;
		
		var _GUIStyleLarge = new GUIStyle(GUI.skin.label);
		_GUIStyleLarge.wordWrap = true;
		_GUIStyleLarge.alignment = TextAnchor.UpperLeft;
		_GUIStyleLarge.fontSize = 16;
		_GUIStyleLarge.fontStyle = FontStyle.Bold;
		
		var _GUIStyleLabel = new GUIStyle(GUI.skin.label);
		_GUIStyleLabel.wordWrap = true;
		_GUIStyleLabel.alignment = TextAnchor.UpperLeft;

		SpiderMove mainScript = (SpiderMove) target; 

		//player prefab
		GUILayout.BeginVertical("GameObjects: ", _GUIStyleHeader);
		mainScript.SpiderModel = EditorGUILayout.ObjectField("Spider model", mainScript.SpiderModel, typeof(Object), true) as GameObject;
		mainScript.CheckFront = EditorGUILayout.ObjectField("Sensor Front", mainScript.CheckFront, typeof(Object), true) as GameObject;
		mainScript.CheckFrontBelow = EditorGUILayout.ObjectField("Sensor FrontBelow", mainScript.CheckFrontBelow, typeof(Object), true) as GameObject;
		mainScript.CheckBelow = EditorGUILayout.ObjectField("Sensor Below", mainScript.CheckBelow, typeof(Object), true) as GameObject;
		mainScript.CheckBackBelow = EditorGUILayout.ObjectField("Sensor BackBelow", mainScript.CheckBackBelow, typeof(Object), true) as GameObject;
		GUILayout.EndVertical();
	
		GUILayout.BeginVertical("Settings: ", _GUIStyleHeader);
		// min max slider SPEED
		mainScript.MinSpeed = mainScript.tempMinSpeed;
		mainScript.MaxSpeed = mainScript.tempMaxSpeed;
		GUIContent PropContent = new GUIContent("Speed " + mainScript.MinSpeed.ToString("F1")+" - " + mainScript.MaxSpeed.ToString("F1"));
		EditorGUILayout.MinMaxSlider(PropContent, ref mainScript.tempMinSpeed, ref mainScript.tempMaxSpeed, 0.1f, 4);

		// min max slider SIZE
		mainScript.MinSize = mainScript.tempMinSize;
		mainScript.MaxSize = mainScript.tempMaxSize;
		GUIContent PropContent2 = new GUIContent("Size " + mainScript.MinSize.ToString("F1")+" - " + mainScript.MaxSize.ToString("F1"));
		EditorGUILayout.MinMaxSlider(PropContent2, ref mainScript.tempMinSize, ref mainScript.tempMaxSize, 0.5f, 6);

		// min max slider ChOICE MAKING
		mainScript.SpiderChoice = EditorGUILayout.IntSlider("ChoiceDelay", mainScript.SpiderChoice, 10, 200);

		mainScript.CanProduceSilk = GUILayout.Toggle(mainScript.CanProduceSilk,"Can produce silk");
		if(mainScript.CanProduceSilk == true){
			mainScript.SilkStringModel = EditorGUILayout.ObjectField("Silk string model", mainScript.SilkStringModel, typeof(Object), true) as GameObject;
		}
		GUILayout.EndVertical();
	}
}
