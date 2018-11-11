using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Cryptography;
using Random = System.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bubblegum
{

	/// <summary>
	/// Contains extention methods for Unitys classes
	/// </summary>
	public static class ExtensionMethods
	{

		#region OBJECT

		/// <summary>
		/// Get the default value for the object
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object GetDefault (this Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance (type);

			return null;
		}

		/// <summary>
		/// Try and cast the object to the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public static bool TryCast<T> (this object obj, out T result)
		{
			if (obj is T)
			{
				result = (T) obj;
				return true;
			}

			result = default (T);
			return false;
		}

		/// <summary>
		/// Gets the object as the given type or finds the component matching the type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static T GetAs<T> (this UnityEngine.Object obj)
		{
			T value = default (T);

			if (obj is T)
				value = (T) (object) obj;
			else if (obj is Component)
				value = ((Component) obj).GetComponent<T> ();
			else if (obj is GameObject)
				value = ((GameObject) obj).GetComponent<T> ();

			return value;
		}

		/// <summary>
		/// Validate object as a type of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="property"></param>
		public static T Validate<T> (this UnityEngine.Object obj, ref UnityEngine.Object reference)
		{
			T property = obj.Validate<T> ();

			if (property == null)
				reference = null;

			return property;
		}

		/// <summary>
		/// Validate object as a type of T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="property"></param>
		public static T Validate<T> (this UnityEngine.Object obj)
		{
			if (obj)
			{
				T property = obj.GetAs<T> ();

				if (property == null)
				{
					Debug.LogError ("Cannot convert" + obj.name + " to " + typeof (T));
#if UNITY_EDITOR
					EditorUtility.DisplayDialog ("Interface Error", "Cannot convert" + obj.name + " to " + typeof (T), "Ok");
#endif
					obj = null;
				}

				return property;
			}

			return default (T);
		}

		/// <summary>
		/// Validate a collection of objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objects"></param>
		/// <returns></returns>
		public static T[] Validate<T> (this UnityEngine.Object[] objects)
		{
			if (objects != null)
			{
				T[] convertedObjects = new T[objects.Length];

				for (int i = 0; i < objects.Length; i++)
					convertedObjects[i] = objects[i].Validate<T> (ref objects[i]);

				return convertedObjects;
			}

			return new T[0];
		}

		/// <summary>
		/// Check if the given object is a numeric type
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static bool IsNumericType (this object o)
		{
			if (o == null)
				return false;

			switch (Type.GetTypeCode (o.GetType ()))
			{
				case TypeCode.Byte:
				case TypeCode.SByte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}


		/// <summary>
		/// Compare the two objects
		/// </summary>
		/// <param name="value"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		public static bool Compare (this object value, object checkAgainst, ComparisonMode mode)
		{
			if (value.IsNumericType ())
				return value.CompareNumber (checkAgainst, mode);

			if (value == checkAgainst)
				return mode == ComparisonMode.EQUALS;

			return mode == ComparisonMode.EQUALS && value != null && value.Equals (checkAgainst);
		}

		/// <summary>
		/// Check if the saved number equals the one given
		/// </summary>
		/// <param name="checkAgainst"></param>
		/// <returns></returns>
		private static bool CompareNumber (this object value, object checkAgainst, ComparisonMode compareMode)
		{
			if (!checkAgainst.IsNumericType () || checkAgainst.GetType () != value.GetType ())
				throw new Exception ("Object of type " + checkAgainst.GetType () + " is not a number type or is not supported with " + value.GetType ());

			switch (compareMode)
			{
				case ComparisonMode.LESS_THAN:
					if (checkAgainst is int)
						return (int) checkAgainst < (int) value;
					else
						return (float) checkAgainst < (float) value;

				case ComparisonMode.GREATER_THAN:
					if (checkAgainst is int)
						return (int) checkAgainst > (int) value;
					else
						return (float) checkAgainst > (float) value;
				default:
					if (checkAgainst is int)
						return ((int) value).Equals ((int) checkAgainst);
					else
						return ((float) value).Equals ((float) checkAgainst);

			}
		}

		#endregion

		#region REFLECTION

		/// <summary>
		/// Get all of the members for the given type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static MemberInfo[] GetAllMembers (this Type type)
		{
			List<MemberInfo> members = new List<MemberInfo> ();
			members.AddRange (GetAllFields (type));
			members.AddRange (GetAllProperties (type));

			return members.ToArray ();
		}

		/// <summary>
		/// Get field info from the target
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="currentFields"></param>
		/// <returns></returns>
		public static FieldInfo[] GetAllFields (this Type type, bool inherited = true, List<FieldInfo> currentFields = null)
		{
			//Get fields
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			List<FieldInfo> fields = type.GetFields (flags).ToList ();

			//Merge if current fields exists
			if (currentFields != null)
			{
				currentFields.AddRange (fields);
				fields = currentFields;
			}

			//Recursive return
			if (inherited && type.BaseType != null)
				return GetAllFields (type.BaseType, inherited, fields);
			else
				return fields.ToArray ();
		}

		/// <summary>
		/// Get field info from the target
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="currentProperties"></param>
		/// <returns></returns>
		public static PropertyInfo[] GetAllProperties (this Type type, bool requireRead = true, bool requireWrite = true, List<PropertyInfo> currentProperties = null)
		{
			//Get properties
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			List<PropertyInfo> properties = type.GetProperties (flags).ToList ();

			//Check all properties
			for (int i = 0; i < properties.Count; i++)
				if ((!properties[i].CanRead && requireRead) || (!properties[i].CanWrite && requireWrite))
				{
					properties.RemoveAt (i);
					i--;
				}

			//Merge if current properties exists
			if (currentProperties != null)
			{
				currentProperties.AddRange (properties);
				properties = currentProperties;
			}

			//Recursive return
			if (type.BaseType != null)
				return GetAllProperties (type.BaseType, requireRead, requireWrite, properties);
			else
				return properties.ToArray ();
		}

		/// <summary>
		/// Get all of the methods for the given object
		/// </summary>
		/// <param name="type"></param>
		/// <param name="currentMethods"></param>
		/// <returns></returns>
		public static MethodInfo[] GetAllPublicMethods (this Type type, List<MethodInfo> currentMethods = null)
		{
			//Get methods
			BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
			List<MethodInfo> methods = type.GetMethods (flags).ToList ();

			//Merge if we can
			if (currentMethods != null)
			{
				currentMethods.AddRange (methods);
				methods = currentMethods;
			}

			//Recursive return
			if (type.BaseType != null)
				return GetAllPublicMethods (type.BaseType, methods);
			else
				return methods.ToArray ();
		}

		/// <summary>
		/// Copy all component
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="original"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		public static T CopyComponent<T> (this T destination, T original) where T : Component
		{
			System.Reflection.FieldInfo[] fields = typeof (T).GetAllFields ();

				for (int i = 0; i < fields.Length; i++)
					fields[i].SetValue (destination, fields[i].GetValue (original));

			return destination;
		}

		#endregion

		#region PRIMITIVES

		/// <summary>
		/// Determines if the integer is in the given range
		/// </summary>
		/// <returns><c>true</c> if is in range the specified number min max; otherwise, <c>false</c>.</returns>
		/// <param name="number">Number.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static bool IsRange (this int number, int min, int max)
		{
			return number >= min && number < max;
		}

		/// <summary>
		/// Convert a bool array into a bitmask
		/// </summary>
		/// <param name="bits"></param>
		/// <returns></returns>
		public static int GetBitmask (this bool[] bits)
		{
			return bits.Select ((b, i) => b ? 1 << i : 0).Aggregate ((a, b) => a | b);
		}

		/// <summary>
		/// Convert a bitmask into a boolean array mask
		/// </summary>
		/// <param name="mask"></param>
		/// <returns></returns>
		public static bool[] GetBooleanMask (this int mask)
		{
			return Enumerable.Range (0, 9).Select (b => (mask & (1 << b)) != 0).ToArray ();
		}

		#endregion // PRIMITIVES

		#region TEXT

		/// <summary>
		/// Clear the contents of the stringbuilder
		/// </summary>
		/// <param name="stringBuilder"></param>
		public static void Clear (this StringBuilder stringBuilder)
		{
			stringBuilder.Remove (0, stringBuilder.Length);
		}

		/// <summary>
		/// Increment the number at the end of the text
		/// </summary>
		/// <param name="text"></param>
		/// <param name="increment"></param>
		/// <returns></returns>
		public static string IncrementEndNumber (this string text)
		{
			if (!Char.IsNumber (text[text.Length - 1]))
				return text + " 1";

			int i = Int32.Parse (new string (text.Reverse ().TakeWhile (n => Char.IsNumber (n)).Reverse ().ToArray ()));
			string name = text.Substring (0, text.Length - i.ToString ().Length);
			return name + (++i);
		}

		#endregion

		#region COLLECTIONS

		/// <summary>
		/// Check if the array is null or empty
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty<T> (this T[] array)
		{
			return array == null || array.Length == 0;
		}

		/// <summary>
		/// Concat the specified y array to the x array
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] Concat<T> (this T[] x, T[] y)
		{
			if (x == null)
				throw new ArgumentNullException ("x");
			if (y == null)
				throw new ArgumentNullException ("y");

			int oldLen = x.Length;
			Array.Resize<T> (ref x, x.Length + y.Length);
			Array.Copy (y, 0, x, oldLen, y.Length);

			return x;
		}

		/// <summary>
		/// Shuffle the specified collection.
		/// </summary>
		/// <param name="collection">Collection.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] Shuffle<T> (this T[] collection)
		{
			Random random = new Random ();

			// Knuth shuffle algorithm :: courtesy of Wikipedia :)
			for (int i = 0; i < collection.Length; i++)
			{
				T tmp = collection[i];

				int j = random.Next (i, collection.Length);
				collection[i] = collection[j];
				collection[j] = tmp;
			}

			return collection;
		}

		/// <summary>
		/// Shuffle all the items in the deck with a better algorithm
		/// </summary>
		public static T[] ShuffleBetter<T> (this T[] collection)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider ();
			int n = collection.Length;

			while (n > 1)
			{
				byte[] box = new byte[1];

				do
					provider.GetBytes (box);
				while (!(box[0] < n * (byte.MaxValue / n)));

				int k = box[0] % n;
				n--;

				//Swap
				T value = collection[k];
				collection[k] = collection[n];
				collection[n] = value;
			}

			return collection;
		}

		/// <summary>
		/// Creates a array of unique indexes
		/// </summary>
		/// <returns>The unique random indexes.</returns>
		/// <param name="array">Array.</param>
		/// <param name="legnth">Legnth.</param>
		/// <param name="indexCount">Index count.</param>
		public static int[] CreateUniqueRandomIndexes (this int[] array, int length, int indexCount)
		{
			array = new int[indexCount];
			int randomIndex = 0;
			Random random = new Random ();

			for (int i = 0; i < indexCount; i++)
			{
				do
				{
					randomIndex = random.Next (0, length);
				} while (array.Contains (randomIndex));

				array[i] = randomIndex;
			}

			return array;
		}

		/// <summary>
		/// Converts an int into an array
		/// </summary>
		/// <returns>The array.</returns>
		/// <param name="num">Number.</param>
		public static int[] ToArray (this int num)
		{
			List<int> listOfInts = new List<int> ();

			while (num > 0)
			{
				listOfInts.Add (num % 10);
				num = num / 10;
			}

			return listOfInts.ToArray ();
		}

		/// <summary>
		/// Converts a long into an array of ints
		/// </summary>
		/// <returns>The array.</returns>
		/// <param name="num">Number.</param>
		public static int[] ToArray (this long num)
		{
			List<int> listOfInts = new List<int> ();

			while (num > 0)
			{
				listOfInts.Add ((int) (num % 10));
				num = num / 10;
			}

			return listOfInts.ToArray ();
		}

		/// <summary>
		/// Check if the list contains any item from the array
		/// </summary>
		/// <param name="list"></param>
		/// <param name="arr"></param>
		/// <returns></returns>
		public static bool ContainsAny<T> (this List<T> list, T[] arr)
		{
			for (int i = 0; i < list.Count; i++)
				if (arr.Contains (list[i]))
					return true;

			return false;
		}

		/// <summary>
		/// Initialize a dictionary with the keys and values given
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="keys"></param>
		/// <param name="values"></param>
		public static Dictionary<TKey, TValue> Initialize<TKey, TValue> (this Dictionary<TKey, TValue> dictionary, TKey[] keys, TValue[] values)
		{
			dictionary = new Dictionary<TKey, TValue> ();

			for (int i = 0; i < keys.Length; i++)
			{
				if (!dictionary.ContainsKey (keys[i]))
					dictionary.Add (keys[i], values[i]);
				else
					throw new Exception ("Key " + keys[i] + " already exists within the dictionary");
			}

			return dictionary;
		}

		/// <summary>
		/// Initialize a dictionary of lists with the keys and values given
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="dictionary"></param>
		/// <param name="keys"></param>
		/// <param name="values"></param>
		public static Dictionary<TKey, List<TValue>> Initialize<TKey, TValue> (this Dictionary<TKey, List<TValue>> dictionary, TKey[] keys, TValue[] values)
		{
			dictionary = new Dictionary<TKey, List<TValue>> ();

			for (int i = 0; i < keys.Length; i++)
			{
				if (!dictionary.ContainsKey (keys[i]))
					dictionary.Add (keys[i], new List<TValue> ());

				dictionary[keys[i]].Add (values[i]);
			}

			return dictionary;
		}


		/// <summary>
		/// Swap two items in an array
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="index1"></param>
		/// <param name="index2"></param>
		public static void Swap<T> (this T[] array, int index1, int index2)
		{
			T temp = array[index1];
			array[index1] = array[index2];
			array[index2] = temp;
		}

		/// <summary>
		/// Swap two items in a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="index1"></param>
		/// <param name="index2"></param>
		public static void Swap<T> (this IList<T> list, int index1, int index2)
		{
			T temp = list[index1];
			list[index1] = list[index2];
			list[index2] = temp;
		}

		#endregion // COLLECTIONS

		#region MATH

		/// <summary>
		/// Triangles the number, returning a value that equals the number plus every number smaller than it
		/// is. 4 would equal 4 + 3 + 2 + 1 = 10
		/// </summary>
		/// <returns>The number.</returns>
		/// <param name="num">Number.</param>
		public static int TriangleNumber (this int num)
		{
			int final = 0;

			for (int i = num; i > 0; i--)
			{
				final += num;
			}

			return final;
		}

		/// <summary>
		/// Check if the bitwise mask contains the given value
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool MaskContains (this int mask, int value)
		{
			return ((mask & (1 << value)) > 0);
		}

		/// <summary>
		/// Check the bitwise mask with the given value
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool MaskContains (this IConvertible mask, IConvertible value)
		{
			return ((int) value & (int) mask) == (int) value;
		}

		#endregion // MATH

		#region ENUMS

		/// <summary>
		/// Convert the enum to a Vector3 value
		/// </summary>
		/// <param name="axis"></param>
		/// <returns></returns>
		public static Vector3 ToVector3 (this Axis3 axis)
		{
			switch (axis)
			{
				case Axis3.X:
					return Vector3.right;
				case Axis3.Y:
					return Vector3.up;
				default:
					return Vector3.forward;
			}
		}

		/// <summary>
		/// Return the type value for the variable type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static Type ToType (this VariableType type)
		{
			return Types.VariableTypes[(int) type];
		}

		/// <summary>
		/// Convert the device orientation to a screen orientation
		/// </summary>
		/// <param name="orientation"></param>
		/// <returns></returns>
		public static ScreenOrientation ToScreenOrientation (this DeviceOrientation orientation)
		{
			switch (orientation)
			{
				case DeviceOrientation.LandscapeLeft:
					return ScreenOrientation.Landscape;

				case DeviceOrientation.LandscapeRight:
					return ScreenOrientation.LandscapeRight;

				case DeviceOrientation.Portrait:
					return ScreenOrientation.Portrait;

				case DeviceOrientation.PortraitUpsideDown:
					return ScreenOrientation.PortraitUpsideDown;

				default:
					return ScreenOrientation.Unknown;
			}
		}

		#endregion

		#region GAME_OBJECT

		/// <summary>
		/// Gets a component that extends an interface
		/// </summary>
		/// <returns>The interface.</returns>
		/// <param name="gameObject">Game object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetInterface<T> (this GameObject gameObject) where T : class
		{
			return gameObject.GetComponents<Component> ().OfType<T> ().FirstOrDefault ();
		}

		/// <summary>
		/// Get the index for this component
		/// </summary>
		/// <param name="component"></param>
		/// <returns></returns>
		public static int GetIndex (this Component component)
		{
			List<Component> components = component.gameObject.GetComponents<Component> ().ToList ();
			return components.IndexOf (component);
		}

		/// <summary>
		/// Find a child object with the given tag
		/// </summary>
		/// <param name="component"></param>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static GameObject FindChildWithTag (this Component component, string tag)
		{
			Transform[] children = component.GetComponentsInChildren<Transform> ();

			for (int i = 0; i < children.Length; i++)
				if (children[i].CompareTag (tag))
					return children[i].gameObject;

			return null;
		}

		#endregion // GAME_OBJECT

		#region TRANSFORM/VECTORS

		/// <summary>
		/// Get the path to the transform from the other transform (null means full path)
		/// </summary>
		/// <param name="gameObject"></param>
		/// <returns></returns>
		public static string GetPath (this Transform transform, Transform fromTransform = null)
		{
			string path = transform.name;

			while (transform.parent && transform.parent != fromTransform.parent)
			{
				transform = transform.parent;
				path = transform.name + "/" + path;
			}

			return path;
		}

		/// <summary>
		/// Return the amount of active children for this transform
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static int GetActiveChildCount (this Transform transform)
		{
			int count = 0;

			for (int i = 0; i < transform.childCount; i++)
				if (transform.GetChild (i).gameObject.activeSelf)
					count++;

			return count;
		}

		/// <summary>
		/// Randomize the specified vector3 using the percentage amount given (should be between 0f and 1f)
		/// </summary>
		/// <param name="vector3">Vector3.</param>
		/// <param name="amount">Amount.</param>
		public static Vector3 Randomize (this Vector3 vector3, float amount)
		{
			amount = Mathf.Clamp (amount, 0f, 1f);
			vector3.Set (UnityEngine.Random.Range (vector3.x - (vector3.x * amount), vector3.x + (vector3.x * amount)),
						UnityEngine.Random.Range (vector3.y - (vector3.y * amount), vector3.y + (vector3.y * amount)),
						UnityEngine.Random.Range (vector3.z - (vector3.z * amount), vector3.z + (vector3.z * amount)));
			return vector3;
		}

		/// <summary>
		/// Get the signed angle between this vector and another
		/// </summary>
		/// <param name="vector3"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static float SignedAngle (this Vector3 vector3, Vector3 other)
		{
			// the vector that we want to measure an angle from
			Vector3 referenceRight = Vector3.Cross (Vector3.up, vector3);

			// Get the angle in degrees between 0 and 180
			float angle = Vector3.Angle (other, vector3);

			// Determine if the degree value should be negative.  Here, a positive value
			// from the dot product means that our vector is on the right of the reference vector   
			// whereas a negative value means we're on the left.
			return Mathf.Sign (Vector3.Dot (other, referenceRight)) * angle;
		}

		/// <summary>
		/// Get the signed angle between this quaternion and another
		/// </summary>
		/// <param name="vector3"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static float SignedXAngle (this Quaternion quaternion, Quaternion other)
		{
			//Get a "forward vector" for each rotation
			var forwardA = quaternion * Vector3.forward;
			var forwardB = other * Vector3.forward;

			//Get a numeric angle for each vector, on the Y-Z plane (relative to world forward)
			var angleA = Mathf.Atan2 (forwardA.z, forwardA.y) * Mathf.Rad2Deg;
			var angleB = Mathf.Atan2 (forwardB.z, forwardB.y) * Mathf.Rad2Deg;

			// get the signed difference in these angles
			return Mathf.DeltaAngle (angleA, angleB);
		}

		/// <summary>
		/// Get the signed angle between this quaternion and another
		/// </summary>
		/// <param name="vector3"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static float SignedYAngle (this Quaternion quaternion, Quaternion other)
		{
			//Get a "forward vector" for each rotation
			var forwardA = quaternion * Vector3.forward;
			var forwardB = other * Vector3.forward;

			//Get a numeric angle for each vector, on the X-Z plane (relative to world forward)
			var angleA = Mathf.Atan2 (forwardA.z, forwardA.x) * Mathf.Rad2Deg;
			var angleB = Mathf.Atan2 (forwardB.z, forwardB.x) * Mathf.Rad2Deg;

			// get the signed difference in these angles
			return Mathf.DeltaAngle (angleA, angleB);
		}

		/// <summary>
		/// Get the signed angle between this quaternion and another
		/// </summary>
		/// <param name="vector3"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		public static float SignedZAngle (this Quaternion quaternion, Quaternion other)
		{
			//Get a "forward vector" for each rotation
			var forwardA = quaternion * Vector3.up;
			var forwardB = other * Vector3.up;

			//Get a numeric angle for each vector, on the X-Y plane (relative to world forward)
			var angleA = Mathf.Atan2 (forwardA.y, forwardA.x) * Mathf.Rad2Deg;
			var angleB = Mathf.Atan2 (forwardB.y, forwardB.x) * Mathf.Rad2Deg;

			// get the signed difference in these angles
			return Mathf.DeltaAngle (angleA, angleB);
		}

		/// <summary>
		/// Clamp the specified vector3 and values.
		/// </summary>
		/// <param name="vector3">Vector3.</param>
		/// <param name="values">Values.</param>
		public static Vector3 Clamp (this Vector3 vector3, Vector3 minValues, Vector3 maxValues)
		{
			return vector3 = new Vector3 (Mathf.Clamp (vector3.x, minValues.x, maxValues.x),
										 Mathf.Clamp (vector3.y, minValues.y, maxValues.y),
										 Mathf.Clamp (vector3.z, minValues.z, maxValues.z));
		}

		/// <summary>
		/// Clamps the z value of the given vector3
		/// </summary>
		/// <param name="vector3">Vector3.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static Vector3 ClampZ (this Vector3 vector3, float min, float max)
		{
			return vector3 = new Vector3 (vector3.x, vector3.y, Mathf.Clamp (vector3.z, min, max));
		}

		/// <summary>
		/// Clamps the x value of the given vector3
		/// </summary>
		/// <param name="vector3">Vector3.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static Vector3 ClampX (this Vector3 vector3, float min, float max)
		{
			return vector3 = new Vector3 (Mathf.Clamp (vector3.x, min, max), vector3.y, vector3.z);
		}

		/// <summary>
		/// Clamps the y value of the given vector3
		/// </summary>
		/// <param name="vector3">Vector3.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static Vector3 ClampY (this Vector3 vector3, float min, float max)
		{
			return vector3 = new Vector3 (vector3.x, Mathf.Clamp (vector3.y, min, max), vector3.z);
		}

		/// <summary>
		/// Determines if the vector is within the given range
		/// </summary>
		/// <returns><c>true</c> if is in range the specified vector3 min max; otherwise, <c>false</c>.</returns>
		/// <param name="vector3">Vector3.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Max.</param>
		public static bool IsInRange (this Vector3 vector3, Vector3 min, Vector3 max)
		{
			return vector3.x > min.x && vector3.y > min.y && vector3.z > min.z &&
				vector3.x < max.x && vector3.y < max.y && vector3.z < max.z;
		}

		/// <summary>
		/// Get a random float from the vectors range
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static float Range (this Vector2 vector)
		{
			return UnityEngine.Random.Range (vector.x, vector.y);
		}

		/// <summary>
		/// Round the vector 3 values
		/// </summary>
		/// <param name="round"></param>
		public static Vector3 Round (this Vector3 round, float precision)
		{
			precision = 1f / precision;
			round.x = Mathf.Floor (round.x * precision) / precision;
			round.y = Mathf.Floor (round.y * precision) / precision;
			round.z = Mathf.Floor (round.z * precision) / precision;

			return round;
		}

		/// <summary>
		/// Gets the mid point of all the vectors
		/// </summary>
		/// <returns>The point.</returns>
		/// <param name="positions">Positions.</param>
		public static Vector3 MidPoint (this Vector3[] positions)
		{
			Vector3 midpoint = new Vector3 ();

			for (int i = 0; i < positions.Length; i++)
				midpoint += positions[i];

			return midpoint /= positions.Length;
		}

		/// <summary>
		/// Gets the positions.
		/// </summary>
		/// <returns>The positions.</returns>
		/// <param name="monos">Monos.</param>
		public static Vector3[] GetPositions (this MonoBehaviour[] monos)
		{
			Vector3[] positions = new Vector3[monos.Length];

			for (int i = 0; i < monos.Length; i++)
				positions[i] = monos[i].transform.position;

			return positions;
		}

		/// <summary>
		/// Mimics the transform given
		/// </summary>
		/// <param name="trans">Transform to alter</param>
		/// <param name="transform">Transform to mimic</param>
		public static void MimicTransform (this Transform trans, Transform transform)
		{
			trans.position = transform.position;
			trans.rotation = transform.rotation;
			trans.localScale = transform.localScale;
		}

		/// <summary>
		/// Gets the ground position closest to the given coordinate, null if could not find a collider
		/// </summary>
		/// <returns>The position at coordinate.</returns>
		/// <param name="coordinate">Coordinate.</param>
		public static Vector3? GroundPositionAtCoordinate (this Vector3 coordinate, LayerMask groundMask, float range = 2f)
		{
			Collider[] nearestGround = Physics.OverlapSphere (coordinate, range, groundMask);

			if (nearestGround.Length > 0)
			{
				nearestGround.SortByDistanceFromPosition (coordinate, true);
				return nearestGround[0].ClosestPointOnBounds (coordinate);
			}
			else
				return null;
		}

		/// <summary>
		/// Check if the given vector is similar to this one
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="other"></param>
		/// <param name="tolerance"></param>
		/// <returns></returns>
		public static bool EqualsVector (this Vector3 vector, Vector3 other, float tolerance)
		{
			return 
				Mathf.Abs (vector.x - other.x) < tolerance &&
				Mathf.Abs (vector.y - other.y) < tolerance &&
				Mathf.Abs (vector.z - other.z) < tolerance;
		}

		/// <summary>
		/// Get the distance that the path takes
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static float GetDistance (this Vector3[] path)
		{
			float distance = 0f;

			for (int i = 1; i < path.Length; i++)
				distance += (path[i] - path[i - 1]).sqrMagnitude;

			return Mathf.Sqrt (distance);
		}

		#endregion // TRANSFORM/VECTORS

		#region RENDERING


		/// <summary>
		/// Get the bounds that encompass the renderers in the selected hierarchy
		/// </summary>
		/// <param name="hierarchy"></param>
		/// <returns></returns>
		public static Bounds GetHierarchyBounds (this Transform root)
		{
			Transform[] hierarchy = root.GetComponentsInChildren<Transform> (true);
			Bounds bounds = new Bounds (Vector3.zero, Vector3.zero);

			for (int i = 0; i < hierarchy.Length; i++)
			{
				Renderer renderer = hierarchy[i].GetComponent<Renderer> ();

				if (renderer)
				{
					bounds.Encapsulate (renderer.bounds.min);
					bounds.Encapsulate (renderer.bounds.max);
				}
			}

			return bounds;
		}

		/// <summary>
		/// Merge all of the meshes into one
		/// </summary>
		public static Mesh Combine (this Mesh mergedMesh, Mesh[] meshes, Transform[] transforms)
		{
			CombineInstance[] combineInstances = new CombineInstance[meshes.Length];

			for (int i = 0; i < meshes.Length; i++)
			{
				combineInstances[i].mesh = meshes[i];
				combineInstances[i].transform = transforms[i].localToWorldMatrix;
			}

			mergedMesh.CombineMeshes (combineInstances);

			return mergedMesh;
		}

		#endregion

		#region COLLECTIONS

		/// <summary>
		/// Gets the components from the monobehaviours if they exist
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetComponents<T> (this Component[] components) where T : Component
		{
			return components.GetComponents<T> (components.Length);
		}

		/// <summary>
		/// Gets the components from the components if they exist
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetComponents<T> (this Component[] components, int length) where T : Component
		{
			Stack<T> results = new Stack<T> ();

			for (int i = 0; i < length; i++)
			{
				T result = components[i].GetComponent<T> ();

				if (result != null)
					results.Push (result);
			}

			return results.ToArray ();
		}

		/// <summary>
		/// Gets the components from the monobehaviours if they exist
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> GetComponents<T> (this List<Component> components) where T : Component
		{
			return components.GetComponents<T> (components.Count);
		}

		/// <summary>
		/// Gets the components from the monobehaviours if they exist
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static List<T> GetComponents<T> (this List<Component> components, int length) where T : Component
		{
			List<T> results = new List<T> ();

			for (int i = 0; i < length; i++)
			{
				T result = components[i].GetComponent<T> ();

				if (result != null)
					results.Add (result);
			}

			return results;
		}

		/// <summary>
		/// Gets the components from the monobehaviours if they exist
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetComponentInParents<T> (this Component[] components) where T : Component
		{
			return components.GetComponentInParents<T> (components.Length);
		}

		/// <summary>
		/// Gets the components from the monobehaviours if they exist
		/// </summary>
		/// <returns>The components.</returns>
		/// <param name="components">Components.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetComponentInParents<T> (this Component[] components, int length) where T : Component
		{
			Stack<T> results = new Stack<T> ();

			for (int i = 0; i < length; i++)
			{
				T result = components[i].GetComponentInParent<T> ();

				if (result && !results.Contains (result))
					results.Push (result);
			}

			return results.ToArray ();
		}

		/// <summary>
		/// Removes an element from an array at the given index
		/// </summary>
		/// <returns>The <see cref="``0[]"/>.</returns>
		/// <param name="array">Array.</param>
		/// <param name="index">Index.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] RemoveAt<T> (this T[] array, int index)
		{
			T[] result = new T[array.Length - 1];

			if (index > 0)
				Array.Copy (array, 0, result, 0, index);

			if (index < array.Length - 1)
				Array.Copy (array, index + 1, result, index, array.Length - index - 1);

			return result;
		}

		#endregion // COLLECTIONS

		#region PHYSICS

		/// <summary>
		/// Sorts the by distance by the distance of the objects from the given position
		/// </summary>
		/// <param name="colliders">Colliders.</param>
		/// <param name="position">Position.</param>
		public static void SortByDistanceFromPosition (this Collider[] colliders, Vector3 position, bool closestFirst, int range = -1)
		{
			range = range == -1 ? colliders.Length : range;
			float[] hitDistances = new float[colliders.Length];

			for (int i = 0; i < range; i++)
				hitDistances[i] = (colliders[i].transform.position - position).sqrMagnitude;

			Array.Sort (hitDistances, colliders, 0, range);

			if (!closestFirst)
				Array.Reverse (colliders, 0, range);
		}

		/// <summary>
		/// Checks if the layer mask contains the given layer
		/// </summary>
		/// <returns><c>true</c>, if layer was containsed, <c>false</c> otherwise.</returns>
		/// <param name="LayerMask">Layer mask.</param>
		/// <param name="layer">Layer.</param>
		public static bool ContainsLayer (this LayerMask layerMask, int layer)
		{
			return (layerMask == (layerMask | (1 << layer)));
		}

		/// <summary>
		/// Gets the energy created by the collision
		/// </summary>
		/// <returns>The force.</returns>
		/// <param name="collision">Collision.</param>
		public static float GetForce (this Collision collision, Rigidbody rigidbody)
		{
			return collision.impulse.magnitude / Time.deltaTime;
		}

		/// <summary>
		/// Gets the energy created by the 2D collision
		/// </summary>
		/// <returns>The force.</returns>
		/// <param name="collision">Collision.</param>
		public static float GetForce (this Collision2D collision, Rigidbody2D rb1, Rigidbody2D rb2 = null)
		{
			//TODO when Unity creates collision2D.impulse use that instead
			return Mathf.Abs (Vector2.Dot (collision.contacts[0].normal, collision.relativeVelocity) * rb1.mass);
		}

		/// <summary>
		/// Get the average position of the contact points
		/// </summary>
		/// <param name="contacts"></param>
		/// <returns></returns>
		public static Vector3 GetAveragePosition (this ContactPoint[] contacts) {
			Vector3 average = Vector3.zero;

			for (int i = 0; i < contacts.Length; i++)
				average += contacts[i].point;

			average /= contacts.Length;

			return average;
		}

		/// <summary>
		/// Mimics the 3D rigidbody AddExplosionForce method and adds force to a rigidbody as if it were affected by an explosion
		/// </summary>
		/// <param name="rigidbody"></param>
		/// <param name="explosionForce"></param>
		/// <param name="explosionPosition"></param>
		/// <param name="explosionRadius"></param>
		/// <param name="upwardsModifier"></param>
		/// <param name="mode"></param>
		public static void AddExplosionForce (this Rigidbody2D rigidbody, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0f, ForceMode2D mode = ForceMode2D.Force)
		{
			var explosionDirection = rigidbody.position - explosionPosition;
			var explosionDistance = explosionDirection.magnitude;

			// Normalize without computing magnitude again
			if (upwardsModifier == 0f)
				explosionDirection /= explosionDistance;
			else
			{
				// From Rigidbody.AddExplosionForce doc:
				// If you pass a non-zero value for the upwardsModifier parameter, the direction
				// will be modified by subtracting that value from the Y component of the centre point.
				explosionDirection.y += upwardsModifier;
				explosionDirection.Normalize ();
			}

			float force = Mathf.Lerp (0f, explosionForce, (explosionRadius - explosionDistance) / explosionRadius);
			rigidbody.AddForce (force * explosionDirection, mode);
		}

		/// <summary>
		/// Copy all fixed data across
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void CopyFixed (this Rigidbody to, Rigidbody from)
		{
			to.mass = from.mass;
			to.isKinematic = from.isKinematic;
			to.drag = from.drag;
			to.angularDrag = from.angularDrag;
			to.useGravity = from.useGravity;
			to.interpolation = from.interpolation;
			to.collisionDetectionMode = from.collisionDetectionMode;
			to.constraints = from.constraints;
		}

		/// <summary>
		/// Copy all dynamic variables accross
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void CopyDynamic (this Rigidbody to, Rigidbody from)
		{
			to.velocity = from.velocity;
			to.angularVelocity = from.angularVelocity;
		}

		/// <summary>
		/// Copy values from the hinge joint to our one
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		public static void Copy (this HingeJoint to, HingeJoint from)
		{
			to.anchor = from.anchor;
			to.autoConfigureConnectedAnchor = from.autoConfigureConnectedAnchor;
			to.axis = from.axis;
			to.useSpring = from.useSpring;
			to.spring = from.spring;
			to.useMotor = from.useMotor;
			to.motor = from.motor;
			to.useLimits = from.useLimits;
			to.limits = from.limits;
			to.breakForce = from.breakForce;
			to.breakTorque = from.breakTorque;
			to.enableCollision = from.enableCollision;
			to.enablePreprocessing = from.enablePreprocessing;
			to.massScale = from.massScale;
			to.connectedMassScale = from.connectedMassScale;
		}

		#endregion // PHYSICS

		#region UI

		/// <summary>
		/// Initialize the specified text with the given anchors.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="anchors">Anchors.</param>
		public static void Initialize (this Text text, Rect anchors)
		{
			text.rectTransform.Initialize (anchors);
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = 1;
			text.resizeTextMaxSize = 100;
			text.font = Resources.GetBuiltinResource (typeof (Font), "Arial.ttf") as Font;
		}

		/// <summary>
		/// Initialize the specified rectTransform with given anchors.
		/// </summary>
		/// <param name="rectTransform">Rect transform.</param>
		/// <param name="anchors">Anchors.</param>
		public static void Initialize (this RectTransform rectTransform, Rect anchors)
		{
			rectTransform.anchorMin = new Vector2 (anchors.xMin, anchors.yMax);
			rectTransform.anchorMax = new Vector2 (anchors.xMax, anchors.yMax);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}

		/// <summary>
		/// Converts a rect values from percentages to pixel values.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public static Rect PixelValues (this Rect rect)
		{
			rect.x *= Screen.width;
			rect.y *= Screen.height;
			rect.width *= Screen.width;
			rect.height *= Screen.height;

			return rect;
		}

		/// <summary>
		/// Converts from vector2 from using pixels to percentage of screen size
		/// </summary>
		/// <returns>The from pixels to percentage.</returns>
		/// <param name="vector2">Vector2.</param>
		public static Vector2 ScreenPercentageValues (this Vector2 vector2)
		{
			return new Vector2 (vector2.x / Screen.width, vector2.y / Screen.height);
		}

		/// <summary>
		/// Converts the rect transform to screen bounds.
		/// </summary>
		/// <returns>The transform to screen bounds.</returns>
		/// <param name="rectTransform">Rect transform.</param>
		public static Rect ToScreenBounds (this RectTransform rectTransform)
		{
			RectTransform canvasRect = rectTransform.GetComponentInParent<Canvas> ().GetComponent<RectTransform> ();
			float x = (rectTransform.anchorMin.x * canvasRect.rect.width + rectTransform.offsetMin.x) / canvasRect.rect.width * Screen.width;
			float y = (rectTransform.anchorMin.y * canvasRect.rect.height + rectTransform.offsetMin.y) / canvasRect.rect.height * Screen.height;
			float width = rectTransform.rect.width / canvasRect.rect.width * Screen.width;
			float height = rectTransform.rect.height / canvasRect.rect.height * Screen.height;

			return new Rect (x, y, width, height);
		}

		#endregion // UI

		#region PARTICLES

		/// <summary>
		/// Set the emission rate for a particle system
		/// </summary>
		/// <param name="particleSystem">System to set emission for</param>
		/// <param name="emissionRate">New rate</param>
		public static void SetEmissionRate (this ParticleSystem particleSystem, float emissionRate)
		{
#if UNITY_5_5_OR_NEWER
			var emission = particleSystem.emission;
			var rate = emission.rateOverTime;
			rate.constantMax = emissionRate;
			emission.rateOverTime = rate;
#elif UNITY_5_3_OR_NEWER
			var emission = particleSystem.emission;
			var rate = emission.rate;
			rate.constantMax = emissionRate;
			emission.rate = rate;
#else
			particleSystem.emissionRate = emissionRate;
#endif
		}

		#endregion

		#region AUDIO

		/// <summary>
		/// Copy settings across audio source
		/// </summary>
		/// <param name="to"></param>
		/// <param name="from"></param>
		public static void CopySettings (this AudioSource to, AudioSource from)
		{
			to.outputAudioMixerGroup = from.outputAudioMixerGroup;
			to.volume = from.volume;
			to.loop = from.loop;
			to.mute = from.mute;
			to.bypassEffects = from.bypassEffects;
			to.playOnAwake = from.playOnAwake;
		}

		#endregion

		#region SERIALIZATION

#if UNITY_EDITOR

		/// <summary>
		/// Get the value using the property path from the array
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		private static object GetValueFromArray (System.Collections.IList array, string propertyPath)
		{
			int index = Convert.ToInt32 (new string (propertyPath.Where (c => char.IsDigit (c)).ToArray ()));

			if (index < array.Count)
				return array[index];
			else
				return null;
		}

		/// <summary>
		/// Find the target object for the property
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializedProperty"></param>
		/// <returns></returns>
		public static object FindPropertyObject <T>(this SerializedProperty serializedProperty)
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			object target = serializedProperty.serializedObject.targetObject;
			string propertyPath = serializedProperty.propertyPath;
			propertyPath = propertyPath.Replace (".Array.data", ".");
			string[] splitPath = propertyPath.Split ('.');

			for (int i = 0; i < splitPath.Length; i++)
			{
				//Break if target is null
				if (target == null)
					return null;

				//Array
				var array = target as System.Collections.IList;

				if (array != null)
				{
					target = GetValueFromArray (array, splitPath[i]);
					continue;
				}

				//Property
				PropertyInfo property = target.GetType ().GetProperty (splitPath[i], flags);

				if (property != null)
				{
					target = property.GetValue (target, null);
					continue;
				}

				//Field
				FieldInfo field = target.GetType ().GetField (splitPath[i], flags);

				if (field != null)
					target = field.GetValue (target);
				else
					throw new Exception ("Property in " + target + " cannot be found");					
			}

			return target;
		}

#endif

		#endregion
	}
}