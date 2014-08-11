using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;

namespace Terademo.Mvc.Data.Migrations
{
	[Migration(1)]
	public class InitialSchema : Migration
	{
		public override void Up()
		{
			Create.Table("Todo")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Title").AsString()
				.WithColumn("DisplayOrder").AsInt16().WithDefaultValue(0)
				.WithColumn("Description").AsString(4000)
				.WithColumn("IsComplete").AsBoolean()
				.WithColumn("CreatedOn").AsDate()
				.WithColumn("UpdatedAt").AsDateTime();

			Create.Index().OnTable("Todo")
				.OnColumn("DisplayOrder").Descending()
				.OnColumn("CreatedOn").Descending()
				.OnColumn("IsComplete");
		}

		public override void Down()
		{
			Delete.Table("Todo");
		}
	}

	[Migration(2)]
	public class AddUsers : Migration
	{
		public override void Up()
		{
			Create.Table("User")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Username").AsString(50).Indexed()
				.WithColumn("DisplayName").AsString(100);

			Insert.IntoTable("User").Row(new { Username = "", DisplayName = "Unassigned" });

			Alter.Table("Todo")
				.AddColumn("AssignedToUserId").AsInt32().Nullable();

			Update.Table("Todo")
				.Set(new {AssignedToUserId = 1}).AllRows();

			Alter.Table("Todo")
				.AlterColumn("AssignedToUserId").AsInt32().NotNullable().ForeignKey("User", "Id");
		}

		public override void Down()
		{
			Delete.ForeignKey()
				.FromTable("Todo").ForeignColumn("AssignedToUserId")
				.ToTable("User").PrimaryColumn("Id");

			Delete
				.Column("AssignedToUserId").FromTable("Todo");

			Delete
				.Table("User");
		}
	}
}