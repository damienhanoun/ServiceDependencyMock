-- Those right doesn't seems to be enough to work :/
CREATE LOGIN MockStrategyUser WITH PASSWORD = 'yourPassword';
GO

CREATE USER MockStrategyUser FOR LOGIN MockStrategyUser;
GO

GRANT SELECT, DELETE ON MockStrategy TO MockStrategyUser 
GO