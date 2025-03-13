using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public List<GameObject> characters;
    public Transform mapContainer;
    public float cameraFollowSpeed = 5f;
    public float specialInteractionDistance = 2f;
    public Dictionary<string, int> resources = new Dictionary<string, int>();
    public Dictionary<string, GameObject> facilities = new Dictionary<string, GameObject>();

    private GameObject currentCharacter;
    private Vector3 targetPosition;
    private bool isSpecialInteraction = false;

    void Start()
    {
        // 初始化角色选择
        SelectCharacter(characters[0]);

        // 初始化资源
        resources.Add("Wood", 0);
        resources.Add("Stone", 0);

        // 初始化设施
        facilities.Add("Campfire", null);
        facilities.Add("Workshop", null);

        // 生成地图
        GenerateMap();
    }

    void Update()
    {
        // 摄像机跟随
        if (!isSpecialInteraction)
        {
            targetPosition = player.transform.position + new Vector3(0, 10, -10);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraFollowSpeed);
        }

        // 特殊交互视角变化
        if (Vector3.Distance(player.transform.position, player.transform.forward) < specialInteractionDistance)
        {
            isSpecialInteraction = true;
            mainCamera.transform.position = player.transform.position + new Vector3(0, 5, 0);
            mainCamera.transform.LookAt(player.transform.position + new Vector3(0, 1, 0));
        }
        else
        {
            isSpecialInteraction = false;
        }

        // 移动奖励和周期性事件
        MoveRewardAndEvents();

        // 资源收集
        CollectResources();

        // 建造和升级设施
        BuildAndUpgradeFacilities();
    }

    void SelectCharacter(GameObject character)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        currentCharacter = Instantiate(character, new Vector3(0, 0, 0), Quaternion.identity);
        player = currentCharacter;
    }

    void GenerateMap()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.transform.position = new Vector3(x * 2, 0, z * 2);
                tile.transform.parent = mapContainer;
            }
        }
    }

    void MoveRewardAndEvents()
    {
        // 示例：每移动10单位获得奖励
        if (player.transform.position.magnitude > 10)
        {
            Debug.Log("Move Reward: +10 Wood");
            resources["Wood"] += 10;
        }

        // 示例：每分钟发生一次随机事件
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log("Random Event Occurred!");
        }
    }

    void CollectResources()
    {
        // 示例：收集附近的资源
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, 2f);
        foreach (var hit in hitColliders)
        {
            if (hit.gameObject.CompareTag("Resource"))
            {
                string resourceName = hit.gameObject.name;
                if (resources.ContainsKey(resourceName))
                {
                    resources[resourceName] += 1;
                    Destroy(hit.gameObject);
                }
            }
        }
    }

    void BuildAndUpgradeFacilities()
    {
        // 示例：建造和升级设施
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (resources["Wood"] >= 10 && facilities["Campfire"] == null)
            {
                GameObject campfire = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                campfire.transform.position = player.transform.position + new Vector3(0, 1, 0);
                campfire.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                campfire.GetComponent<Renderer>().material.color = Color.red;
                facilities["Campfire"] = campfire;
                resources["Wood"] -= 10;
                Debug.Log("Campfire Built!");
            }
        }
    }
}
