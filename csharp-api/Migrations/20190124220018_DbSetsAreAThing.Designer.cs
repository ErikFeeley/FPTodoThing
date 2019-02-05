﻿// <auto-generated />
using MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Migrations
{
    [DbContext(typeof(TodoDbContext))]
    [Migration("20190124220018_DbSetsAreAThing")]
    partial class DbSetsAreAThing
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data.TodoDbContext+Todo", b =>
                {
                    b.Property<int>("TodoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.HasKey("TodoId");

                    b.ToTable("Todos");

                    b.HasData(
                        new
                        {
                            TodoId = 1,
                            Description = "Dummy"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
