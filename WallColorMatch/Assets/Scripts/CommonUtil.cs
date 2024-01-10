using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonUtil {

	public static void SetTransform (Transform tr, Vector3 position, Vector3 scale) {
		tr.localPosition = position;
		tr.localScale = scale;
	}

	public static int[] RandomNumbers(int maxCount, int n) {
		int[] defaults = new int[maxCount];
		int[] results = new int[n];

		for (int i = 0; i < maxCount; i++) {
			defaults[i] = i;
		}

		for (int i = 0; i < n; i++) {
			int index = Random.Range(0, maxCount);

			results[i] = defaults[index];
			defaults[index] = defaults[maxCount - 1];

			maxCount--;
		}

		return results;
	}
}
