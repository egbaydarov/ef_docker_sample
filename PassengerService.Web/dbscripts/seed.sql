\connect blogdb

CREATE TABLE "Users"
(
    "Id" serial PRIMARY KEY,
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
    "Id" serial PRIMARY KEY,
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

