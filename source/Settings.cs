using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impalers {
	static class Settings {
		static public byte sizeX = 14;
		static public byte sizeY = 5;
		
		static public Random rand = new Random(13/*(System.Int32)(DateTime.Now.Ticks % System.Int32.MaxValue)*/);
	}
}
