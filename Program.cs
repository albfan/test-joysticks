using System;
using OpenTK.Input;
using System.Threading;

namespace test_joysticks
{
    class Program
    {
        static void Main(string[] args)
        {
	     int index = 0; //current device
	     bool dump = false; //show state and controller name
	     bool checkold = true; //compare last state 
	     bool axis = false; //due to bad calibration axis can report data all the time

             JoystickState oldjoystickState = Joystick.GetState(index);
	     while(true) {

                 JoystickState joystickState = Joystick.GetState(index);
		 
                 if (!joystickState.IsConnected) {
                     Console.WriteLine($"Controller {index} not connected");
                     for (int i = 0; i < 90; i++) {
                         if (Joystick.GetState(i).IsConnected) {
                             Console.WriteLine($"Controller/{i} ({GamePad.GetName(i)})");
			     index = i;
	     	             break;
	     	         }
                     }

	             Thread.Sleep(2000);
	             continue;
	         }

                 JoystickCapabilities joystickCapabilities = Joystick.GetCapabilities(index);

	         if (checkold && joystickState.Equals(oldjoystickState)) {
	             continue;
	         }

		 if (dump)
                     Console.WriteLine($"Controller/{index} ({GamePad.GetName(index)})");

	         oldjoystickState = joystickState;
		 if (dump) {
                     Console.WriteLine($"{joystickState.ToString()}");
	             //Thread.Sleep(2000);
		 }

                 //Buttons
                 for (int i = 0; i < joystickCapabilities.ButtonCount; i++) {
                     if (joystickState.IsButtonDown(i)) {
                         Console.WriteLine($"button {i}");
                         Console.WriteLine("pressed");
                     }
                 }

		 if (axis) {
                     //Axis
                     for (int i = 0; i < joystickCapabilities.AxisCount; i++) {
                         if (joystickState.GetAxis(i) > 0.5f && joystickState.GetAxis(i) > 0.1f) {
                             Console.WriteLine($"axis {i}");
                             Console.WriteLine("move");
                         }
                     }
                 }

                 //Hats
                 for (int i = 0; i < joystickCapabilities.HatCount; i++) {
                     JoystickHatState hatState = joystickState.GetHat((JoystickHat)i);
                     string           pos      = null;

                     if (hatState.IsUp)    pos = "Up";
                     if (hatState.IsDown)  pos = "Down";
                     if (hatState.IsLeft)  pos = "Left";
                     if (hatState.IsRight) pos = "Right";
                     if (pos == null)      continue;

                     Console.WriteLine($"hat {i}");
                     Console.WriteLine($"{pos}");
                 }
             }
        }
    }
}
