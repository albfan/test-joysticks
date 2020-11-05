using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace test_joysticks
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
	     int index = 0; //current device
	     bool dump = false; //show state and controller name
	     bool checkold = true; //compare last state 
	     bool axis = false; //due to bad calibration axis can report data all the time

             NativeWindowSettings nativeSettings = NativeWindowSettings.Default;
             nativeSettings.Size = new Vector2i(1280, 720);
             nativeSettings.Title = "Game";
             nativeSettings.WindowState = WindowState.Normal;
             nativeSettings.WindowBorder = WindowBorder.Resizable;
             nativeSettings.NumberOfSamples = 4;

             GameWindow gameWindow = new GameWindow(GameWindowSettings.Default, nativeSettings);

	     while (true) {
                 for (int i = 0; i < gameWindow.JoystickStates.Count; i++)
                 {
	             JoystickState state = gameWindow.JoystickStates[i];
                     if (state != null)
                     {
                         GLFW.GetJoystickHatsRaw(i, out var hatCount);
                         GLFW.GetJoystickAxesRaw(i, out var axisCount);
                         GLFW.GetJoystickButtonsRaw(i, out var buttonCount);
                         var name = GLFW.GetJoystickName(i);

                         Console.WriteLine($"Controller/{i} ({name})");
                         Console.WriteLine($"{hatCount} {axisCount} {buttonCount}");
                         for (int j = 0; j < hatCount; j++)
			 {
                             Console.WriteLine($"hat{j}: {state.GetHat(j)}");
			 }

                         for (int j = 0; j < axisCount; j++)
			 {
                             Console.WriteLine($"axis{j}: {state.GetAxis(j)}");
			 }
			     
                         for (int j = 0; j < buttonCount; j++)
			 {
                             if (state.IsButtonDown(j)) {
                                 Console.WriteLine($"button {j}: down");
			     }
			 }
                         //Console.WriteLine($"Controller/{i} ({state})");
	                 index = i;
                     }
                 }
		 //Thread.Sleep(2000);
		 gameWindow.ProcessEvents();
             }
        }
    }
}
