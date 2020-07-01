using Microsoft.Xna.Framework;

namespace GameUI.Commands
{
	// decouplea keys/input från actions/metoder
	// typ "Up -> Player.MoveBy"
	// göra actions mer generic
	// en lista med commands med historik, så t.ex repeat last action
	// eller undo action
	
	// håller alla generic actions gjorda på entities/tiles osv
	// t.ex combat, movement, uses
	public class CommandManager
	{
		public CommandManager(){}

		private Point _lastMoveActorPoint;
		private Actor _lastMoveActor;
		
		// flytta actor via +/- X/Y
		// returnar true om action utfördes annars false
		public bool MoveActorBy(Actor actor, Point position)
		{
			_lastMoveActor = actor;
			_lastMoveActorPoint = position;
			return actor.MoveBy(position);
		}
		
		// gör sista movet igen sista actors move
		public bool RedoMoveActorBy()
		{
			// kolla så det faktiskt existerar en actor för redo
			if (_lastMoveActor != null)
			{
				return _lastMoveActor.MoveBy(_lastMoveActorPoint);
			}
			else return false;
		}
		
		// undo sista actor movet
		// sen cleara sista undo så det bara går en gång per command
		public bool UndoMoveActorBy()
		{
			// se till att det finns en actor för undo
			if (_lastMoveActor != null)
			{
				// reversea sista movet
				_lastMoveActorPoint = new Point(-_lastMoveActorPoint.X, -_lastMoveActorPoint.Y);

				if (_lastMoveActor.MoveBy(_lastMoveActorPoint))
				{
					// om det gick, sätt lastmoveactorpoint till 0
					_lastMoveActorPoint = new Point(0, 0);
					return true;
				}
				else
				{
					_lastMoveActorPoint = new Point(0, 0);
					return false;
				}
			}

			return false;
		}
	}
}