﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Models
{
	public class UserModel : IdentityUser<Guid>
	{
		[Display(Name = "FirstName")]
		[Required(ErrorMessage = "FirstNameRequired")]
		public string FirstName { get; set; }
		[Display(Name = "LastName")]
		[Required(ErrorMessage = "LastNameRequired")]
		public string LastName { get; set; }
		[Display(Name = "Password")]
		[Required(ErrorMessage = "PasswordRequired"), DataType(DataType.Password)]
		public string Password { get; set; }
		[NotMapped]
		public bool IsEmailConfirmed => EmailConfirmed;
		public System.DateTime? EmailConfirmationDate { get; set; }
		public int Score { get; set; }
	}
}
