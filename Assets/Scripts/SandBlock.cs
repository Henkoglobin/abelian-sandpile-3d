using UnityEngine;

public class SandBlock : MonoBehaviour {
    private SandManager manager;
    private Material material;
    private Renderer myRenderer;

    private int x;
    private int y;
    private int z;

    private bool updated = false;

    void Start() {
        material = GetComponent<Renderer>().material;
        manager = GetComponentInParent<SandManager>();
        myRenderer = GetComponent<Renderer>();

        x = (int)transform.position.x;
        y = (int)transform.position.y;
        z = (int)transform.position.z;
    }

    void Update() {
        if(updated && !manager.IsDirty(x, y, z)) {
            return;
        }

        updated = true;

        var amount = manager.GetAmount(x, y, z);

        myRenderer.enabled = amount > 0;

        if (amount > 0) {
            material.color = GetColor(amount);
            var scale = amount <= 5 ? amount / 5f : 1.0f;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        manager.ResetDirtyFlag(x, y, z);
    }

    private Color GetColor(int amount) {
        if(amount >= 6) {
            return Color.red;
        }

        return Color.Lerp(Color.black, Color.white, amount / 6.0f);
    }
}
