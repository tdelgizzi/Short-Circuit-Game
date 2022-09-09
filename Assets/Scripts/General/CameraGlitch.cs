using System.Collections;
using UnityEngine;

public class CameraGlitch : MonoBehaviour {

    [SerializeField] Material glitchMaterial;

    public float GetSplitFactor() {
        return glitchMaterial.GetFloat("_SplitFactor");
    }

    public float GetStaticFactor() {
        return glitchMaterial.GetFloat("_StaticFactor");
    }

    public void SetSplitFactor(float f) {
        glitchMaterial.SetFloat("_SplitFactor", f);
    }

    public void SetStaticFactor(float f) {
        glitchMaterial.SetFloat("_StaticFactor", f);
    }

    public float GetLineFactor() {
        return glitchMaterial.GetFloat("_LineFactor");
    }

    public float GetLineSpeed() {
        return glitchMaterial.GetFloat("_ScrollSpeed");
    }

    public void SetLineFactorAndSpeed(float f, float s) {
        glitchMaterial.SetFloat("_LineFactor", f);
        glitchMaterial.SetFloat("_ScrollSpeed", s);
    }
}
