using System;
using UnityEngine;

namespace Bubblegum 
{

	/// <summary>
	/// Basic delegate to use for state change event
	/// </summary>
	public delegate void StateChanged ();

	/// <summary>
	/// Basic delegate to use for state change event
	/// </summary>
	public delegate void StateValueChanged (object value);

	/// <summary>
	/// Delegate that passes hit information
	/// </summary>
	/// <param name="hit"></param>
	public delegate void Hit (RaycastHit hit);

	/// <summary>
	/// Differect varaible types
	/// </summary>
	public enum VariableType { NONE = -1, BOOL = 0, INT = 1, FLOAT = 2, STRING = 3 }

	/// <summary>
	/// Used as a link between VariableType enum and their actual types
	/// </summary>
	public static class Types { public static readonly Type[] VariableTypes = new Type[] { typeof (bool), typeof (int), typeof (float), typeof (string) }; }

	/// <summary>
	/// A method that can be used for initialization
	/// </summary>
	public enum InitializeMethod { AWAKE, START, ENABLE, NONE }

	/// <summary>
	/// A method choice for updating components
	/// </summary>
	public enum UpdateMethod { UPDATE, FIXED_UPDATE, LATE_UPDATE, NONE }

	/// <summary>
	/// Basic graph types
	/// </summary>
	public enum Graph { LINEAR, EXPONENTIAL }

	/// <summary>
	/// The grounded mode per object (ie. if has two legs or wheels)
	/// </summary>
	public enum GroundedMode { ANY, GROUNDED, NOT_GROUNDED }

	/// <summary>
	/// Different axis in 3 dimensional space
	/// </summary>
	public enum Axis3 { X, Y, Z }

	/// <summary>
	/// What parent to set for a transform
	/// </summary>
	public enum ParentSetting { CHILD, SIBLING, ROOT_CHILD, NONE }

	/// <summary>
	/// Types of colliders
	/// </summary>
	public enum ColliderType { COLLIDER, TRIGGER }

	/// <summary>
	/// Different types of pointer event
	/// </summary>
	public enum PointerEvent { POINTER_DOWN, POINTER_UP }

	/// <summary>
	/// Different directions
	/// </summary>
	public enum Direction2D { UP, DOWN, LEFT, RIGHT }

	/// <summary>
	/// The different comparison types
	/// </summary>
	public enum ComparisonMode { EQUALS, NOT_EQUALS, GREATER_THAN, LESS_THAN }
	
	/// <summary>
	/// The axis selection type
	/// </summary>
	public enum DualAxisType { XY, XZ, YZ }

	/// <summary>
	/// Common input keys
	/// </summary>
	public static class CommonInput
	{
		public const string FIRE_1 = "Fire1", FIRE_2 = "Fire2", FIRE_3 = "Fire3";
		public static readonly string[] fireKeys = { FIRE_1, FIRE_2, FIRE_3 };
		public enum KEYS { FIRE_1, FIRE_2, FIRE_3 };
	}
}