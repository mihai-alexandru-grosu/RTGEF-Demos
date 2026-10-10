using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Lecture03
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class ForestFoliage : MonoBehaviour
    {
        [Serializable]
        private class SpritePair
        {
            [SerializeField] private Sprite terrainSprite;
            [SerializeField] private TileBase foliageTile;
            [SerializeField] private Vector3Int offset;
            [Range(0f, 1f)]
            [SerializeField] private float chance = 1f;

            internal Sprite TerrainSprite => terrainSprite;
            internal TileBase FoliageTile => foliageTile;
            internal Vector3Int Offset => offset;
            internal float Chance => chance;
        }

        [Header("Separate terrain and decorative layers")]
        [SerializeField] private Tilemap terrain;
        [SerializeField] private TileBase forestTile;
        [SerializeField] private Tilemap foreground;

        [Header("Matching sprite pieces")]
        [SerializeField] private SpritePair[] pairs;

        [Header("Optional interior targets (empty means outside terrain)")]
        [SerializeField] private Sprite[] interiorSprites;

        // Runtime and editor refresh state.
        private bool refreshPending;
        private bool rebuilding;

        private void OnEnable()
        {
            Tilemap.tilemapTileChanged += OnTilesChanged;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += UpdateEditor;
#endif
            refreshPending = true;
        }

        private void OnDisable()
        {
            Tilemap.tilemapTileChanged -= OnTilesChanged;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= UpdateEditor;
#endif
        }

#if UNITY_EDITOR
        private void UpdateEditor()
        {
            if (!Application.isPlaying && refreshPending)
                RebuildFoliage();
        }
#endif

        private void OnValidate()
        {
            refreshPending = true;
        }

        private void OnTilesChanged(Tilemap changedMap, Tilemap.SyncTile[] changes)
        {
            if (changedMap == terrain && !rebuilding)
                refreshPending = true;
        }

        private void LateUpdate()
        {
            if (refreshPending)
                RebuildFoliage();
        }

        private bool CanDecorate(Vector3Int cell)
        {
            if (interiorSprites == null || interiorSprites.Length == 0)
                return !terrain.HasTile(cell);

            return terrain.GetTile(cell) == forestTile
                && Array.IndexOf(interiorSprites, terrain.GetSprite(cell)) >= 0;
        }
        // This map is generated; paint manual decorations on the other foreground map.
        [ContextMenu("Rebuild foreground foliage")]
        public void RebuildFoliage()
        {
            refreshPending = false;

            if (!terrain || !foreground || !forestTile || pairs == null || terrain == foreground)
                return;

            rebuilding = true;

            try
            {
                var desired = new Dictionary<Vector3Int, TileBase>();

                foreach (var cell in terrain.cellBounds.allPositionsWithin)
                {
                    if (terrain.GetTile(cell) != forestTile)
                        continue;

                    var sprite = terrain.GetSprite(cell);

                    for (int i = 0; i < pairs.Length; i++)
                    {
                        var pair = pairs[i];

                        if (pair.TerrainSprite != sprite || !pair.FoliageTile)
                            continue;

                        var target = cell + pair.Offset;

                        if (!CanDecorate(target) || desired.ContainsKey(target))
                            continue;

                        // Stable per-cell variation; rebuilding never consumes Unity's random state.
                        uint hash = unchecked((uint)(cell.x * 73856093 ^ cell.y * 19349663 ^ i * 83492791));
                        hash ^= hash >> 13;
                        hash = unchecked(hash * 1274126177u);
                        float sample = (hash & 65535u) / 65536f;

                        if (sample < pair.Chance)
                            desired.Add(target, pair.FoliageTile);
                    }
                }

                var positions = new List<Vector3Int>();
                var tiles = new List<TileBase>();

                foreach (var cell in foreground.cellBounds.allPositionsWithin)
                {
                    if (foreground.HasTile(cell) && !desired.ContainsKey(cell))
                    {
                        positions.Add(cell);
                        tiles.Add(null);
                    }
                }

                foreach (var entry in desired)
                {
                    if (foreground.GetTile(entry.Key) == entry.Value)
                        continue;

                    positions.Add(entry.Key);
                    tiles.Add(entry.Value);
                }

                if (positions.Count > 0)
                    foreground.SetTiles(positions.ToArray(), tiles.ToArray());
            }
            finally
            {
                rebuilding = false;
            }
        }
    }
}
