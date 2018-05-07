using System;
using UnityEngine;

public class SandManager : MonoBehaviour {
    public const int Size = 37;


    public GameObject blockPrefab;
    public GameObject parent;

    private int[,,] sand;
    private bool[,,] dirty;
    private bool stable = true;

    void Start() {
        sand = new int[Size, Size, Size];
        dirty = new bool[Size, Size, Size];

        Camera.main.transform.position = new Vector3(Size / 2, Size / 2, -Size);

        PrepareBlocks();

        sand[Size / 2, Size / 2, Size / 2] = 10000;
        dirty[Size / 2, Size / 2, Size / 2] = true;
        stable = false;
    }

    private void PrepareBlocks() {
        for (var x = 0; x < Size; x++) {
            for (var y = 0; y < Size; y++) {
                for (var z = 0; z < Size; z++) {
                    Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity, parent.transform);
                }
            }
        }
    }

    void Update() {
        Drop();

        if (!stable) {
            Stabilize2();
        }
    }

    private void Drop() {
        sand[Size / 2, Size / 2, Size / 2] += 16;
        dirty[Size / 2, Size / 2, Size / 2] = true;
        stable = false;
    }

    private void Stabilize() {
        var stable = true;
        var count = 0;
        do {
            stable = true;
            count++;

            for (var x = 0; x < Size; x++) {
                for (var y = 0; y < Size; y++) {
                    for (var z = 0; z < Size; z++) {
                        if (sand[x, y, z] >= 6) {
                            var s = sand[x, y, z];

                            var delta = s - s % 6;
                            var dec = delta / 6;

                            sand[x, y, z] -= delta;
                            dirty[x, y, z] = true;

                            Increment(x - 1, y, z, dec);
                            Increment(x + 1, y, z, dec);
                            Increment(x, y - 1, z, dec);
                            Increment(x, y + 1, z, dec);
                            Increment(x, y, z - 1, dec);
                            Increment(x, y, z + 1, dec);

                            stable = false;
                        }
                    }
                }
            }
        } while (!stable && count < 4);

        this.stable = stable;
    }

    private void Stabilize2() {
        var stable = true;
        var count = 0;
        do {
            stable = true;
            count++;

            for (var x = 0; x < Size; x++) {
                for (var y = 0; y < Size; y++) {
                    for (var z = 0; z < Size; z++) {
                        if (sand[x, y, z] >= 6) {
                            sand[x, y, z] -= 6;
                            dirty[x, y, z] = true;

                            Increment(x - 1, y, z);
                            Increment(x + 1, y, z);
                            Increment(x, y - 1, z);
                            Increment(x, y + 1, z);
                            Increment(x, y, z - 1);
                            Increment(x, y, z + 1);

                            stable = false;
                        }
                    }
                }
            }
        } while (!stable && count < 4);
    }

    private void Increment(int x, int y, int z, int increment = 1) {
        if (x >= 0 && x < Size
            && y >= 0 && y < Size
            && z >= 0 && z < Size) {
            sand[x, y, z] += increment;
            dirty[x, y, z] = true;
        }
    }

    public int GetAmount(int x, int y, int z) {
        return sand[x, y, z];
    }

    public bool IsDirty(int x, int y, int z) {
        return dirty[x, y, z];
    }

    public void ResetDirtyFlag(int x, int y, int z) {
        dirty[x, y, z] = false;
    }
}
