Create table T_PROJECT
(
    ID              UNIQUEIDENTIFIER not null,
    NAME            NVARCHAR(100) not null,
    DESCRIPTION     NVARCHAR(255),
    CREATED_UTC     DATETIME2 not null,
    PRIMARY KEY (ID)
);

