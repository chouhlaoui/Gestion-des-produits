CREATE TABLE `produit` (
  `code` int NOT NULL AUTO_INCREMENT,
  `NomProduit` varchar(50) DEFAULT NULL,
  `Delai` date DEFAULT NULL,
  `PrixHT` float DEFAULT NULL,
  `Quantité` int DEFAULT '0',
  PRIMARY KEY (`code`)
);

insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Lait","2023-05-05",1.5,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Pain","2023-04-25",0.7,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Yaourt","2023-12-05",0.6,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Farine","2024-05-05",1.1,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Lait d'amande","2023-06-05",15.3,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Beurre","2024-05-05",0.9,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Café","2023-05-05",0.85,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Tomates","2023-05-05",4.5,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Harissa","2023-05-05",1.5,500);
insert into produit(NomProduit,Delai,PrixHT,Quantité) values ("Glace","2023-05-05",3,500);


CREATE TABLE `commande` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `total` FLOAT NULL,
  `listeDesArticles` LONGTEXT NULL,
  `date` DATE NULL,
  PRIMARY KEY (`Id`));
  
  