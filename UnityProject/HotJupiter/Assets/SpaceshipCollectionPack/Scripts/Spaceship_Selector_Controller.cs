using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spaceship_Selector_Controller : MonoBehaviour
{
	//Animators
	public Animator camera_Animator, spaceship_text_Animator;

	public Animator arcadia_animator;
	public Animator transporter_animator;
	public Animator fighter_animator;
	public Animator hyperspace_animator;
	public Animator scout_animator;
	public Animator personal_animator;
	public Animator drone_animator;
	public Animator wrecker_animator;
	//Text
	public TextMeshProUGUI spaceship_Text;
	//Audio
	public AudioSource button_b_Sound;
	public AudioSource button_x_p_Sound;
	public AudioSource button_x_n_Sound;
	//Local Variables
	int camera_position = 0;
	bool x_axis_lock = false;
	bool y_axis_lock = false;

	int arcadia_position;
	int transporter_position;
	int fighter_position;
	int hyperspace_position;
	int scout_position;
	int personal_position;
	int drone_position;
	int wrecker_position;
    // Start is called before the first frame update
    void Start()
    {
		
    }

	public void Move_On_Positive_X_Axis(){

		if(!x_axis_lock){
			button_x_p_Sound.Play();
			switch(camera_position){

			case 0:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "TRANSPORTER";
				x_axis_lock = true;
				break;
			case 1:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement_1");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "FIGHTER";
				x_axis_lock = true;
				break;
			case 2:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement_2");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "HYPERSPACE";
				x_axis_lock = true;
				break;
			case 3:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement_3");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "SCOUT";
				x_axis_lock = true;
				break;
			case 4:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement_4");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "PERSONAL";
				x_axis_lock = true;
				break;
			case 5:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement_5");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "DRONE";
				x_axis_lock = true;
				break;
			case 6:
				camera_position++;
				camera_Animator.Play("MainCamera_X_Movement_6");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "WRECKER";
				x_axis_lock = true;
				break;

			}
		}

	}

	public void Move_On_Negative_X_Axis(){

		if(!x_axis_lock){
			button_x_n_Sound.Play();
			switch(camera_position){
		
			case 1:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_1");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "ARCADIA";
				x_axis_lock = true;
				break;
			case 2:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_2");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "TRANSPORTER";
				x_axis_lock = true;
				break;
			case 3:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_3");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "FIGHTER";
				x_axis_lock = true;
				break;
			case 4:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_4");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "HYPERSPACE";
				x_axis_lock = true;
				break;
			case 5:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_5");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "SCOUT";
				x_axis_lock = true;
				break;
			case 6:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_6");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "PERSONAL";
				x_axis_lock = true;
				break;
			case 7:
				camera_position--;
				camera_Animator.Play("MainCamera_Negative_X_Movement_7");
				spaceship_text_Animator.Play("Change_Spaceship_Name", -1, 0f);
				spaceship_Text.text = "DRONE";
				x_axis_lock = true;
				break;

			}
		}

	}

	public void Change_Spaceship_Negative(){
		if(gameObject.transform.position.x == 0f){
			Move_Arcadia_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 20f){
			Move_Transporter_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 40f){
			Move_Fighter_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 60f){
			Move_Hyperspace_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 80f){
			Move_Scout_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 100f){
			Move_Personal_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 120f){
			Move_Drone_Negative_Y_Axis();
		}else if(gameObject.transform.position.x == 140f){
			Move_Wrecker_Negative_Y_Axis();
		}
	}

	public void Change_Spaceship_Positive(){
		if(gameObject.transform.position.x == 0f){
			Move_Arcadia_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 20f){
			Move_Transporter_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 40f){
			Move_Fighter_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 60f){
			Move_Hyperspace_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 80f){
			Move_Scout_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 100f){
			Move_Personal_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 120f){
			Move_Drone_Positive_Y_Axis();
		}else if(gameObject.transform.position.x == 140f){
			Move_Wrecker_Positive_Y_Axis();
		}
	}



	void Move_Arcadia_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(arcadia_position){

			case 0:
				arcadia_position++;
				arcadia_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				arcadia_position++;
				arcadia_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				arcadia_position++;
				arcadia_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Transporter_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(transporter_position){

			case 0:
				transporter_position++;
				transporter_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				transporter_position++;
				transporter_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				transporter_position++;
				transporter_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Fighter_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(fighter_position){

			case 0:
				fighter_position++;
				fighter_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				fighter_position++;
				fighter_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				fighter_position++;
				fighter_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Hyperspace_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(hyperspace_position){
		
			case 0:
				hyperspace_position++;
				hyperspace_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				hyperspace_position++;
				hyperspace_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				hyperspace_position++;
				hyperspace_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Scout_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(scout_position){

			case 0:
				scout_position++;
				scout_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				scout_position++;
				scout_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				scout_position++;
				scout_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Personal_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(personal_position){

			case 0:
				personal_position++;
				personal_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				personal_position++;
				personal_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				personal_position++;
				personal_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Drone_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(drone_position){

			case 0:
				drone_position++;
				drone_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				drone_position++;
				drone_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				drone_position++;
				drone_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Wrecker_Negative_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(wrecker_position){

			case 0:
				wrecker_position++;
				wrecker_animator.Play("Spaceships_N_Y_Mov_0");
				y_axis_lock = true;
				break;
			case 1:
				wrecker_position++;
				wrecker_animator.Play("Spaceships_N_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				wrecker_position++;
				wrecker_animator.Play("Spaceships_N_Y_Mov_2");
				y_axis_lock = true;
				break;

			}
		}

	}




	void Move_Arcadia_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(arcadia_position){

			case 1:
				arcadia_position--;
				arcadia_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				arcadia_position--;
				arcadia_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				arcadia_position--;
				arcadia_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Transporter_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(transporter_position){

			case 1:
				transporter_position--;
				transporter_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				transporter_position--;
				transporter_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				transporter_position--;
				transporter_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Fighter_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(fighter_position){

			case 1:
				fighter_position--;
				fighter_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				fighter_position--;
				fighter_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				fighter_position--;
				fighter_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Hyperspace_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(hyperspace_position){

			case 1:
				hyperspace_position--;
				hyperspace_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				hyperspace_position--;
				hyperspace_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				hyperspace_position--;
				hyperspace_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Scout_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(scout_position){

			case 1:
				scout_position--;
				scout_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				scout_position--;
				scout_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				scout_position--;
				scout_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Personal_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(personal_position){

			case 1:
				personal_position--;
				personal_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				personal_position--;
				personal_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				personal_position--;
				personal_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Drone_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(drone_position){

			case 1:
				drone_position--;
				drone_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				drone_position--;
				drone_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				drone_position--;
				drone_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}

	void Move_Wrecker_Positive_Y_Axis(){

		if(!y_axis_lock && !x_axis_lock){
			button_b_Sound.Play();
			switch(wrecker_position){

			case 1:
				wrecker_position--;
				wrecker_animator.Play("Spaceships_P_Y_Mov_1");
				y_axis_lock = true;
				break;
			case 2:
				wrecker_position--;
				wrecker_animator.Play("Spaceships_P_Y_Mov_2");
				y_axis_lock = true;
				break;
			case 3:
				wrecker_position--;
				wrecker_animator.Play("Spaceships_P_Y_Mov_3");
				y_axis_lock = true;
				break;

			}
		}

	}




	public void Reset_X_Axis_Lock(){
		x_axis_lock = false;
	}

	public void Reset_Y_Axis_Lock(){
		y_axis_lock = false;
	}

	void Update(){
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.75f);
	}

}
