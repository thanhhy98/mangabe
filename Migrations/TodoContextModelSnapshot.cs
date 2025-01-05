﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication3.Models;

#nullable disable

namespace WebApplication3.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.1.24451.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ArtistManga", b =>
                {
                    b.Property<Guid>("ArtistsId")
                        .HasColumnType("uuid");

                    b.Property<int>("MangasId")
                        .HasColumnType("integer");

                    b.HasKey("ArtistsId", "MangasId");

                    b.HasIndex("MangasId");

                    b.ToTable("ArtistManga");
                });

            modelBuilder.Entity("AuthorManga", b =>
                {
                    b.Property<Guid>("AuthorsId")
                        .HasColumnType("uuid");

                    b.Property<int>("MangasId")
                        .HasColumnType("integer");

                    b.HasKey("AuthorsId", "MangasId");

                    b.HasIndex("MangasId");

                    b.ToTable("AuthorManga");
                });

            modelBuilder.Entity("Manga", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CoverImageUrl")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("Mangas");
                });

            modelBuilder.Entity("WebApplication3.Models.Artist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("WebApplication3.Models.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("WebApplication3.Models.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ChapterNumber")
                        .HasColumnType("integer");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MangaId")
                        .HasColumnType("integer");

                    b.Property<List<string>>("PageUrls")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalPages")
                        .HasColumnType("integer");

                    b.Property<int>("VolumeNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MangaId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("WebApplication3.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("WebApplication3.Models.MangaGenre", b =>
                {
                    b.Property<int>("MangaId")
                        .HasColumnType("integer");

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.HasKey("MangaId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("MangaGenres");
                });

            modelBuilder.Entity("WebApplication3.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApplication3.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("WebApplication3.Models.UserSetting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<bool>("DarkMode")
                        .HasColumnType("boolean");

                    b.Property<string>("PreferedLanguage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("ArtistManga", b =>
                {
                    b.HasOne("WebApplication3.Models.Artist", null)
                        .WithMany()
                        .HasForeignKey("ArtistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Manga", null)
                        .WithMany()
                        .HasForeignKey("MangasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AuthorManga", b =>
                {
                    b.HasOne("WebApplication3.Models.Author", null)
                        .WithMany()
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Manga", null)
                        .WithMany()
                        .HasForeignKey("MangasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplication3.Models.Chapter", b =>
                {
                    b.HasOne("Manga", "Manga")
                        .WithMany("Chapters")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manga");
                });

            modelBuilder.Entity("WebApplication3.Models.MangaGenre", b =>
                {
                    b.HasOne("WebApplication3.Models.Genre", "Genre")
                        .WithMany("MangaGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Manga", "Manga")
                        .WithMany("MangaGenres")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Manga");
                });

            modelBuilder.Entity("WebApplication3.Models.UserRole", b =>
                {
                    b.HasOne("WebApplication3.Models.User", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApplication3.Models.UserSetting", b =>
                {
                    b.HasOne("WebApplication3.Models.User", null)
                        .WithMany("UserSettings")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Manga", b =>
                {
                    b.Navigation("Chapters");

                    b.Navigation("MangaGenres");
                });

            modelBuilder.Entity("WebApplication3.Models.Genre", b =>
                {
                    b.Navigation("MangaGenres");
                });

            modelBuilder.Entity("WebApplication3.Models.User", b =>
                {
                    b.Navigation("UserRoles");

                    b.Navigation("UserSettings");
                });
#pragma warning restore 612, 618
        }
    }
}
