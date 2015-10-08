using UnityEngine;
using System.Collections;

public class SpiderMove : MonoBehaviour {

	public GameObject SpiderModel;
	public GameObject CheckFront;
	public GameObject CheckBackBelow;
	public GameObject CheckBelow;
	public GameObject CheckFrontBelow;
	public float MaxSpeed = 3.0f;
	public float MinSpeed = 0.1f;

	public float MaxSize = 4.0f;
	public float MinSize = 0.5f;

	public float tempMaxSpeed = 3.0f;
	public float tempMinSpeed = 0.1f;
	public float tempMaxSize = 4.0f;
	public float tempMinSize = 0.5f;

	public bool CanProduceSilk = false;
	public GameObject SilkStringModel;
	//A low number will make the spider change his speed and direction more ofter (don't go under 4)
	public int SpiderChoice = 100;

	private bool _touchsomething;
	private float _speed = 6.0f;
	private float _size = 1.0f;
	private RaycastHit _hit;
	private bool _silkCreated = false;
	private Vector3 _silkStringStartPosition;
	private GameObject _slikString;
	private float _oldPositionY;
	private int _timeOut;
	private Quaternion _lastRotation;


	void Start(){
		//Give the spider a random size for viariation
		_size = Random.Range(MinSize,MaxSize);
		transform.localScale = new Vector3(_size,_size,_size);
		//random start speed
		_speed = Random.Range(MinSpeed,MaxSpeed);
		if(SpiderModel.GetComponent<Animation>()["Walk"] == null){
			Debug.Log("There's no animation attached to the spider model that's called 'Walk'");
		}else{
			SpiderModel.GetComponent<Animation>().Play("Walk");
			SpiderModel.GetComponent<Animation>()["Walk"].speed = _speed/(_size/2);
		}

		if(SpiderModel.GetComponent<Animation>()["Walk"] == null && CanProduceSilk){
			Debug.Log("There's no animation attached to the spider model that's called 'Descent'");
		}
	}

	void Update() {
		CheckMovement();
		Brain();
    }

	private void Brain(){
		//Spider changes his mind
		int Mode = Random.Range(1,SpiderChoice);
		//new random speed
		if (Mode == 1){
			_speed = Random.Range(MinSpeed,MaxSpeed);
			_speed = _speed * (_size/2);

		}

		if(_silkCreated == false){
			//turn Left
			if (Mode == 2){
				float _turnAmount = Random.Range(-20,-1);
				transform.Rotate(0, _turnAmount, 0, Space.Self);
			}
			//turn Left
			if (Mode == 3){
				float _turnAmount = Random.Range(1,20);
			transform.Rotate(0, _turnAmount, 0, Space.Self);
			}
			//create silk string
			if (CanProduceSilk == true && Mode == 4){
				//measure if the spider is upside down
				var dist = Vector3.Distance(new Vector3(0,1,0), CheckBelow.transform.forward*(1));
				if(dist <= 0.2f){
					CreateSilk();
				}
			}
		}
	}

