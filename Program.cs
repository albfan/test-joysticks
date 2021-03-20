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

             bool list = false;
             bool dump = false; //show state and controller name
             int index = 0; //current device
             bool checkold = true; //compare last state
             bool axis = false; //due to bad calibration axis can report data all the time


             for (int i = 0; i < args.Length; ++i)
             {
                 string arg = args[i];

                 if (arg == "-l" || arg == "--list")
                 {
                     list = true;
                 }
                 else if (arg == "--no-check-state")
                 {
                     checkold = false;
                 }
                 else if (arg == "-a" || arg == "--axis")
                 {
                     axis = true;
                 }
                 else if (arg == "-i" || arg == "--index")
                 {
                     if (i + 1 >= args.Length)
                     {
                         Logger.Error?.Print(LogClass.Application, $"Invalid option '{arg}'");

                         continue;
                     }

                     string i = args[++i];
                     index = Int32.Parse(i);
                 }
                 else if (arg == "-d" || arg == "--debug")
                 {
                     dump = true;
                 }
                 else
                 {
                     index = Int32.Parse(arg);
                 }
             }

             NativeWindowSettings nativeSettings = NativeWindowSettings.Default;
             nativeSettings.Size = new Vector2i(1280, 720);
             nativeSettings.Title = "Game";
             nativeSettings.WindowState = WindowState.Normal;
             nativeSettings.WindowBorder = WindowBorder.Resizable;
             nativeSettings.NumberOfSamples = 4;

             GameWindow gameWindow = new GameWindow(GameWindowSettings.Default, nativeSettings);

             if (list) {
                 for (int i = 0; i < gameWindow.JoystickStates.Count; i++)
                 {
                     JoystickState state = gameWindow.JoystickStates[i];
                     if (dump)
                         Console.WriteLine($"testing {index} {state}");

                     if (state != null)
                     {
                         var name = GLFW.GetJoystickName(i);

                         Console.WriteLine($"Controller/{i} ({name})");
                     }
                 }
                 gameWindow.ProcessEvents();
                 return;
             }

             while (true) {
                 JoystickState state = gameWindow.JoystickStates[index];
                 if (dump)
                     Console.WriteLine($"Controller/{index} ({state})");

                 if (state != null)
                 {
                     GLFW.GetJoystickHatsRaw(index, out var hatCount);
                     GLFW.GetJoystickAxesRaw(index, out var axisCount);
                     GLFW.GetJoystickButtonsRaw(index, out var buttonCount);
                     var name = GLFW.GetJoystickName(index);

                     Console.WriteLine($"Controller/{index} ({name})");
                     if (list)
                         continue;
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
                 }
                 //Thread.Sleep(2000);
                 gameWindow.ProcessEvents();
                 if (list)
                     break;
             }
        }
    }
}
