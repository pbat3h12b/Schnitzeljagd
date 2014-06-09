CREATE TABLE user(
	username char(30),
	passwordHash TEXT,
	passwordSalt TEXT,
	PRIMARY KEY(username)
);

CREATE TABLE minigame(
	id INT AUTO_INCREMENT,
	name char(50),
	PRIMARY KEY(id)
);

CREATE TABLE score(
	scoreId INT AUTO_INCREMENT,
	username char(30) REFERENCES User(username),
	gameId INT REFERENCES Minigame(id),
	points INT,
	playDate DATETIME, /* Muenstermann möchte das nicht playedAtPointInTime nennen.. komisch */
	PRIMARY KEY(scoreId)
);

CREATE TABLE geocache(
	geocacheId INT AUTO_INCREMENT,
	latitude FLOAT,
	longitude FLOAT,
	secret char(50),
	nextCache INT REFERENCES Geocache(id),
	hint char(50),
	PRIMARY KEY(geocacheId)
);

CREATE TABLE logbook(
	logbookId INT AUTO_INCREMENT,
	puzzleSolved BOOLEAN,
	message char(100),
	foundDate DATETIME,
	cacheId INT REFERENCES Geocache(id),
	username char(30) REFERENCES User(username),
	PRIMARY KEY(logbookId)
);

CREATE TABLE positionlog(
	positionLogId INT AUTO_INCREMENT,
	username char(30) REFERENCES User(username),
	latitude FLOAT,
	longitude FLOAT,
	recordedDate DATETIME,
	PRIMARY KEY(positionLogId)
);

/* user user berry_pink mellow_yellow */