	private void CheckMovement(){
		//check if the spider made a silk string
		if(CanProduceSilk && _silkCreated){
			float dist = Vector3.Distance(_silkStringStartPosition, this.transform.position);
			_slikString.transform.localScale = new Vector3(0.02F, dist*0.5f, 0.02F);
			_slikString.transform.position = new Vector3 ((this.transform.position.x+_silkStringStartPosition.x)/2,(this.transform.position.y+_silkStringStartPosition.y)/2,(this.transform.position.z+_silkStringStartPosition.z)/2);

			//force spider to go straight down
			transform.position = new Vector3(_slikString.transform.position.x,transform.position.y,_slikString.transform.position.z);
			transform.rotation = _lastRotation;
			transform.localPosition += new Vector3(0,-_speed* Time.deltaTime,0);

			//timeout if the spider can't land for some reason
			if (transform.position.y > _oldPositionY -0.1f && transform.position.y < _oldPositionY +0.1f){
				_timeOut += 1;
				if(_timeOut > 100){
					Destroy(_slikString);
					_silkCreated = false;
					_touchsomething = true;
					SpiderModel.GetComponent<Animation>().Play("Walk");
				}
			}else{
				_timeOut = 0;
				_oldPositionY = transform.position.y;
			}
			//Spider has landed. remove silk string and start walking again
			if (Physics.Raycast(CheckBelow.transform.position, CheckBelow.transform.forward,_size*0.4f)){
				Destroy(_slikString);
				_silkCreated = false;
				_touchsomething = true;
				SpiderModel.GetComponent<Animation>().Play("Walk");
			}

			if(SpiderModel.GetComponent<Animation>()["Descent"] != null){
				SpiderModel.GetComponent<Animation>()["Descent"].speed = _speed/(_size/2);
			}

		}else{
			_touchsomething = false;
	       //A wall in front of the Spider?
			if (Physics.Raycast(CheckFront.transform.position, CheckFront.transform.forward,out _hit,_size*0.3f)){
				if(_hit.transform.tag != "Spider"){
					//rotate spider up
					transform.Rotate(new Vector3(-15.0f,0,0));
				}
				_touchsomething = true;
			}
			//A ledge in front of the Spider?
			if (!Physics.Raycast(CheckFrontBelow.transform.position, CheckFrontBelow.transform.forward,out _hit,_size*0.3f)){
				//rotate spider down
				transform.Rotate(new Vector3(8.0f,0,0));
			}else{
				//move forward
				transform.position += transform.forward * (_speed/2.0f)* Time.deltaTime;
				_touchsomething = true;
			}
			//Back touching a surface?
			if (Physics.Raycast(CheckBackBelow.transform.position, CheckBackBelow.transform.forward,_size*0.25f)){
				//move forward
				transform.position += transform.forward * (_speed/5.0f)* Time.deltaTime;
				_touchsomething = true;
			}
			//touching a surface?
			if (Physics.Raycast(CheckBelow.transform.position, CheckBelow.transform.forward,_size*0.4f)){
				//move forward
				transform.position += transform.forward * _speed * Time.deltaTime;
				_touchsomething = true;
			}
			if(_touchsomething == false){
					this.GetComponent<Rigidbody>().useGravity = true;
			}else{
					this.GetComponent<Rigidbody>().useGravity = false;
			}
			//animation speed adjusted to current walk speed
			if(SpiderModel.GetComponent<Animation>()["Walk"] != null){
				SpiderModel.GetComponent<Animation>()["Walk"].speed = _speed/(_size/2);
			}
		}
	}

	private void CreateSilk(){
		if(SilkStringModel != null){
			_silkStringStartPosition = this.transform.position;
			_slikString = Instantiate(SilkStringModel, _silkStringStartPosition, Quaternion.identity) as GameObject;
			_slikString.transform.name = "SilkString";
			_slikString.transform.position = new Vector3 ((this.transform.position.x+_silkStringStartPosition.x)/2,(this.transform.position.y+_silkStringStartPosition.y)/2,(this.transform.position.z+_silkStringStartPosition.z)/2);
			_slikString.transform.localScale = new Vector3(0.02F, 0.01f, 0.02F);
			_silkCreated = true;
			_touchsomething = false;
			SpiderModel.GetComponent<Animation>().Play("Descent");
			transform.Rotate(180, 0, 0);
			_lastRotation = transform.rotation;
		}else{
			Debug.LogWarning("There is no SilkString model assigned in the SpiderMove script!");
		}
	}


	void FixedUpdate() {
		if(_touchsomething == true){
			//push the spider down
       	 	GetComponent<Rigidbody>().AddRelativeForce(0, -2f, 0);
		}
	}
	void OnDrawGizmos() {
        DrawLinesAndIcons();
    }
	private void DrawLinesAndIcons() {
		Gizmos.DrawLine(CheckFront.transform.position, CheckFront.transform.position+(CheckFront.transform.forward*_size*0.3f));
		Gizmos.DrawLine(CheckFrontBelow.transform.position, CheckFrontBelow.transform.position+(CheckFrontBelow.transform.forward*_size*0.3f));
		Gizmos.DrawLine(CheckBackBelow.transform.position, CheckBackBelow.transform.position+(CheckBackBelow.transform.forward*_size*0.25f));
		Gizmos.DrawLine(CheckBelow.transform.position, CheckBelow.transform.position+(CheckBelow.transform.forward*_size*0.4f));

		Gizmos.DrawLine(this.transform.position, this.transform.position + new Vector3(0,_size*0.3f,0));
	}

}
