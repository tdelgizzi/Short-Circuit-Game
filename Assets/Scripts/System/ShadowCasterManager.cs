using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowCasterManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] GameObject shadowCastPrefab;

    public void CreateShadowCasters()
    {
        // Remove previous shadow casters
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Spawn new ones in
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    SpawnPrefab(new Vector3(x, y, 0));
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
    }

    private void SpawnPrefab(Vector3 position)
    {
        Instantiate(shadowCastPrefab, position, Quaternion.identity, transform);
    }
}
