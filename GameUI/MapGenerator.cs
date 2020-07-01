using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Rectangle = GoRogue.Rectangle;

namespace GameUI
{
	// https://roguesharp.wordpress.com/2016/03/26/roguesharp-v3-tutorial-simple-room-generation/
	public class MapGenerator
	{
		private Map _map;

		public Map GenerateMap(
			int mapWidth,
			int mapHeight,
			int maxRooms,
			int minRoomSize,
			int maxRoomSize)
		{
			var random = new Random();
			// skapa en top map
			_map = new Map(mapWidth, mapHeight);

			// lista med rum såhär långt
			// Rectangle från GoRogue
			var Rooms = new List<Rectangle>();

			// skapa maxRooms och se till att dem inte överlappar
			for (var i = 0; i < maxRooms; i++)
			{
				// sätt storlek som random inom roomSize
				var newRoomWidth = random.Next(minRoomSize, maxRoomSize);
				var newRoomHeight = random.Next(minRoomSize, maxRoomSize);
				// sätt pos som random inom mapsize
				var newRoomX = random.Next(0, mapWidth - newRoomWidth - 1);
				var newRoomY = random.Next(0, mapHeight - newRoomHeight - 1);
				// använd GoRogue för att skapa en rektangel
				var newRoom =
					new Rectangle(newRoomX, newRoomY, newRoomWidth, newRoomHeight);
				// är den över ett rum som redan finns?
				var newRoomIntersects = Rooms.Any(room => newRoom.Intersects(room));
				// annars adda till listan
				if (!newRoomIntersects) Rooms.Add(newRoom);
			}

			// både för att slippa null-problem och att "överflödiga" tiles är väggar
			// ala dungeon så skapa walls överallt
			FillWithWalls();

			// cutta ut rum som finns i Rooms
			foreach (var room in Rooms) CreateRoom(room);

			// gå igenom hela listan och cutta ut en tunnel mellan rum 1-2, 2-3 osv
			for (var i = 1; i < Rooms.Count; i++)
			{
				// hämta mitten av nuvarande rummet och förra rummet
				Point previousRoomCenter = Rooms[i - 1].Center;
				Point currentRoomCenter = Rooms[i].Center;

				// slumpa 50/50 vilket håll "L"-tunnelt ska "vika av"
				// -------------|        | |
				// -----------| |        | |
				//            | |        | ---------
				//            | |        |----------
				// fooStart, fooEnd, cutAxis
				if (random.Next(1, 2) == 1)
				{
					CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X
						, previousRoomCenter.Y);
					CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y
						, currentRoomCenter.X);
				}
				else
				{
					CreateVerticalTunnel(previousRoomCenter.Y, currentRoomCenter.Y
						, previousRoomCenter.X);
					CreateHorizontalTunnel(previousRoomCenter.X, currentRoomCenter.X
						, currentRoomCenter.Y);
				}
			}

