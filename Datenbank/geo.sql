CREATE TABLE User(
	nickname char(30),
	password TEXT,
PRIMARY KEY(nickname)
);

CREATE TABLE Minigame(
	id INT AUTO_INCREMENT,
	name char(50),
	PRIMARY KEY(id)
);

CREATE TABLE Score(
	nickname char(30) REFERENCES User(nickname),
	gameId INT REFERENCES Minigame(id),
	points INT,
	playDate DATETIME, /* Muenstermann möchte das nicht playedAtPointInTime nennen.. komisch */
	PRIMARY KEY(nickname,gameId)
);

CREATE TABLE Geocache(
	id INT AUTO_INCREMENT,
	position TEXT,
	secret char(50),
	nextCache INT REFERENCES Geocache(id),
	hint char(50),
	PRIMARY KEY(id)
);

CREATE TABLE Logbook(
	puzzleSolved BOOLEAN,
	message char(100),
	foundDate DATETIME,
	cacheId INT REFERENCES Geocache(id),
	nickname char(30) REFERENCES User(nickname),
	PRIMARY KEY(cacheId,nick)
);


CREATE TABLE PositionLog(
	nickname char(30) REFERENCES User(nickname),
	position TEXT,
	recordedDate DATETIME,
	PRIMARY KEY(nickname,recordedDate)
);