using UnityEngine;
using UnityEngine.InputSystem;

public class MiniCharacter : MonoBehaviour
{
	#region 変数宣言

	//! キャラの移動、回転用変数
	private Vector3 moveInputVelocity = Vector3.zero;
	private Vector3 lookInputVelocity = Vector3.zero;
	public float moveSpeed = 1f;
	public Vector3 angle = Vector3.zero;
	public GameObject CameraJoint;
	public float maxAngle = 45f;
	public float minAngle = -90f;

	//! キャラの攻撃用変数
	public GameObject blastPos;		//銃のルート
	public GameObject blastObj;		//銃のオブジェクト
	public GameObject shotPoint;		//キャラの弾の発射位置
	public GameObject bulletPrefab;	//キャラの弾のプレハブ

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
		OnMove(); //  キャラの移動処理
		Rotation(); // キャラの回転処理
		OnAttack(); // キャラの攻撃処理
	}
	#endregion 更新処理

	#region 入力値の取得

	//! === キャラの移動用の入力値を取得する関数（WASD） ===
	void OnMove(InputValue value)
	{
		//Debug.Log($"move value is {value.Get<Vector2>()}");
		moveInputVelocity = value.Get<Vector2>();
	}

	//! === キャラの視点移動用の入力値を取得する関数（マウス座標） ===
	void OnLook(InputValue value)
	{
		//Debug.Log($"look value is {value.Get<Vector2>()}");
		lookInputVelocity = value.Get<Vector2>();
	}

	//! === キャラの攻撃用の入力値を取得する関数（スペースキー） ===
	void OnAttack(InputValue value)
	{
		Debug.Log($"attack value is {value.Get<float>()}");
		attackInputValue = value.Get<float>();
	}

	#endregion 入力値の取得

	#region 移動処理
	private void OnMove()
	{
		Vector3 velocity = Vector3.zero;
		velocity.x = moveInputVelocity.x;
		velocity.z = moveInputVelocity.y;

		transform.Translate(velocity * Time.deltaTime * moveSpeed);
	}
	#endregion 移動処理

	#region 回転処理
	private void Rotation()
	{
		angle.x -= lookInputVelocity.y;
		angle.y += lookInputVelocity.x;

		angle.x = Mathf.Clamp(angle.x, minAngle, maxAngle);

		transform.eulerAngles = new Vector3(0, angle.y, 0);
		CameraJoint.transform.eulerAngles = new Vector3(angle.x, CameraJoint.transform.eulerAngles.y, CameraJoint.transform.eulerAngles.z);
	}
	#endregion 回転処理

	#region 攻撃処理
	private void OnAttack()
	{
		if (attackInputValue > 0f)
		{
			GameObject bullet = Instantiate(bulletPrefab, shotPoint.transform.position, shotPoint.transform.rotation);
			Rigidbody rb = bullet.GetComponent<Rigidbody>();
			rb.AddForce(-shotPoint.transform.up * bulletSpeed, ForceMode.Impulse);

			attackInputValue = 0f;
		}
	}
	#endregion 攻撃処理
}
