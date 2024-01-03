using UnityEngine;

public class _10GenerateMap : MonoBehaviour
{
    public Transform _parentTrans;

    private string _path1 = "ResTest/Cyan";
    private string _path2 = "ResTest/Green";

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject gameObject = null;
                if ((i + j) % 2 == 0)
                {
                    gameObject = ResourceService.Instance.InstantiatePrefab(_parentTrans, _path1, true);
                }
                else
                {
                    gameObject = ResourceService.Instance.InstantiatePrefab(_parentTrans, _path2, true);
                }
                gameObject.transform.position = new Vector3(i * 10, 0, j * 10);
            }
        }
    }
}
