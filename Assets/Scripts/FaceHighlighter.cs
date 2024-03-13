using UnityEngine;

public class FaceHighlighter : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Start()
    {
        // Assuming the BoxCollider is attached to the same GameObject as this script
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        // Cast a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (boxCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            string direction = CalculateDirection(hit);
            // Debug.Log("Hovering over face: " + direction);

            // grab child object called "highlight-${direction}"
            GameObject highlight = transform.Find("highlight-" + direction).gameObject;

            // set opacity to 1
            Color color = highlight.GetComponent<Renderer>().material.color;
            color.a = 1;
            highlight.GetComponent<Renderer>().material.color = color;

            // set opacity to 0 for all other objects with name "highlight-"
            foreach (Transform child in transform)
            {
                if (child.name.StartsWith("highlight-") && child.name != "highlight-" + direction)
                {
                    Color otherColor = child.GetComponent<Renderer>().material.color;
                    otherColor.a = 0;
                    child.GetComponent<Renderer>().material.color = otherColor;
                }
            }

            // get coordinates of current object
            Vector3 position = transform.position;

            // add 1 to the x, y, or z coordinate of the current object based on the direction
            switch (direction)
            {
                case "x+":
                    position.x += 1;
                    break;
                case "x-":
                    position.x -= 1;
                    break;
                case "y+":
                    position.y += 1;
                    break;
                case "y-":
                    position.y -= 1;
                    break;
                case "z+":
                    position.z += 1;
                    break;
                case "z-":
                    position.z -= 1;
                    break;
            }

            // create new instance of the "BlockTemplate" prefab at the new position
            if (Input.GetMouseButtonDown(0))
            {
                // eventually we'll have a lot of block types, this'll be better than having a lot of class variables
                // https://docs.unity3d.com/ScriptReference/Resources.html
                GameObject block = Instantiate(Resources.Load("LevelBlocks/BlockTemplate"), position, Quaternion.identity) as GameObject;
            }
        } else {
            // mouse not over any face, set opacity to 0 for all objects with name "highlight-"
            foreach (Transform child in transform)
            {
                if (child.name.StartsWith("highlight-"))
                {
                    Color otherColor = child.GetComponent<Renderer>().material.color;
                    otherColor.a = 0;
                    child.GetComponent<Renderer>().material.color = otherColor;
                }
            }
        }
    }

    string CalculateDirection(RaycastHit hit)
    {
        // Calculate hit point in local space of the box collider
        Vector3 hitLocalPoint = transform.InverseTransformPoint(hit.point);

        // Calculate the absolute value of hitLocalPoint to avoid negative values
        Vector3 absHitLocalPoint = new Vector3(Mathf.Abs(hitLocalPoint.x), Mathf.Abs(hitLocalPoint.y), Mathf.Abs(hitLocalPoint.z));

        // Determine which face is being hovered over
        if (absHitLocalPoint.x > absHitLocalPoint.y && absHitLocalPoint.x > absHitLocalPoint.z)
        {
            if (hitLocalPoint.x > 0)
                return "x+";
            else
                return "x-";
        }
        else if (absHitLocalPoint.y > absHitLocalPoint.x && absHitLocalPoint.y > absHitLocalPoint.z)
        {
            if (hitLocalPoint.y > 0)
                return "y+";
            else
                return "y-";
        }
        else
        {
            if (hitLocalPoint.z > 0)
                return "z+";
            else
                return "z-";
        }
    }
}