using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public static class Utility
{
    public static TextMeshPro CreateWorldText3D(string value, float fontSize, Vector3 position, Quaternion rotation)
    {
        //create gameobject
        GameObject textObject = new GameObject("DebugDisplay");
        //DebugDisplay debugDisplay = textObject.AddComponent<DebugDisplay>();
        TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();
        RectTransform rectTransform = textObject.GetComponent<RectTransform>();

        //modify text settings
        textObject.transform.position = position;

        rectTransform.sizeDelta = new Vector2(1f, 0.5f);
        rectTransform.rotation = rotation;

        textMesh.fontSize = fontSize;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.alignment = TextAlignmentOptions.Midline;

        //update string
        textMesh.text = value;

        return textMesh;
    }

    public static Vector3 GetMouseHitPoint()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        mousePos.z = 10f;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
            return Vector3.zero;
    }
    public static Vector3 GetMouseHitPoint(float modifiedYValue)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        mousePos.z = 10f;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x, modifiedYValue, hit.point.z);
        }
        else
            return Vector3.zero;
    }
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        return worldPos;
    }
    public static string DisplayTimeMinutes(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //float milliseconds = timeToDisplay % 1 * 10; ;

        return string.Format("{0:00}:{1:00}", minutes, seconds); //:{2:00}
    }
    public static string DisplayTimeSeconds(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        return string.Format("{0:00}", seconds);
    }
}
