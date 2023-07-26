using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [SerializeField] GameObject[] buildObjects; // array of objects to build
    private GameObject selectedObject; // the currently selected object
    private GameObject previewObject; // the preview object
    private bool isPreviewing; // whether or not the preview object is active
    private Ray ray; // the ray to cast from the camera to the mouse position
    private RaycastHit hit; // the object that was hit by the ray

    private void Update()
    {
        HandlePreview();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }

    // Handle the previewing of the selected object
    private void HandlePreview()
    {
        if (isPreviewing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                DestroyPreviewObject();
                isPreviewing = false;
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
                {
                    MovePreviewObject(hit.point);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RotatePreviewObject();
                }
            }
        }
    }

    // Move the preview object to the target position
    private void MovePreviewObject(Vector3 targetPosition)
    {
        previewObject.transform.position = targetPosition;
    }

    // Rotate the preview object by 90 degrees
    private void RotatePreviewObject()
    {
        previewObject.transform.Rotate(Vector3.up, 90f);
    }

    // Place the selected object in the scene at the target position
    private void PlaceObject()
    {
        if (previewObject != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                InstantiateSelectedObject(hit.point);
            }

            previewObject = null;
            isPreviewing = false;
        }
    }

    // Instantiate the selected object at the given position
    private void InstantiateSelectedObject(Vector3 position)
    {
        Instantiate(selectedObject, position, Quaternion.identity);
    }

    // Select an object from the build menu
    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {
            selectedObject = buildObjects[index];
            DestroyPreviewObject();
            CreatePreviewObject();
            isPreviewing = true;
        }
        EventSystem.current.SetSelectedGameObject(null); // cancel keyboard (pressing space etc.)
    }

    // Destroy the preview object if it exists
    private void DestroyPreviewObject()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    // Create a preview object for the selected object
    private void CreatePreviewObject()
    {
        previewObject = Instantiate(selectedObject);
    }
}
