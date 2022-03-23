using UnityEngine;

public class PID
{

	private double P;
	private double I;
	private double D;

	private double prevErr;
	private double sumErr;

	public PID(double P, double I, double D)
	{
		this.P = P;
		this.I = I;
		this.D = D;
	}

	public double calc(double current, double target)
	{

		double dt = Time.fixedDeltaTime;

		double err = target - current;
		this.sumErr += err;

		double force = this.P * err + this.I * this.sumErr * dt + this.D * (err - this.prevErr) / dt;

		this.prevErr = err;
		return force;
	}

};
