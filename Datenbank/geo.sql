CREATE TABLE User(
	nickname char(30),
	passwordHash TEXT,
	PRIMARY KEY(nickname)
);

CREATE TABLE Minigame(
	id INT AUTO_INCREMENT,
	name char(50),
	PRIMARY KEY(id)
);

CREATE TABLE Score(
	scoreId INT AUTO_INCREMENT,
	nickname char(30) REFERENCES User(nickname),
	gameId INT REFERENCES Minigame(id),
	points INT,
	playDate DATETIME, /* Muenstermann möchte das nicht playedAtPointInTime nennen.. komisch */
	PRIMARY KEY(scoreId)
);

CREATE TABLE Geocache(
	geocacheId INT AUTO_INCREMENT,
	position TEXT,
	secret char(50),
	nextCache INT REFERENCES Geocache(id),
	hint char(50),
	PRIMARY KEY(geocacheId)
);

CREATE TABLE Logbook(
	logbookId INT AUTO_INCREMENT,
	puzzleSolved BOOLEAN,
	message char(100),
	foundDate DATETIME,
	cacheId INT REFERENCES Geocache(id),
	nickname char(30) REFERENCES User(nickname),
	PRIMARY KEY(logbookId)
);

CREATE TABLE PositionLog(
	positionLogId INT AUTO_INCREMENT,
	nickname char(30) REFERENCES User(nickname),
	position TEXT,
	recordedDate DATETIME,
	PRIMARY KEY(positionLogId)
);

/* user user berry_pink mellow_yellow */
