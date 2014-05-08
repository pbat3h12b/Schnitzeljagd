/*André Münstermann*/

CREATE TABLE User(
nickname char(30),
passwort char(30),
email char(50),
PRIMARY KEY(nickname)
);

CREATE TABLE Minispiel(
	id INT AUTO_INCREMENT,
	name char(50),
	PRIMARY KEY(id)
);

CREATE TABLE Score(
	nickname char(30) REFERENCES User(nickname),
	spielID INT REFERENCES Minispiel(id),
	punkte INT,
	datum DATETIME,
	PRIMARY KEY(nickname,spielID)
);

CREATE TABLE Cache(
	id INT AUTO_INCREMENT,
	ort char(50),
	secret char(50),
	naechsterCache INT REFERENCES Cache(id),
	hint char(50),
	PRIMARY KEY(id)
);

CREATE TABLE Logbuch(
	raetselGeloest BOOLEAN,
	nachricht char(100),
	fundDatum DATETIME,
	cacheID INT REFERENCES Cache(id),
	nick char(30) REFERENCES User(nickname),
	PRIMARY KEY(cacheID,nick)
);


CREATE TABLE PosLog(
	nick char(30) REFERENCES User(nickname),
	position char(60),
	datum DATETIME,
	PRIMARY KEY(nick,datum)
);

/*Andre Münstermamn*/


