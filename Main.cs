/*
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;


namespace GrblStreamSharp
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if(args.Length != 2)
			{
				Console.WriteLine("usage: <program> <gcode-file> <serialport>");
				return;
			}
			
			GrblStreamer grbl = new GrblStreamer(args[0], args[1]);
			grbl.Run ();
			
			
			Console.WriteLine("G-code streaming finished!");
			Console.WriteLine("WARNING: Wait until grbl completes buffered g-code blocks before exiting.");
			
			Console.ReadLine();
		}
	}
}
