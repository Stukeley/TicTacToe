﻿using System;

namespace TicTacToe.Models
{
	public class TurnModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public UserModel User { get; set; }
		public string Email { get; set; }
		public string IconNumber { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}
}
