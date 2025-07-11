SELECT * FROM [yolo].[userRole];
GO
SELECT * FROM [yolo].[user];
GO
SELECT * FROM [yolo].[player];
GO
SELECT * FROM [yolo].[playerRole];
GO
SELECT * FROM [yolo].[playerStatics];
GO
SELECT * FROM [yolo].[poll];
GO
INSERT INTO [yolo].[user] (email, password, username, firstName, middleName, lastName, phoneNo, city, zipCode, state, country ) 
values ('sub@gmail.com', 'abc123','nabin123', 'Nabin', '', 'Khatri', 1111111111, 'Morrisville', 27560, 'NC', 'USA');
GO