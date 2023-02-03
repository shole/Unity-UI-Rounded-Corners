using UnityEngine;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners {
	[RequireComponent(typeof(RectTransform))]
	public class ImageWithRoundedBorders : MonoBehaviour {
		private static readonly int Props = Shader.PropertyToID("_WidthHeightRadius");
		private static readonly int BorderOffset = Shader.PropertyToID("_borderOffset");
		private static readonly int BorderColorMul = Shader.PropertyToID("_borderColorMul");
		private static readonly int EdgePadding = Shader.PropertyToID("_edgePadding");

		public float radius = 10;
		public float borderOffset = 0.75f;
		public float borderColorMultiplier = 0.25f;
		public float edgePadding = 1f;
		private Material material;

		[HideInInspector, SerializeField] private MaskableGraphic image;

		private void OnValidate() {
			Validate();
			Refresh();
		}

		private void OnDestroy() {
			DestroyHelper.Destroy(material);
			image = null;
			material = null;
		}

		private void OnEnable() {
			Validate();
			Refresh();
		}

		private void OnRectTransformDimensionsChange() {
			if (enabled && material != null) {
				Refresh();
			}
		}

		public void Validate() {
			if (material == null) {
				material = new Material(Shader.Find("UI/RoundedCorners/RoundedBorders"));
			}

			if (image == null) {
				TryGetComponent(out image);
			}

			if (image != null) {
				image.material = material;
			}
		}

		public void Refresh() {
			var rect = ((RectTransform)transform).rect;
			material.SetVector(Props, new Vector4(rect.width, rect.height, radius, 0));
			material.SetFloat(BorderOffset,   borderOffset);
			material.SetFloat(BorderColorMul, borderColorMultiplier);
			material.SetFloat(EdgePadding,   edgePadding);
		}
	}
}