			// spotta ut en hel karta
			return _map;
		}

		// cutta ut en tunnel som går efter x-axis
		private void CreateHorizontalTunnel(int xStart, int xEnd, int yCutPosition)
		{
			// loopa igenom från minsta till högsta nummer
			for (var x = Math.Min(xStart, xEnd); x < Math.Max(xStart, xEnd); x++)
				// samma x-axis men ny y-pos
				CreateFloor(new Point(x, yCutPosition));
		}

		private void CreateVerticalTunnel(int yStart, int yEnd, int xCutPosition)
		{
			for (var y = Math.Min(yStart, yEnd); y < Math.Max(yStart, yEnd); y++)
				CreateFloor(new Point(xCutPosition, y));
		}

		// fyll hela mappen med walls
		private void FillWithWalls()
		{
			for (var i = 0; i < _map.Tiles.Length; i++)
				_map.Tiles[i] = new TileWall();
		}

		// "bygger"(cuttar ut?) ett rum från input
		// golv i mitten, en rad med walls på utsidan
		private void CreateRoom(Rectangle room)
		{
			// lägg golv i mitten (exklusive perimetern)
			for (var x = room.MinExtentX + 1; x < room.MaxExtentX - 1; x++)
			{
				for (var y = room.MinExtentY + 1; y < room.MaxExtentY - 1; y++)
					CreateFloor(new Point(x, y));
			}

			// sätt väggar omkring
			var perimiter = GetBorderCellLocations(room);
			foreach (var location in perimiter) CreateWall(location);
		}

		// returnerar en lista med points som är "inre utsidan" av en rektangel
		private List<Point> GetBorderCellLocations(Rectangle room)
		{
			// sätter sidorna
			var left = room.MinExtentX; // left
			var right = room.MaxExtentX; // right
			var top = room.MinExtentY; // top
			var bottom = room.MaxExtentY; // bottom

			// gör en lista med "raka linjer" från rectangle
			var borderCells = new List<Point>();
			// top
			borderCells.AddRange(GetTileLocationsAlongLine(left, top, right, top));
			// left
			borderCells.AddRange(GetTileLocationsAlongLine(left, top, left, bottom));
			// bottom
			borderCells.AddRange(
				GetTileLocationsAlongLine(left, bottom, right, bottom));
			// right
			borderCells.AddRange(
				GetTileLocationsAlongLine(right, top, right, bottom));

			return borderCells;
		}

		// returnerar en ienum av points som är ett antal punkter i en linje
		// helvetesdrygt jämfört med att bara dra en rak linje men troligen värt att göra redan nu
		private IEnumerable<Point> GetTileLocationsAlongLine(int xStart, int yStart
			, int xEnd, int yEnd)
		{
			// ToDo: Kommer säkert få  det här problemet igen, göra lite helper-functions istället?

			// ser till att linjen inte overflowar totala mapsizen
			xStart = ClampX(xStart);
			yStart = ClampY(yStart);
			xEnd = ClampX(xEnd);
			yEnd = ClampY(yEnd);

			// absoluta värdet av längden på linjen
			var absXLineValue = Math.Abs(xEnd - xStart);
			var absYLineValue = Math.Abs(yEnd - yStart);

			// om start < end, lägg på ett
			// om start > end, dra av ett
			var xChangeValue = xStart < xEnd ? 1 : -1;
			var yChangeValue = yStart < yEnd ? 1 : -1;
			var diff = absXLineValue - absYLineValue;

			// älska oändliga loopar <3
			// ToDo: mindre kärlek till oändliga loopar
			while (true)
			{
				// outputta nuvarande pointen från xstart och ystart
				yield return new Point(xStart, yStart);
				// när xstart och ystart kommer till xend och yend så breaka
				if (xStart == xEnd && yStart == yEnd) break;

				// för att kolla diagonalen så kollar vi mot diffen^2
				// om en sidas diff vs fooStart är större än den andra så
				// "öka" den sidan med changeValue beroende på om valuen är
				// positiv eller negativ
				var diffSquared = 2 * diff;
				if (diffSquared > -absYLineValue)
				{
					diff -= absYLineValue;
					xStart += xChangeValue;
				}

				if (diffSquared < absXLineValue)
				{
					diff += absXLineValue;
					yStart += yChangeValue;
				}
			}
		}

		// sätt x mellan minx och maxx-sidorna på mappen för att slippa
		// outofbounds
		// ToDo: Kolla om sadConsole inte har en clamp-funktion någonstans
		private int ClampX(int x)
		{
			if (x < 0) x = 0;
			else if (x > _map.Width - 1)
				x = _map.Width - 1;
			return x;
			// men snälla sluta försöka skriva allt skit på en rad
			// går inte att läsa // Mvh skogix dagen efter
			// return (x < 0) ? 0 : (x > _map.Width - 1) ? _map.Width - 1 : x;
		}

		private int ClampY(int y)
		{
			if (y < 0) y = 0;
			else if (y > _map.Height - 1) y = _map.Height - 1;
			return y;
		}

		// ToIndex är helper för typ (loc.Y * Width + loc.X)
		private void CreateWall(Point location)
		{
			_map.Tiles[location.ToIndex(_map.Width)] = new TileWall();
		}

		private void CreateFloor(Point location)
		{
			_map.Tiles[location.ToIndex(_map.Width)] = new TileFloor();
		}
	}
}