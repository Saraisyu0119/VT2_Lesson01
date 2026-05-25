using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
	#region 変数宣言

	//! タンクの移動、回転用変数
	public GameObject topAxis;               //タンク上部（砲塔）のオブジェクト
	public GameObject cannonAxis;           //タンクの砲のオブジェクト
	private Vector3 moveInputVelocity = Vector3.zero;
    private Vector3 lookInputVelocity = Vector3.zero;
    public float moveSpeed = 1f;

	//! タンクの攻撃用変数
	public GameObject bulletPrefab;             //タンクの弾のプレハブ
	public GameObject shotPoint;                //タンクの弾の発射位置
	private float attackInputValue = 0f;
	public float bulletSpeed = 500f;

	#endregion 変数宣言

	#region 初期化処理
	void Start()
    {
        
    }
	#endregion 初期化処理

	#region 更新処理
	void Update()
    {
		//! === タンクの移動処理 ===
		Vector3 move = Vector3.zero;
		move.z = moveInputVelocity.y;

		Vector3 bodyTorque = Vector3.zero;
		bodyTorque.y = moveInputVelocity.x;

		transform.Translate(move * Time.deltaTime * moveSpeed);

		//! === タンク上部の回転処理 ===
		Vector3 topTorque = Vector3.zero;
		topTorque.y = lookInputVelocity.x;

		transform.Rotate(bodyTorque * Time.deltaTime * 90);
		topAxis.transform.Rotate(topTorque * Time.deltaTime * 90);

		//! === タンク砲の回転処理 ===
		Vector3 cannonTorque = Vector3.zero;
		cannonTorque.x = lookInputVelocity.y;
		cannonAxis.transform.Rotate(-cannonTorque * Time.deltaTime * 90);

		//! === タンクの攻撃処理 ===
		if(attackInputValue > 0f)
		{
			GameObject bullet = Instantiate(bulletPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
			Rigidbody rb = bullet.GetComponent<Rigidbody>();
			rb.AddForce(-shotPoint.transform.up * bulletSpeed, ForceMode.Impulse);

			attackInputValue = 0f;
		}
	}
	#endregion 更新処理

	#region 入力値の取得

	//! === タンクの移動、回転用の入力値を取得する関数（WASD） ===
	void OnMove(InputValue value)
    {
        Debug.Log($"move value is {value.Get<Vector2>()}");
        moveInputVelocity = value.Get<Vector2>();
    }

	//! === タンクの上部回転用の入力値を取得する関数（マウス座標） ===
	void OnLook(InputValue value)
    {
        Debug.Log($"look value is {value.Get<Vector2>()}");
        lookInputVelocity = value.Get<Vector2>();
	}

	//! === タンクの攻撃用の入力値を取得する関数（スペースキー） ===
	void OnAttack(InputValue value)
	{
		Debug.Log($"attack value is {value.Get<float>()}");
		attackInputValue = value.Get<float>();
	}

	#endregion 入力値の取得
}
