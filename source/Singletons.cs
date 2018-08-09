using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impalers {
	class Singletons {
		static public Random rand = new Random(13/*(System.Int32)(DateTime.Now.Ticks % System.Int32.MaxValue)*/);

		static public byte startClickX, startClickY, endClickX, endClickY;
	}
}
