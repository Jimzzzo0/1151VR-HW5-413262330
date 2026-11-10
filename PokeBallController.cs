using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallController : MonoBehaviour
{
    private bool isHit = false;

    // 接受產生器傳來的方向與力道向量
    public void Shoot(Vector3 force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(force);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isHit) return;

        // 判定撞到的是否為寶可夢
        if (other.gameObject.CompareTag("Pokemon"))
        {
            isHit = true;

            // 1. 關閉物理運動，將球固定在碰撞位置
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // 2. 播放收服粒子特效
            ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            // 3. 寶可夢被收服消失
            other.gameObject.SetActive(false);

            // 4. 通知產生器結束遊戲 (不再生成新球)
            PokeBallGenerator generator = FindObjectOfType<PokeBallGenerator>();
            if (generator != null)
            {
                generator.GameOver();
            }

            Debug.Log("成功收服寶可夢，遊戲結束！");
        }
    }
}