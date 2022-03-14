\connect blogdb

CREATE TABLE "Users"
(
    "Id" VARCHAR(50) PRIMARY KEY,
    "FirstName"  VARCHAR (50)  NOT NULL,
    "LastName"  VARCHAR (50)  NOT NULL,
    "MiddleName"  VARCHAR (50)  NOT NULL,
    "Email"  VARCHAR (50)  NOT NULL,
    "Phone"  VARCHAR (50)  NOT NULL,
    "Login"  VARCHAR (50)  NOT NULL,
    "ModeratorToken"  VARCHAR (50),
    "CompanyId"  INTEGER,
    "CompanyName"  VARCHAR (50),
    "UserType"  VARCHAR (50),
    "ImageUrl" VARCHAR (300)
);
ALTER TABLE "Users" OWNER TO bloguser;

CREATE TABLE "Comments"
(
    "Id" VARCHAR(50) PRIMARY KEY,
    "FirstName"  VARCHAR (50)  NOT NULL,
    "LastName"  VARCHAR (50)  NOT NULL,
    "MiddleName"  VARCHAR (50)  NOT NULL,
    "Email"  VARCHAR (50)  NOT NULL,
    "Phone"  VARCHAR (50)  NOT NULL,
    "CompanyName"  VARCHAR (50)  NOT NULL,
    "Vehicle"  VARCHAR (50) NOT NULL,
    "Departure"  VARCHAR (50) NOT NULL,
    "Arrival"  VARCHAR (50) NOT NULL,
    "Rating" FLOAT NOT NULL,
    "Feedback" VARCHAR (50) NOT NULL
);
ALTER TABLE "Comments" OWNER TO bloguser;

CREATE TABLE "Wallets"
(
    "UserId" VARCHAR(50) PRIMARY KEY,
    "Money"  INTEGER  NOT NULL
);
ALTER TABLE "Wallets" OWNER TO bloguser;

CREATE TABLE "Tickets"
(
    "Id" INTEGER PRIMARY KEY,
    "UserId"  VARCHAR (50)  NOT NULL,
    "PassengerId"  INT  NOT NULL,
    "RouteId"  INT  NOT NULL,
    "Seat"  VARCHAR (50)  NOT NULL,
    "Price"  VARCHAR (50)  NOT NULL,
    "Status"  VARCHAR (50)  NOT NULL
);
ALTER TABLE "Tickets" OWNER TO bloguser;

CREATE TABLE "Passengers"
(
    "PassengerId" INTEGER PRIMARY KEY,
    "UserId"  VARCHAR (50)  NOT NULL,
    "Name"  VARCHAR (50)  NOT NULL,
    "Phone"  VARCHAR (50)  NOT NULL,
    "Email"  VARCHAR (50)  NOT NULL,
    "Document"  VARCHAR (300)  NOT NULL
);
ALTER TABLE "Passengers" OWNER TO bloguser;