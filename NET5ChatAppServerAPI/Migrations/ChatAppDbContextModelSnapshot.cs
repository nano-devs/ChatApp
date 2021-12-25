﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NET5ChatAppServerAPI.Data;

namespace NET5ChatAppServerAPI.Migrations
{
    [DbContext(typeof(ChatAppDbContext))]
    partial class ChatAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("NET5ChatAppServerAPI.Models.Friends", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FriendId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "FriendId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("NET5ChatAppServerAPI.Models.GroupChat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("From")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GroupChats");
                });

            modelBuilder.Entity("NET5ChatAppServerAPI.Models.GroupMember", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "GroupId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("NET5ChatAppServerAPI.Models.Groups", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("NET5ChatAppServerAPI.Models.PendingGroupChat", b =>
                {
                    b.Property<Guid>("GroupChatId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupChatId", "UserId");

                    b.ToTable("PendingGroupChats");
                });

            modelBuilder.Entity("NET5ChatAppServerAPI.Models.PrivateChat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("From")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("To")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PrivateChats");
                });
#pragma warning restore 612, 618
        }
    }
}
