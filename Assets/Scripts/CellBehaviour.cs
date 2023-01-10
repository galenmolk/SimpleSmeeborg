using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public TMP_Text gCost;
        public TMP_Text hCost;
        public TMP_Text fCost;
        public TMP_Text openClosedText;

        public Cell Cell;

        public Vector2 Position => thisTransform.position;

        private SpriteRenderer spriteRenderer;
        private Transform thisTransform;

        public void InitializeCell(Cell cell)
        {
            Cell = cell;
            SetWallVisuals();
            ToggleIcons();
            gameObject.name = $"Cell ({cell.X},{cell.Y})";
        }

        private void SetWallVisuals()
        {
            Material material = new Material(spriteRenderer.material);
            material.SetInt(northUniform, Cell.HasNorthPassage.ToInt());
            material.SetInt(southUniform, Cell.HasSouthPassage.ToInt());
            material.SetInt(eastUniform, Cell.HasEastPassage.ToInt());
            material.SetInt(westUniform, Cell.HasWestPassage.ToInt());
            spriteRenderer.material = material;
        }

        private void ToggleIcons()
        {
            switch (Cell.CellType)
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
