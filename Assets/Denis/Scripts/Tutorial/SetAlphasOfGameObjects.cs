using UnityEngine;

public class SetAlpha : MonoBehaviour
{
    public GameObject[] gameObjects; // Array of GameObjects to modify
    public float targetAlpha = 0.4f; // Target alpha value

    void Start()
    {
        SetGameObjectsAlpha(targetAlpha);
    }

    void SetGameObjectsAlpha(float alpha)
    {
        foreach (GameObject obj in gameObjects)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                // Create unique instances of the materials
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = new Material(materials[i]);
                    Color color = materials[i].color;
                    color.a = alpha;
                    materials[i].color = color;

                    // If using a shader that supports transparency, enable the appropriate keyword
                    materials[i].SetFloat("_Mode", 3); // 3 is the mode for transparent in Unity's Standard Shader
                    materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    materials[i].SetInt("_ZWrite", 0);
                    materials[i].DisableKeyword("_ALPHATEST_ON");
                    materials[i].EnableKeyword("_ALPHABLEND_ON");
                    materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    materials[i].renderQueue = 3000;
                }
                // Assign the new materials back to the renderer
                renderer.materials = materials;
            }
        }
    }
}
