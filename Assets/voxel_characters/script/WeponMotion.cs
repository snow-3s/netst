using UnityEngine;
using System.Collections;

//
// 装備武器によって、AnimatorControllerが替わる
// 
public class WeponMotion : MonoBehaviour {

	public GameObject Wepon;
	public Vector3 WeponScale = new Vector3(0.17f,0.17f,0.17f);
	public GameObject Shield;
	public GameObject RightHandPosition;
	public GameObject LeftHandPosition;
	public GameObject ShieldPosition;

	[HideInInspector]
	public WeponStatus ws; // WeponStatus 
	/*							L WeponType (int)
	 * 								0:none 
	 * 								1:one hand wepon 
	 * 								2:two hand wepon 
	 * 								3:dual wepon 
	 * 								4:pole wepon
	 * 								5:bow
	*/
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		//武器を持っていない場合はデフォルトのアニメーション
		if(Wepon != null){
			animator.runtimeAnimatorController = Wepon.GetComponent<Animator> ().runtimeAnimatorController;
		}
		else return;

		ws = Wepon.GetComponent<WeponStatus> ();
		GameObject w_obj;
		if (Wepon != null) {
			if (ws != null) {
				if (ws.WeponType == 5) { //弓なら左手に持つ
					w_obj = (GameObject)Instantiate (Wepon, LeftHandPosition.transform.position, Quaternion.Euler (0, 0, 0));
					w_obj.transform.parent = LeftHandPosition.transform;
				} else {
					w_obj = (GameObject)Instantiate (Wepon, RightHandPosition.transform.position, Quaternion.Euler (0, 0, 0));
					w_obj.transform.parent = RightHandPosition.transform;
				}
				w_obj.transform.localScale = WeponScale;
			}
		}
			
		if(ws != null) {
			switch (ws.WeponType) {
			case 0 : //none

			case 1: //one hand wepon
				if (Shield != null) { //盾を持っている場合、装着する
					w_obj = (GameObject)Instantiate (Shield, ShieldPosition.transform.position, ShieldPosition.transform.rotation);
					w_obj.transform.parent = ShieldPosition.transform;
					w_obj.transform.localScale = WeponScale;
					w_obj.transform.localPosition = new Vector3 (0.4f, -0.5f, 0.0f);
				}
				break;

			case 2 : // two hand wepon
				break;

			case 3 : // dual wepon
				w_obj = (GameObject)Instantiate (Wepon, LeftHandPosition.transform.position, Quaternion.Euler(0,0,0));
				w_obj.transform.parent = LeftHandPosition.transform;
				w_obj.transform.localScale = WeponScale;
				break;

			case 4 : // pole wepon
				break;	
			case 5 : // bow
				break;	
			}
		}
	}
}
