BEGIN TRANSACTION;
CREATE TABLE `Kinezet` (
	`ID`	INTEGER NOT NULL,
	`Nev`	TEXT NOT NULL,
	PRIMARY KEY(`ID`)
);

CREATE TABLE `Jatekadat` (
	`FelhasznaloID`	INTEGER,
	`Penz`	INTEGER,
	`AktivSzint`	INTEGER,
	`AktivKinezetID`	INTEGER,
	FOREIGN KEY(`FelhasznaloID`) REFERENCES `Felhasznalo`(`ID`),
	FOREIGN KEY(`AktivKinezetID`) REFERENCES `Kinezet`(`ID`)
);

CREATE TABLE `Felhasznalo` (
	`ID`	INTEGER NOT NULL,
	`Kor`	INTEGER NOT NULL,
	`Nev`	TEXT NOT NULL,
	PRIMARY KEY(`ID`)
);

COMMIT;
