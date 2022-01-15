﻿// <auto-generated />
using System;
using ChatApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatApp.Api.Migrations
{
    [DbContext(typeof(ChatAppDbContext))]
    [Migration("20220115083644_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("ChatApp.Api.Models.Friend", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FriendId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("ChatApp.Api.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UniqueGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("UniqueGuid");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ChatApp.Api.Models.GroupMember", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("ChatApp.Api.Models.GroupMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MessageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SendToUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("MessageId");

                    b.HasIndex("SendToUserId");

                    b.ToTable("GroupMessages");
                });

            modelBuilder.Entity("ChatApp.Api.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("PostedByUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SentDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PostedByUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatApp.Api.Models.PrivateMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MessageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SendToUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("SendToUserId");

                    b.ToTable("PrivateMessages");
                });

            modelBuilder.Entity("ChatApp.Api.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ChatApp.Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PublicName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UniqueGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("UniqueGuid");

                    b.HasIndex("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatApp.Api.Models.Friend", b =>
                {
                    b.HasOne("ChatApp.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Api.Models.User", "Friends")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Friends");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Api.Models.GroupMember", b =>
                {
                    b.HasOne("ChatApp.Api.Models.Group", "Group")
                        .WithMany("GroupMembers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Api.Models.User", "User")
                        .WithMany("GroupMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Api.Models.GroupMessage", b =>
                {
                    b.HasOne("ChatApp.Api.Models.Group", "Group")
                        .WithMany("Messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Api.Models.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Api.Models.User", "SendToUser")
                        .WithMany()
                        .HasForeignKey("SendToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Message");

                    b.Navigation("SendToUser");
                });

            modelBuilder.Entity("ChatApp.Api.Models.Message", b =>
                {
                    b.HasOne("ChatApp.Api.Models.User", "PostedByUser")
                        .WithMany("Messages")
                        .HasForeignKey("PostedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PostedByUser");
                });

            modelBuilder.Entity("ChatApp.Api.Models.PrivateMessage", b =>
                {
                    b.HasOne("ChatApp.Api.Models.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Api.Models.User", "SendToUser")
                        .WithMany("PrivateMessages")
                        .HasForeignKey("SendToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("SendToUser");
                });

            modelBuilder.Entity("ChatApp.Api.Models.RefreshToken", b =>
                {
                    b.HasOne("ChatApp.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Api.Models.User", b =>
                {
                    b.HasOne("ChatApp.Api.Models.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ChatApp.Api.Models.Group", b =>
                {
                    b.Navigation("GroupMembers");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ChatApp.Api.Models.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("GroupMembers");

                    b.Navigation("Messages");

                    b.Navigation("PrivateMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
