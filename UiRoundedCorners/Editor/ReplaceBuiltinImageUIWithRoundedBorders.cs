using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace Nobi.UiRoundedCorners {
	public static class ReplaceBuiltinImageUIWithRoundedBorders {
		static Sprite UISprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
		static Sprite Background = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
		static Sprite InputFieldBackground = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
		static bool ReplaceImg(Image img) {
			if (
				img != null
				&&
				(
					UISprite == img.sprite
					||
					Background == img.sprite
					||
					InputFieldBackground == img.sprite
				)
			) {
				img.sprite = null;
				Debug.Log(" Removed UISprite from Image component", img);

				if ( // if already has some  associated script, ignore
				    !img.GetComponent<ImageWithRoundedBorders>()
				    &&
				    !img.GetComponent<ImageWithRoundedCorners>()
				    &&
				    !img.GetComponent<ImageWithIndependentRoundedCorners>()
				   ) {
					img.gameObject.AddComponent<ImageWithRoundedBorders>();
				}
				return true;

			}
			return false;
		}

		[MenuItem("Assets/Replace selected builtin UI with RoundedBorders", false, 200)]
		static void ReplaceSpritesWithShader() {
			if ( Selection.gameObjects.Length == 0 ) {
				EditorUtility.DisplayDialog("Replace selected builtin UI with RoundedBorders", "Please select a GameObject/GameObjects to replace", "OK");
				return;
			}

			bool children = EditorUtility.DisplayDialog("Replace selected builtin UI with RoundedBorders", "Replace builtin UI elements also in children of selected objects?", "Yes", "No");

			GameObject[] gos = Selection.gameObjects;
			int removed = 0;
			foreach ( GameObject go in gos ) {
				if ( ReplaceImg(go.GetComponent<Image>()) ) {
					removed++;
				}
				if ( children ) {
					Image[] rs = go.GetComponentsInChildren<Image>(true);
					// Debug.Log("transforms " + rs.Length);
					for ( int i = rs.Length - 1; i >= 0; i-- ) {
						if ( rs[i] == null ) {
							// Debug.Log("null " + i);
							continue;
						}
						if ( ReplaceImg(rs[i]) ) {
							removed++;
						}
					}
				}
			}
			Debug.Log("Replaced " + (removed) + " Image components!");
		}
	}

}
