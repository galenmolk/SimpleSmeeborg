using UnityEngine;
using UnityEngine.UI;

namespace SimpleSmeeborg
{
    [RequireComponent(typeof(Image))]
    public class CellBehaviour : MonoBehaviour
    {
        private readonly int northUniform = Shader.PropertyToID("_North");
        private readonly int southUniform = Shader.PropertyToID("_South");
        private readonly int eastUniform = Shader.PropertyToID("_East");
        private readonly int westUniform = Shader.PropertyToID("_West");

        [SerializeField] private GameObject startIcon;
        [SerializeField] private GameObject finishIcon;

        private Cell Cell { get; set; }

        private Image image;

        public void InitializeCell(Cell cell)
        {
            Cell = cell;
            SetWallVisuals();
            ToggleIcons();
            gameObject.name = $"Cell ({cell.X},{cell.Y})";
        }

        private void SetWallVisuals()
        {
            Material material = new Material(image.material);
            material.SetInt(northUniform, Cell.North);
            material.SetInt(southUniform, Cell.South);
            material.SetInt(eastUniform, Cell.East);
            material.SetInt(westUniform, Cell.West);
            image.material = material;
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
            image = GetComponent<Image>();
        }
    }
}
