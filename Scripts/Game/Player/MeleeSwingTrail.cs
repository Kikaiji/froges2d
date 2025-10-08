using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeSwingTrail : Line2D
{
	
	[Export] public int _inBetweenInterval = 10;
	private float _initDisplayTimer = 0.05f;
	private float _displayTimer = 0f;
	
	private float _removalTimer = 0f;
	private float _initRemovalTimer = 0f;
	
	public override void _Process(double delta){
		_displayTimer -= (float)delta;
		_removalTimer -= (float)delta;
		
		if(_displayTimer <= 0){
			ClearPoints();
		}
		
		if(GetPointCount() > 0 && _removalTimer <= 0){
			RemovePoint(0);
			_removalTimer = _initRemovalTimer;
		}
		
	}
	
	public void DisplayTrail(Vector2 startAngle, Vector2 endAngle, float trailWidth){
		ClearPoints();
		
		float posMultiplier = trailWidth * 0.5f;
		
		Width = trailWidth;
		
		
		List<Vector2> inbetweens = CalculateInBetweens(startAngle, endAngle);
		
		AddPoint(startAngle * Mathf.Lerp(posMultiplier * 0.8f, posMultiplier, ((1)/(float)(inbetweens.Count + 2))));
		
		for(int i = 1; i <= inbetweens.Count; i++){
			AddPoint(inbetweens[i - 1].Normalized() * Mathf.Lerp(posMultiplier * 0.8f, posMultiplier, ((i + 1)/(float)(inbetweens.Count + 2))));
		}
		
		AddPoint(endAngle * posMultiplier);
		AddPoint(endAngle * posMultiplier);
		
		_displayTimer = _initDisplayTimer;
		_initRemovalTimer = _initDisplayTimer / GetPointCount();
		_removalTimer = _initRemovalTimer;
	}
	
	private List<Vector2> CalculateInBetweens(Vector2 startAngle, Vector2 endAngle){
		float radBetween = Mathf.Abs(startAngle.AngleTo(endAngle));
		int numInBetween = Mathf.FloorToInt((radBetween - (radBetween % Mathf.DegToRad(_inBetweenInterval))) / Mathf.DegToRad(_inBetweenInterval));
		//GD.Print("Inbetweens " + numInBetween);
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
