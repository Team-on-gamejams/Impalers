using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impalers {
	static class Settings {
		static public byte sizeX = 14;
		static public byte sizeY = 5;

		static public byte meatImageZ = 1;
		static public byte stickImageZ = 2;

		static public bool useAnimation = false;

		static Settings() {
			Read();
		}

		static public void Read() {
			string[] lines = System.IO.File.ReadAllText(@".\settings").Split('|');
			if(!byte.TryParse(lines[0], out sizeX))
				sizeX = 14;
			if(!byte.TryParse(lines[1], out sizeY))
				sizeY = 5;
			if(!byte.TryParse(lines[2], out meatImageZ))
				meatImageZ = 1;
			if(!byte.TryParse(lines[3], out stickImageZ))
				stickImageZ = 2;
			if(!bool.TryParse(lines[4], out useAnimation))
				useAnimation = false;
		}

		static public void Save() {
			string text = "";
			foreach(var field in typeof(Settings).GetFields()) {
				if(field.IsStatic && field.IsPublic)
					text += field.GetValue(null).ToString() + '|';
			}
			text = text.Substring(0, text.Length - 1);
			System.IO.File.WriteAllText(@".\settings", text);
		}
	}
}
