using UnityEngine;

namespace SimpleSmeeborg
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellBehaviour : MonoBehaviour
    {
        private readonly int northUniform = Shader.PropertyToID("_North");
        private readonly int southUniform = Shader.PropertyToID("_South");
        private readonly int eastUniform = Shader.PropertyToID("_East");
        private readonly int westUniform = Shader.PropertyToID("_West");

        [SerializeField] private GameObject startIcon;
        [SerializeField] private GameObject finishIcon;

        private SpriteRenderer spriteRenderer;
        private Transform thisTransform;

        public void InitializeCell(Cell cell)
        {
            SetWallVisuals(cell);
            TryToggleIcons(cell.CellType);

            // Give meaningful names to the cells if running in the editor.
            #if UNITY_EDITOR
            gameObject.name = $"Cell ({cell.Coordinates})";
            #endif
        }

        private void SetWallVisuals(Cell cell)
        {
            Material material = new Material(spriteRenderer.material);
            material.SetInt(northUniform, cell.HasNorthPassage.ToInt());
            material.SetInt(southUniform, cell.HasSouthPassage.ToInt());
            material.SetInt(eastUniform, cell.HasEastPassage.ToInt());
            material.SetInt(westUniform, cell.HasWestPassage.ToInt());
            spriteRenderer.material = material;
        }

        private void TryToggleIcons(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.START:
                    startIcon.SetActive(true);
                    break;
                case CellType.FINISH:
                    finishIcon.SetActive(true);
                    break;
            }
        }

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            thisTransform = transform;
        }
    }
}
