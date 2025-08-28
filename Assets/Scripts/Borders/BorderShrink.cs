using UnityEngine;

public class BorderShrink : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed = 0.1f;
    [SerializeField] private float minimumSize = 0.1f;
    
    private void Update()
    {
        if (transform.localScale.x > minimumSize)
        {
            float newScale = transform.localScale.x - shrinkSpeed * Time.deltaTime;
            newScale = Mathf.Max(newScale, minimumSize);
            transform.localScale = new Vector3(newScale, newScale, 1f);
        }
    }
}
