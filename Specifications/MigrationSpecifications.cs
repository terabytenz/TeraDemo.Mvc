using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Machine.Specifications;
using Terademo.Mvc.Data;
using Terademo.Mvc.Data.Migrations;

namespace Specifications
{
	public static class MigrationSpecifications
    {
		[Subject(typeof(MigrationManager))]
	    public class When_apply_migrations
	    {
			static TestDatabase Subject;

		    private Establish context = () => Subject = new TestDatabase();
			private Because of = () => MigrationManager.CreateDatabase(Subject.ConnectionString, typeof(MigrationManager).Assembly, showSql:true);
		    private It should_work = () => Subject.HasTableAsync("Todo").Await().Should().BeTrue();
		    Cleanup cleanup = () => Subject.Dispose();
	    }

		[Subject(typeof(MigrationManager))]
	    public class When_apply_initial_schema
	    {
			static TestDatabase Subject;

		    private Establish context = () => Subject = new TestDatabase();
			private Because of = () => MigrationManager.MigrateUp(Subject.ConnectionString, typeof(MigrationManager).Assembly, targetVersion:1);
		    private It should_work = () => Subject.HasTableAsync("Todo").Await().Should().BeTrue();
		    Cleanup cleanup = () => Subject.Dispose();
	    }

		[Subject(typeof(MigrationManager))]
	    public class When_update_to_user_schema
	    {
			static TestDatabase Subject;

		    private Establish context = () =>
		    {
			    Subject = new TestDatabase();
			    MigrationManager.MigrateUp(Subject.ConnectionString, typeof (MigrationManager).Assembly, targetVersion: 1);

		    };

			private Because of = () => MigrationManager.MigrateUp(Subject.ConnectionString, typeof(MigrationManager).Assembly, targetVersion:2);
			private It should_have_user_table = () => Subject.HasTableAsync("User").Await().Should().BeTrue();
		    Cleanup cleanup = () => Subject.Dispose();
	    }

		[Subject(typeof(MigrationManager))]
	    public class When_rollback_to_initial_schema
	    {
			static TestDatabase Subject;

		    private Establish context = () =>
		    {
			    Subject = new TestDatabase();
			    MigrationManager.MigrateUp(Subject.ConnectionString, typeof (MigrationManager).Assembly, targetVersion: 2);
		    };

			private Because of = () => MigrationManager.MigrateDown(Subject.ConnectionString, typeof(MigrationManager).Assembly, targetVersion:1);
		    private It should_not_have_user_table = () => Subject.HasTableAsync("User").Await().Should().BeFalse();
		    Cleanup cleanup = () => Subject.Dispose();
	    }
    }
}
