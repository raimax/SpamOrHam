﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpamOrHam.SqlServer;

#nullable disable

namespace SpamOrHam.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SpamOrHam.Models.DataPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DatasetId")
                        .HasColumnType("int");

                    b.Property<double>("HamProbability")
                        .HasColumnType("float");

                    b.Property<string>("LastRecordContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SpamProbability")
                        .HasColumnType("float");

                    b.Property<double>("TimesOccurredInHam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<double>("TimesOccurredInSpam")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DatasetId");

                    b.ToTable("DataPoints");
                });

            modelBuilder.Entity("SpamOrHam.Models.Dataset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("HamCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<double>("PriorHamProbability")
                        .HasColumnType("float");

                    b.Property<double>("PriorSpamProbability")
                        .HasColumnType("float");

                    b.Property<double>("SpamCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.HasKey("Id");

                    b.ToTable("Datasets");
                });

            modelBuilder.Entity("SpamOrHam.Models.DataPoint", b =>
                {
                    b.HasOne("SpamOrHam.Models.Dataset", "Dataset")
                        .WithMany("DataPoints")
                        .HasForeignKey("DatasetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dataset");
                });

            modelBuilder.Entity("SpamOrHam.Models.Dataset", b =>
                {
                    b.Navigation("DataPoints");
                });
#pragma warning restore 612, 618
        }
    }
}
