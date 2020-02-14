//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2019 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the camera's field of view.
/// </summary>

[RequireComponent(typeof(Light))]
[AddComponentMenu("NGUI/Tween/Tween Intensity")]
public class TweenIntensity : UITweener
{
	public float from = 0;
	public float to = 1;

	Light mlight;

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
	public Camera cachedCamera { get { if (mCam == null) mCam = camera; return mCam; } }
#else
	public Light cachedCamera { get { if (mlight == null) mlight = GetComponent<Light>(); return mlight; } }
#endif

	[System.Obsolete("Use 'value' instead")]
	public float fov { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public float value { get { return cachedCamera.intensity; } set { cachedCamera.intensity = value; } }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV comp = UITweener.Begin<TweenFOV>(go, duration);
		comp.from = comp.value;
		comp.to = to;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue() { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue() { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart() { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd() { value = to; }
}
