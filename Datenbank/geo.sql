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
	position TEXT,
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

CREATE TABLE positionLog(
	positionLogId INT AUTO_INCREMENT,
	username char(30) REFERENCES User(username),
	position TEXT,
	recordedDate DATETIME,
	PRIMARY KEY(positionLogId)
);

INSERT INTO user 
VALUES ( "testUser", "ccb74d7c1422a9e5efcf8dbfd55fd4bc", "t%,g=6LDai/q_E1M*~YP" )

/* user user berry_pink mellow_yellow */
	username char(30),
	passwordHash TEXT,
	passwordSalt TEXT,