using GameUI.Entities;
using Microsoft.Xna.Framework;
using System.Text;
// gorogues dice-"emulator"
using GoRogue.DiceNotation;

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

		// "gör en attack" och skickar outcome till message log
		public void Attack(Actor attacker, Actor defender)
		{
			// skapa två messages som beskriver outcome
			StringBuilder attackMessage = new StringBuilder();
			StringBuilder defenseMessage = new StringBuilder();
			
			// damage done och blocks
			int hits = ResolveAttack(attacker, defender, attackMessage);
			int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);
			
			// visa outcome
			GameLoop.UIManager.MessageLog.Add(attackMessage.ToString());
			// om message finns så adda till messagelog
			if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
				GameLoop.UIManager.MessageLog.Add(defenseMessage.ToString());

			int damage = hits - blocks;
			
			// defender tar skada
			ResolveDamage(defender, damage);

		}

		// räkna ut outcome av en attack mot defender
		// använder gorogues random d100-rolls
		// ändrar stringbuldermessaget som visas i messagelog
		private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
		{
			// gör en string som visar attacker och defenders names
			int hits = 0;
			attackMessage.AppendFormat("{0} attacks {1}, ", attacker.Name, defender.Name);
			
			// attackers attackvalue bestämmer hur många d100 som rollas
			for (int dice = 0; dice < attacker.Attack; dice++)
			{
				// rolla en d100 och lägg till resultatet till attackmessage
				int diceOutcome = Dice.Roll("1d100");
				
				// resolvea outcome och regga en hit beroende på attackchance
				if (diceOutcome >= 100 - attacker.AttackChance) hits++;
			}

			return hits;
		}
		
		// räkna ut outcome av att blocka
		// ändrar stringbuildermessage som visas i messagelog
		private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
		{
			int blocks = 0;
			if (hits > 0)
			{
				// skapa en sträng som visas namn och outcome
				attackMessage.AppendFormat("scoring {0} hits.", hits);
				defenseMessage.AppendFormat(" {0} defends and rolls: ", defender.Name);
				
				// defenders defensevalue bestämmer antal d100
				for (int dice = 0; dice < defender.Defense; dice++)
				{
					// en d100 och lägg till resultat till defensemessage
					int diceOutcome = Dice.Roll("1d100");
					
					// resolvea outcome och regga en block beroende på defensechance
					if (diceOutcome >= 100 - defender.DefenseChance) blocks++;
				}

				defenseMessage.AppendFormat("resulting in {0} blocks.", blocks);
			}
			else
			{
				attackMessage.AppendFormat("and misses completely!");
			}

			return blocks;
		}
		
		// räkna ut skadan en defender tar efter en träff och dra av 
		// det från health
		// skicka outcome till messagelog
		private static void ResolveDamage(Actor defender, int damage)
		{
			if (damage > 0)
			{
				defender.Health -= damage;
				GameLoop.UIManager.MessageLog.Add($" {defender.Name} was hit for {damage} damage");
				if (defender.Health <= 0)
				{
					ResolveDeath(defender);
				}
			}
			else
			{
				GameLoop.UIManager.MessageLog.Add($"{defender.Name} blocked all damage!");
			}
		}
		
		// ta bort en actor som har dtt och visa ett message med loot
		private static void ResolveDeath(Actor defender)
		{
			// ta bort entityn från currentmap
			GameLoop.World.CurrentMap.Remove(defender);

			if (defender is Player)
			{
				GameLoop.UIManager.MessageLog.Add($" {defender.Name} was killed.");
			}
			else if (defender is Monster)
			{
				GameLoop.UIManager.MessageLog.Add($"{defender.Name} died and dropped {defender.Gold} gold.");
			}
		}
	}
}