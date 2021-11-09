﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TicTacToe.Dal;

namespace TicTacToe.Dal.Migrations
{
    [DbContext(typeof(TicTacToeContext))]
    partial class TicTacToeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("TicTacToe.Domain.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Cells")
                        .HasColumnType("BLOB");

                    b.Property<Guid?>("CrossId")
                        .HasColumnType("TEXT");

                    b.Property<byte>("NextTurn")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Winner")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("ZeroId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Game");
                });
#pragma warning restore 612, 618
        }
    }
}
