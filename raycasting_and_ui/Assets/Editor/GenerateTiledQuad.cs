using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateTiledQuad : MonoBehaviour {

	static Texture t;

	[MenuItem("YTU/Generate Tiled Quad")]
	static void GenerateQuad() {
		t = Resources.Load<Texture> ("aesthetics/cloud_texture");
		GameObject new_quad = new GameObject ();
		new_quad.name = "tiled_quad";

		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				GameObject new_cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
                Material mat = Resources.Load<Material>("aesthetics/Materials/block_mat");
                new_cube.GetComponent<Renderer>().material = mat;
                mat.mainTexture = t;

                // HSB color representation is more convenient
                // for use with color palettes.
                HSBColor desired_color = new HSBColor(0.3f, 0.6f, 1.0f, 1.0f);

				if (i % 2 == 0) {
					if (j % 2 == 0) {
                        desired_color = new HSBColor(0.6f, 0.6f, 1.0f, 1.0f);
                    } else {
                        desired_color = new HSBColor(0.6f, 0.6f, 0.6f, 1.0f);
                    }
				} else {
					if (j % 2 == 0) {
                        desired_color = new HSBColor(0.6f, 0.6f, 0.6f, 1.0f);
                    } else {
                        desired_color = new HSBColor(0.6f, 0.6f, 1.0f, 1.0f);
                    }
				}

                new_cube.GetComponent<Renderer>().material.color = desired_color.ToColor();

				new_cube.transform.SetParent (new_quad.transform);
				new_cube.transform.localPosition = new Vector3 (i, 0, j);
				new_cube.name = "tiled_cube";
			}
		}
	}
}
