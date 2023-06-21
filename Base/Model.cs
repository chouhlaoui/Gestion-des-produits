﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AppDB : DbContext
{
    public DbSet<Produit> Produits { get; set; }
    public DbSet<Comd> Comds { get; set; }

    public string DbPath { get; }

    public AppDB()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "GestionProduit.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produit>()
            .HasKey(p => p.Code);
        modelBuilder.Entity<Produit>()
            .Property(p => p.Code)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Comd>()
                    .HasKey(p => p.Id);
        modelBuilder.Entity<Comd>()
                    .Property(p => p.Id)
                    .ValueGeneratedOnAdd();
    }

}

public class Produit
{
    public int Code { get; set; }
    public string NomProduit { get; set; }
    public string Delai { get; set; }
    public float Prix { get; set; }
    public int Quantité { get; set; }

}

public class Comd
{
    public int Id { get; set; }
    public string listeArticles { get; set; }
    public string Date { get; set; }
    public float Total { get; set; }
}