using System;
using Celeste;
using System.IO.Ports;

namespace TAS {
	public enum SerialButtons {
		R = 0x10,
		L = 0x20,
		X = 0x40,
		A = 0x80,
		Right = 0x100,
		Left = 0x200,
		Down = 0x400,
		Up = 0x800,
		Start = 0x1000,
		Select = 0x2000,
		Y = 0x4000,
		B = 0x8000
	}
	public class Serial {
		public static SerialPort serialPort;
		private readonly char[] setupData = new char[] { 'S', 'A', 'S', (char)0x80, (char)0x00 };
		private readonly char[] frameAdvance = new char[] { 'L', 'A' };
		public int device_vid;
		public int device_pid;
		private bool initialized;
		private bool portExists = true;
		public Serial(int vid, int pid) {
			device_vid = vid;
			device_pid = pid;
		}
		public void Update() {
			if (!portExists)
				return;
			if (serialPort == null)
				try {
					PortSetup();
				}
				catch (System.IO.IOException e) {
					this.portExists = false;
					Console.WriteLine("No serial port found");
					return;
				}
			if (!this.initialized) {
				serialPort.Write("R");
				serialPort.Write(setupData, 0, 5);
				this.initialized = true;
			}
			else
				serialPort.Write(frameAdvance, 0, 2);

			serialPort.Write(ConvertCurrentInputs(), 0, 3);	
		}
		private void PortSetup() {
			serialPort = new SerialPort();
			serialPort.PortName = "COM1";
			serialPort.BaudRate = 9600;
			serialPort.StopBits = (StopBits)1;
			serialPort.Parity = 0;
			serialPort.DataBits = 8;
			serialPort.Open();

		}
		private byte[] ConvertCurrentInputs() {
			ushort frameInputs = 0;
			if (Input.Jump.Check || Input.MenuConfirm.Check) { frameInputs += (ushort)SerialButtons.A; }
			if (Input.Dash.Check || Input.MenuCancel.Check || Input.Talk.Check) { frameInputs += (ushort)SerialButtons.B; }
			if (Input.Grab.Check) { frameInputs += (ushort)SerialButtons.Y; }
			if (Input.Pause.Check) { frameInputs += (ushort)SerialButtons.Start; }
			if (Input.MenuLeft.Check || Input.MoveX.Value < 0) { frameInputs += (ushort)SerialButtons.Left; }
			if (Input.MenuRight.Check || Input.MoveX.Value > 0) { frameInputs += (ushort)SerialButtons.Right; }
			if (Input.MenuUp.Check || Input.MoveY.Value < 0) { frameInputs += (ushort)SerialButtons.Up; }
			if (Input.MenuDown.Check || Input.MoveY.Value > 0) { frameInputs += (ushort)SerialButtons.Down; }
			byte[] inputBytes = new byte[3];
			inputBytes[0] = (byte)'A';
			Array.Copy(BitConverter.GetBytes(frameInputs), 0, inputBytes, 1, 2);
			return inputBytes;
		}
	}
}
