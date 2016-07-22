using UnityEngine;
using System.Collections;

public class mainControl : MonoBehaviour {

    public Transform GirlTransform; //角色
    public Transform MonsterTransform; //怪物
    public Transform BackTransform1; //背景
    public Transform BackTransform2;
    public Transform FloorTransform1; //地板
    public Transform FloorTransform2;
    public int Speed = 3; //角色奔跑速度

    private Animation GirlAnimation; //角色动画
    private int state = 1; //角色状态
    private int frame = 0; //帧数

    void Start () {
        GirlAnimation = GameObject.Find("girlPrefab").GetComponent<Animation>();
    }

	void Update () {
        if (state == 1)
        {
            BackTransform1.Translate(new Vector3(0, 0, 1) * Speed * Time.deltaTime);
            BackTransform2.Translate(new Vector3(0, 0, 1) * Speed * Time.deltaTime);
            FloorTransform1.Translate(new Vector3(0, 0, 1) * Speed * Time.deltaTime);
            FloorTransform2.Translate(new Vector3(0, 0, 1) * Speed * Time.deltaTime);
            MonsterTransform.Translate(new Vector3(0, 0, 1) * Speed * Time.deltaTime);
            if (BackTransform1.position.z > 10)
            {
                BackTransform1.Translate(new Vector3(0, 0, -20));
            }
            if (BackTransform2.position.z > 10)
            {
                BackTransform2.Translate(new Vector3(0, 0, -20));
            }
            if (FloorTransform1.position.z > 10)
            {
                FloorTransform1.Translate(new Vector3(0, 0, -20));
            }
            if (FloorTransform2.position.z > 10)
            {
                FloorTransform2.Translate(new Vector3(0, 0, -20));
            }
            if (Vector3.Distance(GirlTransform.position, MonsterTransform.position) <= 1)
            {
                frame = 0;
                state = 2;
                GirlAnimation.CrossFade("Attack1", 0.01f, PlayMode.StopAll);
            }
        }
        else if(state == 2)
        {
            frame++;
            if (frame >= 260)
            {
                frame = 0;
                MonsterTransform.Translate(new Vector3(0, 0, -10));
                GirlAnimation.CrossFade("Run1", 0.01f, PlayMode.StopAll);
                state = 1;
            }
        }
    }
}