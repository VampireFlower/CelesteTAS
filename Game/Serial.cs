using System;
using System.IO;
using Celeste;
using System.IO.Ports;

namespace TAS {
	public enum SerialButtons {
		//These should be ascending from 0x10 to 0x8000, but somehow the data sent to TAStm32 is completely out of order
		R = 0x40,
		L = 0x20,
		X = 0x100,
		A = 0x10,
		Right = 0x200,
		Left = 0x400,
		Down = 0x800,
		Up = 0x1000,
		Start = 0x2000,
		Select = 0x4000,
		Y = 0x80,
		B = 0x8
	}
	public class Serial {
		public static SerialPort serialPort;
		private readonly byte[] setupData = new byte[] { (byte)'R', (byte)'S', (byte)'A', (byte)'S', 0x80, 0x00 };
		private readonly byte[] frameAdvance = new byte[] { (byte)'L', (byte)'A' };
		private readonly byte[] startFrame = new byte[] { (byte)'A' };
		public int device_vid;
		public int device_pid;
		private bool initialized;
		private bool portExists = true;
		private InputRecord currentInputs;
		public Serial(int vid, int pid) {
			device_vid = vid;
			device_pid = pid;
		}
		public void Update(InputRecord inputs) {
			this.currentInputs = inputs;
			//This shouldn't ever fail but we can't have it crashing the game if it does
			try {
				this.Update();
			}
			catch { }
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
				byte[] setup = new byte[] { (byte)'R', (byte)'S', (byte)'A', (byte)'S', 0x80, 0x00 };
				serialPort.Write(setup, 0, 6);
				this.initialized = true;
			}
			else
				serialPort.Write(frameAdvance, 0, 2);
			serialPort.Write(startFrame, 0, 1);
			serialPort.Write(ConvertCurrentInputs(currentInputs), 0, 2);
		}
		private void PortSetup() {
			//Yeah we shouldn't manually specify a COM port
			//I tried the obvious solution of just connecting to whatever COM port exists, but that broke TAStm32 for some reason
			serialPort = new SerialPort("COM3");
			serialPort.Open();
		}
		private byte[] ConvertCurrentInputs(InputRecord input) {
			ushort frameInputs = 0;
			if (input.HasActions(Actions.Jump))
				frameInputs += (ushort)SerialButtons.A;
			if (input.HasActions(Actions.Jump2))
				frameInputs += (ushort)SerialButtons.X;
			if (input.HasActions(Actions.Dash) || input.HasActions(Actions.Dash2))
				frameInputs += (ushort)SerialButtons.B;
			if (input.HasActions(Actions.Grab))
				frameInputs += (ushort)SerialButtons.Y;
			if (input.HasActions(Actions.Start))
				frameInputs += (ushort)SerialButtons.Start;
			if (input.HasActions(Actions.Left))
				frameInputs += (ushort)SerialButtons.Left;
			if (input.HasActions(Actions.Right))
				frameInputs += (ushort)SerialButtons.Right;
			if (input.HasActions(Actions.Up))
				frameInputs += (ushort)SerialButtons.Up;
			if (input.HasActions(Actions.Down))
				frameInputs += (ushort)SerialButtons.Down;

			byte[] bytes = BitConverter.GetBytes(frameInputs);
			//BitConverter gets bytes in little endian
			return new byte[] { bytes[1], bytes[0] };
		}
	}
}
