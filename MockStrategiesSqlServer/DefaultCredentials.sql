CREATE LOGIN StoreMockStrategy WITH PASSWORD = '(Z7SuB4KVdprZBz`'
GO

CREATE USER StoreMockStrategy FOR LOGIN StoreMockStrategy;
GO

GRANT INSERT TO StoreMockStrategy
GO


CREATE LOGIN GetMockStrategy WITH PASSWORD = 'd}~G6<?^+s!f$6~';
GO

CREATE USER GetMockStrategy FOR LOGIN GetMockStrategy;
GO

GRANT SELECT, DELETE TO GetMockStrategy
GO