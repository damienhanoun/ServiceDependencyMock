CREATE LOGIN MockStrategyUser WITH PASSWORD = 'd}~G6<?^+s!f$6~';
GO

CREATE USER MockStrategyUser FOR LOGIN MockStrategyUser;
GO

GRANT SELECT, DELETE TO MockStrategyUser
GO