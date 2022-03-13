using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class quadrocopterScript : MonoBehaviour {

	//фактические параметры
	private double pitch; //Тангаж
	private double roll; //Крен
	private double yaw; //Рыскание
	public double throttle; //Газ, газ мы задаем извне, поэтому он public

    //требуемые параметры
    public double targetPitch;
    public double targetRoll;
    public double targetYaw;

	public Vector3 targetPossition;


	public double targetHeight;
	public double targetSpeed;
	public double distansToTargetPosition;

	public int counterPoint = 0;

	GPSModul gps;
	public bool checkAutoHangup;
	public bool autoPilot;
	public bool stab;

	private Globalparametr Gp;


	public float fwdSpeed;
	public float driftSpeed;
	//PID регуляторы, которые будут стабилизировать углы
	//каждому углу свой регулятор, класс PID определен ниже
	//константы подобраны на глаз :) пробуйте свои значения
	private PID pitchPID = new PID (100, 0, 20);
	private PID rollPID = new PID (100, 0, 20);
	private PID yawPID = new PID (50, 0, 50);

	private PID SpeedPID = new PID(0.4, 0, 0);

	private int radiusZone = 2;
	private Quaternion prevRotation = new Quaternion (0, 1, 0, 0);

	void readRotation () {
		
		//фактическая ориентация нашего квадрокоптера,
		//в реальном квадрокоптере эти данные необходимо получать
		//из акселерометра-гироскопа-магнетометра, так же как делает это ваш
		//смартфон
		Vector3 rot = GameObject.Find ("Frame").GetComponent<Transform> ().rotation.eulerAngles;
		pitch = rot.x;
		yaw = rot.y;
		roll = rot.z;

	}

	//функция стабилизации квадрокоптера
	//с помощью PID регуляторов мы настраиваем
	//мощность наших моторов так, чтобы углы приняли нужные нам значения
	void stabilize () {

		fwdSpeed = Vector3.Dot(gps.rb.velocity, gps.Quadrocopter.transform.forward);
		driftSpeed = Vector3.Dot(gps.rb.velocity, gps.Quadrocopter.transform.right);

		//нам необходимо посчитать разность между требуемым углом и текущим
		//эта разность должна лежать в промежутке [-180, 180] чтобы обеспечить
		//правильную работу PID регуляторов, так как нет смысла поворачивать на 350
		//градусов, когда можно повернуть на -10

		double dPitch = targetPitch - pitch;
		double dRoll = targetRoll - roll;
		double dYaw = targetYaw - yaw;

		dPitch -= Math.Ceiling (Math.Floor (dPitch / 180.0) / 2.0) * 360.0;
		dRoll -= Math.Ceiling (Math.Floor (dRoll / 180.0) / 2.0) * 360.0;
		dYaw -= Math.Ceiling (Math.Floor (dYaw / 180.0) / 2.0) * 360.0;

		//1 и 2 мотор впереди
		//3 и 4 моторы сзади
		throttle = throttle > 0 ? throttle : 0;
		double motor1power = throttle;
		double motor2power = throttle;
		double motor3power = throttle;
		double motor4power = throttle;

		//ограничитель на мощность подаваемую на моторы,
		//чтобы в сумме мощность всех моторов оставалась
		//одинаковой при регулировке
		double powerLimit = throttle > 20 ? 20 : throttle;

		//управление тангажем:
		//на передние двигатели подаем возмущение от регулятора
		//на задние противоположное возмущение
		double pitchForce = - pitchPID.calc (0, dPitch / 180.0);
		pitchForce = pitchForce > powerLimit ? powerLimit : pitchForce;
		pitchForce = pitchForce < -powerLimit ? -powerLimit : pitchForce;
		motor1power +=   pitchForce;
		motor2power +=   pitchForce;
		motor3power += - pitchForce;
		motor4power += - pitchForce;

		//управление креном:
		//действуем по аналогии с тангажем, только регулируем боковые двигатели
		double rollForce = - rollPID.calc (0, dRoll / 180.0);
		rollForce = rollForce > powerLimit ? powerLimit : rollForce;
		rollForce = rollForce < -powerLimit ? -powerLimit : rollForce;
		motor1power +=   rollForce;
		motor2power += - rollForce;
		motor3power += - rollForce;
		motor4power +=   rollForce;

		//управление рысканием:
		double yawForce = yawPID.calc (0, dYaw / 180.0);
		yawForce = yawForce > powerLimit ? powerLimit : yawForce;
		yawForce = yawForce < -powerLimit ? -powerLimit : yawForce;
		motor1power +=   yawForce;
		motor2power += - yawForce;
		motor3power +=   yawForce;
		motor4power += - yawForce;

		GameObject.Find ("Motor1").GetComponent<motorScript>().power = motor1power;
		GameObject.Find ("Motor2").GetComponent<motorScript>().power = motor2power;
		GameObject.Find ("Motor3").GetComponent<motorScript>().power = motor3power;
		GameObject.Find ("Motor4").GetComponent<motorScript>().power = motor4power;
	}

	void AutoHangup()
    {
		//проверка включена ли автовысота

		double verticalSpeed = gps.getYVelocity();
		throttle = 2.48134235*10 - verticalSpeed*5 + ((targetPossition.y- gps.getHeight())*20);//5
		throttle = throttle > 0 ? throttle : 0;
		//для вычесления коэфицента
		//throttle += 2 * verticalSpeed * (1 / (throttle + 1) * -1);

	}
	
	void stabilite()
    {
		if (!stab )
			return;
		stabForward();
		stabHorizntal();
		
	}

	void stabForward()
    {
		if (fwdSpeed < 5 && fwdSpeed > -5)
			targetPitch = -fwdSpeed*3;
		else
			targetPitch = -Math.Sign(fwdSpeed) * 30;
	}

	void stabHorizntal()
	{
		if (driftSpeed < 5 && driftSpeed > -5)
			targetRoll = driftSpeed*3;
		else
			targetRoll = Math.Sign(driftSpeed) * 30;
	}

	void AutoPossition()
    {
		
		if (!autoPilot)
			return;
		//если автопилот включен 
		// берем точку с маршрута
		targetPossition = Gp.route[counterPoint];
		//считаем растояние до точки
		distansToTargetPosition = 
			Math.Sqrt(Math.Pow(Math.Abs(targetPossition.x - gps.getGps().x), 2) +
			Math.Pow(Math.Abs(targetPossition.z - gps.getGps().z), 2));
		//считаем угол на который необходимо повернутся
		targetYaw = (Math.Atan2(targetPossition.x - gps.getGps().x, targetPossition.z - gps.getGps().z)) * 180 / Math.PI;


		//если подлетаем к точке на растояние radiusZone то начинаем слидить за следуйщей
		if (Math.Abs(targetPossition.x - gps.getGps().x) < radiusZone && Math.Abs(targetPossition.z - gps.getGps().z) < radiusZone)
		{
			counterPoint = counterPoint<Gp.route.Count-1 ? counterPoint+1 : counterPoint;
			targetPossition = Gp.route[counterPoint];
			stabHorizntal();
		}


		//SpeedPID.calc(distansToTargetPosition, 0.0)
		//тормозим за 5 метров умнажая скорость на кооэфиуент которой увелечивается при приблтжение к позиции
		//if(targetPossition.y - gps.getGps().y <1)
		//      {

		targetPitch = 30;
		//}
	}

	void AddControls()
	{
		if (autoPilot)
			return;
		//Change throttle to move up or down
		if (Input.GetKey(KeyCode.UpArrow))
			targetPossition.y += 0.2f;

		if (Input.GetKey(KeyCode.DownArrow))
			targetPossition.y -= 0.2f;

		//Steering
		if (Input.GetKey(KeyCode.W))
			targetPitch += 1f;
		else
		{
			if (Input.GetKey(KeyCode.S))
			{
				targetPitch += -1f;
			}
			else
				targetPitch = 0;
		}

		targetPitch = Mathf.Clamp((float)targetPitch, -30f, 30f);

		if (Input.GetKey(KeyCode.D))
		{
			targetRoll += -1f;
		}
		else
		{
			if (Input.GetKey(KeyCode.A))
			{
				targetRoll += 1f;
			}
			else
				targetRoll = 0;
		}
		//Yaw
		targetRoll = Mathf.Clamp((float)targetRoll, -30f, 30f);
		if (Input.GetKey(KeyCode.LeftArrow))
			targetYaw += -1f;
		if (Input.GetKey(KeyCode.RightArrow))
			targetYaw += 1f;
		if (targetYaw < 0)
			targetYaw = 360+targetYaw;
		if(targetYaw>360 )
			targetYaw = targetYaw - 360;

	}
	//как советуют в доке по Unity вычисления проводим в FixedUpdate, а не в Update
	private void Start()
    {
		Gp = Globalparametr.getInstance();
		gps = GetComponent<GPSModul>();
	}
    void FixedUpdate () {
		
		readRotation ();
		stabilize ();
		AutoPossition();
		AutoHangup();
		AddControls();
		stabilite();
	}
	
}


public class PID {
	
	private double P;
	private double I;
	private double D;
	
	private double prevErr;
	private double sumErr;
	
	public PID (double P, double I, double D) {
		this.P = P;
		this.I = I;
		this.D = D;
	}
	
	public double calc (double current, double target) {
		
		double dt = Time.fixedDeltaTime;
		
		double err = target - current;
		this.sumErr += err;
		
		double force = this.P * err + this.I * this.sumErr * dt + this.D * (err - this.prevErr) / dt;
		
		this.prevErr = err;
		return force;
	}

};
