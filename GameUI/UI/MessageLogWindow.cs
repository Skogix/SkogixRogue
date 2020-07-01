using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;

namespace GameUI.UI
{
	// scrollbart window som visar messages (FIFO) via queue
	public class MessageLogWindow : Window
	{
		// antal messages som sparas i loggen
		private static readonly int _maxLines = 100;

		// queue för FIFO-lista
		private readonly Queue<string> _lines;

		// messageConsole displayar aktiva meddelanden
		private readonly ScrollingConsole _messageConsole;

		// scrollbar
		private readonly ScrollBar _messageScrollbar;

		// håll reda på scrollbarpos
		private int _scrollBarCurrentPosition;

		// räkna med tjockleken från window border för att slippa spillover
		private readonly int _windowBorderThickness = 2;

		// nytt fönster med titel - dragbart(?)
		public MessageLogWindow(int width, int height, string title) : base(width, height)
		{
			// sätt bakgrundsfärg
			// Theme.
			_lines = new Queue<string>();
			CanDrag = true;
			Title = title.Align(HorizontalAlignment.Center, width);

			// lägg till messageconsole, sätt position och adda till window
			_messageConsole = new ScrollingConsole(width - _windowBorderThickness, _maxLines);
			_messageConsole.Position = new Point(1, 1);
			_messageConsole.ViewPort = new Rectangle(0, 0, width - 1, height - _windowBorderThickness);

			// skapa scrollbar och sätt den till en eventhandler
			_messageScrollbar = new ScrollBar(Orientation.Vertical, height - _windowBorderThickness);
			_messageScrollbar.Position = new Point(_messageConsole.Width + 1, _messageConsole.Position.X);
			_messageScrollbar.IsEnabled = false;
			_messageScrollbar.ValueChanged += MessageScrollBar_ValueChanged;
			Add(_messageScrollbar);

			// tillåt musinput
			UseMouse = true;

			Children.Add(_messageConsole);
		}

		// custom updatefunktion som uppdaterar scrollbaren
		public override void Update(TimeSpan time)
		{
			base.Update(time);

			// se till att scrollbaren håller reda på positionen på _messageConsole
			if ((_messageConsole.TimesShiftedUp != 0) |
					(_messageConsole.Cursor.Position.Y >= _messageConsole.ViewPort.Height +
						_scrollBarCurrentPosition))
			{
				// sätt scrollbar till enabled när det finns text nog för att scrolla
				_messageScrollbar.IsEnabled = true;

				// se till så vi aldrig kan scrolla mer än hela buffern
				if (_scrollBarCurrentPosition <
						_messageConsole.Height - _messageConsole.ViewPort.Height)
					// spara hur mycket vi har scrollat för att visa hur långt tillbaka 
					// vi kan se
					_scrollBarCurrentPosition += _messageConsole.TimesShiftedUp != 0
						? _messageConsole.TimesShiftedUp
						: 1;

				// scrollbarens max-pos
				_messageScrollbar.Maximum =
					_scrollBarCurrentPosition - _windowBorderThickness;

				// följ cursorn
				_messageScrollbar.Value = _scrollBarCurrentPosition;

				// reseta hur mycket vi shiftar
				_messageConsole.TimesShiftedUp = 0;
			}
		}

		// kontrollerar positionen av messageLogs viewport
		// ändras via scrollbaren
		private void MessageScrollBar_ValueChanged(object sender, EventArgs e)
		{
			_messageConsole.ViewPort = new Rectangle(
				0,
				_messageScrollbar.Value + _windowBorderThickness,
				_messageConsole.Width,
				_messageConsole.ViewPort.Height);
		}

		// lägg till en linje i queuen
		public void Add(string message)
		{
			_lines.Enqueue(message);

			// removea äldsta vid _maxLines
			if (_lines.Count > _maxLines) _lines.Dequeue();
			// flytta cursorn till lista linen och printa
			_messageConsole.Cursor.Position = new Point(1, _lines.Count);
			_messageConsole.Cursor.Print(message + "\n");
		}
	}
}