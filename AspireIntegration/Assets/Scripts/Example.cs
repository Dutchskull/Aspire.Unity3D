using UnityEngine;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(ConfigProvider.Configuration.ToString());
    }
}
