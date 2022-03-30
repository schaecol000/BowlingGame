using System;

public class Frame
{
	// declare variables
	public int roll1;
	public int roll2;
	public int roll3; // applicable only to Frame 10
	public int score;

	public Frame(int roll1, int roll2)
	{
		this.roll1 = roll1;
		this.roll2 = roll2;
		this.roll3 = 0;
	}

	public void SetRoll3(int roll3)
	{
		this.roll3 = roll3;
	}
}