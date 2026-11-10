using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallGenerator : MonoBehaviour
{
    public GameObject pokeBallPrefab; // 精靈球 Prefab
    public Transform spawnPoint;      // 發射起始點

    private Vector3 dragStartPos;
    private bool isGameOver = false;

    // 力道倍率 (可在 Inspector 自行微調)
    public float forceFactor = 8f;

    void Update()
    {
        // 遊戲結束則不再接受發射
        if (isGameOver) return;

        // 1. 滑鼠左鍵按下：記錄拖曳起點
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
        }

        // 2. 滑鼠左鍵放開：計算拖曳向量並拋出
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 dragEndPos = Input.mousePosition;
            Vector3 dragVector = dragEndPos - dragStartPos;

            // 只有向上滑動才發射 (避免誤觸原地掉落)
            if (dragVector.y > 10f)
            {
                // 生成精靈球
                Vector3 startPos = spawnPoint != null ? spawnPoint.position : new Vector3(-2f, 1.3f, -6f);
                GameObject ball = Instantiate(pokeBallPrefab, startPos, Quaternion.identity);

                // 計算方向與力道：
                // X 軸：左右偏移力道
                // Y 軸：向上拋起弧度
                // Z 軸：往前飛行的推力 (依向上拖曳長度加乘)
                float forwardForce = dragVector.y * forceFactor;
                float upForce = Mathf.Clamp(dragVector.y * 0.6f * forceFactor, 200f, 600f);
                float rightForce = dragVector.x * (forceFactor * 0.8f);

                Vector3 throwForce = new Vector3(rightForce, upForce, forwardForce);

                // 給予推力
                ball.GetComponent<PokeBallController>().Shoot(throwForce);
            }
        }
    }

    // 遊戲結束方法
    public void GameOver()
    {
        isGameOver = true;
    }
}