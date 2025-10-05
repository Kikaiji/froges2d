using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeSwingTrail : Line2D
{
	
	[Export] public int _inBetweenInterval = 10;
	private float _displayTimer = 0f;
	
	public override void _Process(double delta){
		_displayTimer -= (float)delta;
		
		if(_displayTimer <= 0){
			ClearPoints();
		}
	}
	
	public void DisplayTrail(Vector2 startAngle, Vector2 endAngle, float trailWidth){
		ClearPoints();
		
		float posMultiplier = trailWidth * 0.5f;
		
		Width = trailWidth;
		AddPoint(startAngle * posMultiplier);
		
		List<Vector2> inbetweens = CalculateInBetweens(startAngle, endAngle);
		
		for(int i = 0; i < inbetweens.Count; i++){
			AddPoint(inbetweens[i].Normalized() * posMultiplier);
		}
		
		AddPoint(endAngle * posMultiplier);
		
		_displayTimer = 0.1f;
	}
	
	private List<Vector2> CalculateInBetweens(Vector2 startAngle, Vector2 endAngle){
		float radBetween = startAngle.AngleTo(endAngle);
		int numInBetween = Mathf.FloorToInt((radBetween - (radBetween % Mathf.DegToRad(_inBetweenInterval))) / Mathf.DegToRad(_inBetweenInterval));
		GD.Print("Inbetweens " + numInBetween);
		float distanceBetweenInterval = radBetween / numInBetween;
		
		List<Vector2> results = new List<Vector2>();
		
		for(int i = 1; i < numInBetween; i++){
			//Vector2 nextPoint = Vector2.FromAngle(startAngle.Angle() + (distanceBetweenInterval * i));
			Vector2 nextPoint = startAngle.Slerp(endAngle, (float)i/(float)numInBetween);
			results.Add(nextPoint);
		}
		
		return results;
	}
}
