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
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace GrblStreamSharp
{
	public class GrblStreamer
	{
		const int RX_BUFFER_SIZE = 128;
		SerialPort port;
		StreamReader reader;
		
		string file;
		string portname;
		
		public GrblStreamer (string file, string port)
		{
			reader = new StreamReader(file);
			this.port = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
			this.file = file;
			this.portname = port;
		}
		
		
		public void Run()
		{
			port.Open ();
			port.Write ("\r\n\r\n");
			Thread.Sleep(2000);
			
			port.ReadExisting();
			
			int l_count = 0;
			int g_count = 0;
			List<int> c_line = new List<int>();
			
			Console.WriteLine("Streaming " + file + " to " + portname);

			string line;
			while((line = reader.ReadLine()) != null)
			{
				l_count += 1;
				string l_block = line.Trim();
				c_line.Add(l_block.Length + 1);
				string grbl_out = "";
				
				while(c_line.Sum(x => x) >= RX_BUFFER_SIZE - 1 | port.BytesToRead > 0)
				{
        			string out_temp = port.ReadLine().Trim();
					if(out_temp.IndexOf("ok") < 0 && out_temp.IndexOf("error") < 0)
					{
						Console.WriteLine("  Debug: " + out_temp);
					}
					else
					{
            			grbl_out += out_temp;
            			g_count += 1;
            			grbl_out += g_count;
            			c_line.RemoveAt(0);
					}
    
				}
    			Console.WriteLine("SND: " + l_count + " : " + l_block + " BUF: " + c_line.Sum () + " REC: " + grbl_out);
    			port.Write(l_block + '\n');
			}
		}
	}
}

